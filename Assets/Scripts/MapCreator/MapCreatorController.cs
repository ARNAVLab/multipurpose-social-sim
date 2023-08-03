using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MapCreatorController : MonoBehaviour
{
    private static MapCreatorController _instance;

    [SerializeField] private float _cameraMoveSpeed = 2.0f;
    [SerializeField] private float _cameraDragMoveSpeed = 0.2f;
    [SerializeField] private float _cameraZoomSpeed = 0.5f;
    [SerializeField] private float _cameraScrollZoomSpeed = 0.1f;
    [SerializeField] private float _cameraZoomMin = 1.0f;
    [SerializeField] private float _cameraZoomMax = 100.0f;
    
    [SerializeField] private float trackingRate = 10f;
    private static Transform trackedObject;
    private static bool doTracking = false;

    private Controls _controls;
    private Camera _camera;
    private bool _clickLeft = false;
    private bool _dragLeft = false;
    private bool _clickRight = false;
    private bool _clickMiddle = false;
    private bool _controlHold = false;
    private bool _shiftHold = false;
    private Vector2 _mousePosition;
    private Vector2 _clickLeftOrigin;
    private Vector2 _clickRightOrigin;
    private Vector2 _clickMiddleOrigin;
    private Vector2 _cameraMovement;
    private float _cameraZoom;
    private Vector2 _mouseDelta;
    private float _scrollDelta;

    private MapCreator _mapCreator;
    private GridTile _hoveredTile;

    public static UnityEvent _onSelectEvent = new UnityEvent();

    public static MapCreatorController GetInstance()
    {
        return _instance;
    }

    private void Awake()
    {
        _instance = this;
        _controls = new Controls();
        _camera = GetComponent<Camera>();
        _mapCreator = FindObjectOfType<MapCreator>();
    }

    private void Update()
    {
        _mousePosition = _camera.ScreenToWorldPoint(_controls.player.mousePosition.ReadValue<Vector2>());
        _cameraMovement = _controls.player.wasd.ReadValue<Vector2>();
        _cameraZoom = _controls.player.zoom.ReadValue<float>();
        _mouseDelta += _controls.player.mouseDelta.ReadValue<Vector2>();
        _scrollDelta += _controls.player.scroll.ReadValue<Vector2>().y;
    }

    private void FixedUpdate()
    {
        if (IsCursorOverUIElement()) return;
        
        Vector3 cameraPositionDifference = (Vector3)(Time.deltaTime * _cameraMoveSpeed * _camera.orthographicSize * _cameraMovement);

        this.transform.position = this.transform.position + cameraPositionDifference;

        float oldOrthographicSize = _camera.orthographicSize;

        if (_controlHold)
        {
            if (_clickLeft)
            {
                Vector3 positionDifference = Time.deltaTime * _cameraDragMoveSpeed * _camera.orthographicSize * -_mouseDelta;
                _mapCreator.MoveBackground(-positionDifference);
            }

            if (Mathf.Abs(_scrollDelta) > 0.01f)
            {
                _mapCreator.ResizeBackground(Time.deltaTime * _cameraScrollZoomSpeed * _scrollDelta);
            }
        }
        else
        {
            _camera.orthographicSize = Mathf.Clamp
            (
                _camera.orthographicSize + _camera.orthographicSize * Time.deltaTime * 
                    -(_cameraZoomSpeed * _cameraZoom + _cameraScrollZoomSpeed * _scrollDelta), 
                _cameraZoomMin,
                _cameraZoomMax
            );

            if (Mathf.Abs(_scrollDelta) > 0.01f)
            {
                float orthographicProportion = _camera.orthographicSize / oldOrthographicSize;
                float orthographicDifference = orthographicProportion - 1.0f;
                Vector2 cameraPosition = Vector2.LerpUnclamped(_camera.transform.position, _mousePosition, -orthographicDifference);
                _camera.transform.position = new Vector3(cameraPosition.x, cameraPosition.y, _camera.transform.position.z);
            }
        }

        if (!_dragLeft && _clickLeft && _clickLeftOrigin != _mousePosition)
            _dragLeft = true;
        if (_dragLeft)
        {
            Vector2 cursorDifference = _mousePosition - _clickLeftOrigin;
        }

        if (_clickMiddle)
        {
            Vector3 positionDifference = Time.deltaTime * _cameraDragMoveSpeed * _camera.orthographicSize * -_mouseDelta;
            this.transform.Translate(positionDifference, Space.World);
        }

        _mouseDelta = Vector2.zero;
        _scrollDelta = 0;
    }

    private void LateUpdate()
    {
        if (doTracking)
        {
            Vector3 targetPos = trackedObject.position;
            targetPos.z = transform.position.z;

            Vector3 smoothedPos = Vector3.Lerp(transform.position, targetPos, trackingRate * Time.deltaTime);
            transform.position = smoothedPos;
        }
    }

    /**
     * Returns true if the cursor is currently hovering over a UI element.
     */
    private bool IsCursorOverUIElement()
    {
        int UILayer = LayerMask.NameToLayer("UI");

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        
        foreach (RaycastResult r in raycastResults)
        {
            if (r.gameObject.layer == UILayer)
                return true;
        }
        return false;
    }

    public void HoverTile(GridTile tile)
    {
        _hoveredTile = tile;
        if (_hoveredTile != null && !_controlHold)
        {
            if (_clickLeft)
                _mapCreator.PaintTile(tile);
            else if (_clickRight)
                _mapCreator.EraseTile(tile);
        }
    }

    /**
     * Triggers when left mouse button is initially pressed.
     */
    private void MouseLeftDown(InputAction.CallbackContext context)
    {
        if (IsCursorOverUIElement())
            return;

        _clickLeft = true;
        _clickLeftOrigin = _mousePosition;

        if (_hoveredTile != null && !_controlHold)
        {
            HoverTile(_hoveredTile);
        }
    }
    /**
     * Triggers when left mouse button is released.
     */
    private void MouseLeftUp(InputAction.CallbackContext context)
    {
        if (!_clickLeft)
            return;

        _clickLeft = false;
        SelectionManager.Instance.DeselectAll();
        if (_dragLeft)
        {
            _dragLeft = false;
            if (_controlHold)
            {
                SelectionManager.Instance.SelectHovered();
            }
            else
            {
                SelectionManager.Instance.SelectMoreHovered();
            }
        }
        else
        {
            if (_controlHold)
            {
                SelectionManager.Instance.SelectAnother();
            }
            else
            {
                SelectionManager.Instance.SelectOne();
            }
        }

        _onSelectEvent.Invoke();
    }

    /**
     * Triggers when left mouse button is initially pressed.
     */
    private void MouseRightDown(InputAction.CallbackContext context)
    {
        if (IsCursorOverUIElement())
            return;

        _clickRight = true;
        _clickRightOrigin = _mousePosition;

        if (_hoveredTile != null && !_controlHold)
        {
            HoverTile(_hoveredTile);
        }
    }

    /**
     * Triggers when left mouse button is released.
     */
    private void MouseRightUp(InputAction.CallbackContext context)
    {
        _clickRight = false;
    }

    /**
     * Triggers when middle mouse button is initially pressed.
     */
    private void MouseMiddleDown(InputAction.CallbackContext context)
    {
        _clickMiddle = true;
        Cursor.lockState = CursorLockMode.Locked;
        _clickMiddleOrigin = _mousePosition;Vector2 cursorDifference = _mousePosition - _clickLeftOrigin;
    }
    /**
     * Triggers when middle mouse button is released.
     */
    private void MouseMiddleUp(InputAction.CallbackContext context)
    {
        _clickMiddle = false;
        Cursor.lockState = CursorLockMode.None;
    }

    /**
     * Triggers when ctrl key is initially pressed.
     */
    private void CtrlDown(InputAction.CallbackContext context)
    {
        _controlHold = true;
    }
    /**
     * Triggers when ctrl key is released.
     */
    private void CtrlUp(InputAction.CallbackContext context)
    {
        _controlHold = false;
    }

    /**
     * Triggers when shift key is initially pressed.
     */
    private void ShiftDown(InputAction.CallbackContext context)
    {
        _shiftHold = true;
    }
    /**
     * Triggers when shift key is released.
     */
    private void ShiftUp(InputAction.CallbackContext context)
    {
        _shiftHold = false;
    }

    private void OnEnable()
    {
        _controls.Enable();
        _controls.player.leftMouse.started += MouseLeftDown;
        _controls.player.leftMouse.canceled += MouseLeftUp;
        _controls.player.rightMouse.started += MouseRightDown;
        _controls.player.rightMouse.canceled += MouseRightUp;
        _controls.player.middleMouse.started += MouseMiddleDown;
        _controls.player.middleMouse.canceled += MouseMiddleUp;
        _controls.player.control.started += CtrlDown;
        _controls.player.control.canceled += CtrlUp;
        _controls.player.shift.started += ShiftDown;
        _controls.player.shift.canceled += ShiftUp;
    }
    private void OnDisable()
    {
        _controls.Disable();
        _controls.player.leftMouse.started -= MouseLeftDown;
        _controls.player.leftMouse.canceled -= MouseLeftUp;
        _controls.player.rightMouse.started -= MouseRightDown;
        _controls.player.rightMouse.canceled -= MouseRightUp;
        _controls.player.middleMouse.started -= MouseMiddleDown;
        _controls.player.middleMouse.canceled -= MouseMiddleUp;
        _controls.player.control.started -= CtrlDown;
        _controls.player.control.canceled -= CtrlUp;
        _controls.player.shift.started -= ShiftDown;
        _controls.player.shift.canceled -= ShiftUp;
    }
}
