using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SelectionController : MonoBehaviour
{
    private static SelectionController _instance;

    [SerializeField] private float _cameraMoveSpeed = 2.0f;
    [SerializeField] private float _cameraDragMoveSpeed = 0.2f;
    [SerializeField] private float _cameraZoomSpeed = 0.5f;
    [SerializeField] private float _cameraScrollZoomSpeed = 0.1f;
    [SerializeField] private float _cameraZoomMin = 1.0f;
    [SerializeField] private float _cameraZoomMax = 100.0f;
    [SerializeField] private GameObject _selectionBoxPrefab;
    [SerializeField] private Vector3 _selectionBoxDefSize;
    
    [SerializeField] private float trackingRate = 10f;
    private static Transform trackedObject;
    private static bool doTracking = false;

    private Controls _controls;
    private Camera _camera;
    private GameObject _selectionBox;
    private SpriteRenderer _selectionBoxRenderer;
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

    public static UnityEvent _onSelectEvent = new UnityEvent();

    public static SelectionController GetInstance()
    {
        return _instance;
    }

    private void Awake()
    {
        _instance = this;
        _controls = new Controls();
        _camera = GetComponent<Camera>();
        _selectionBox = Instantiate(_selectionBoxPrefab);
        _selectionBoxRenderer = _selectionBox.GetComponent<SpriteRenderer>();
        _selectionBoxRenderer.enabled = false;
    }

    private void Start()
    {
        _selectionBox.transform.localScale = _selectionBoxDefSize;
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

        if (!_dragLeft && _clickLeft && _clickLeftOrigin != _mousePosition)
            _dragLeft = true;
        if (_dragLeft)
        {
            Vector2 cursorDifference = _mousePosition - _clickLeftOrigin;
            _selectionBox.transform.localScale = (Vector3)cursorDifference + Vector3.forward;
            _selectionBox.transform.position = (Vector3)(_clickLeftOrigin + 0.5f * cursorDifference)
                + _selectionBox.transform.position.z * Vector3.forward;
        }
        else
        {
            _selectionBox.transform.position = _mousePosition;
        }

        if (_clickMiddle)
        {
            Vector3 positionDifference = Time.deltaTime * _cameraDragMoveSpeed * _camera.orthographicSize * -_mouseDelta;
            this.transform.Translate(positionDifference, Space.World);

            // The camera was moved manually; disable automatic Actor tracking, if enabled.
            DisableTracking();
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

    public static void EnableTracking(Transform toTrack)
    {
        trackedObject = toTrack;
        doTracking = true;
    }

    public static void DisableTracking()
    {
        trackedObject = null;
        doTracking = false;
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

    /**
     * Triggers when left mouse button is initially pressed.
     */
    private void MouseLeftDown(InputAction.CallbackContext context)
    {
        if (IsCursorOverUIElement())
            return;

        _clickLeft = true;
        _clickLeftOrigin = _mousePosition;
        _selectionBoxRenderer.enabled = true;
        _selectionBox.transform.position = _clickLeftOrigin;
        _selectionBox.transform.localScale = _selectionBoxDefSize;
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
        _selectionBoxRenderer.enabled = false;
        _selectionBox.transform.localScale = new Vector3(0.1f, 0.1f, 1f);

        _onSelectEvent.Invoke();
    }

    /**
     * Triggers when middle mouse button is initially pressed.
     */
    private void MouseMiddleDown(InputAction.CallbackContext context)
    {
        _clickMiddle = true;
        Cursor.lockState = CursorLockMode.Locked;
        _clickMiddleOrigin = _mousePosition;Vector2 cursorDifference = _mousePosition - _clickLeftOrigin;
        _selectionBox.transform.localScale = Vector3.zero;
        _selectionBox.transform.position = (Vector3)(_clickLeftOrigin + 0.5f * cursorDifference)
            + _selectionBox.transform.position.z * Vector3.forward;
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
        _controls.player.middleMouse.started -= MouseMiddleDown;
        _controls.player.middleMouse.canceled -= MouseMiddleUp;
        _controls.player.control.started -= CtrlDown;
        _controls.player.control.canceled -= CtrlUp;
        _controls.player.shift.started -= ShiftDown;
        _controls.player.shift.canceled -= ShiftUp;
    }

    public void SetSelectBoxLayer(string layer)
    {
        _selectionBox.layer = LayerMask.NameToLayer(layer);
    }
}
