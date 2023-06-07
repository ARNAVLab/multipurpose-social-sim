using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SelectionController : MonoBehaviour
{
    [SerializeField] private float _cameraMoveSpeed;
    [SerializeField] private float _cameraZoomSpeed;
    [SerializeField] private float _cameraZoomMin;
    [SerializeField] private float _cameraZoomMax;
    [SerializeField] private GameObject _selectionBoxPrefab;
    [SerializeField] private Vector3 _selectionBoxDefSize;

    private Controls _controls;
    private Camera _camera;
    private GameObject _selectionBox;
    private SpriteRenderer _selectionBoxRenderer;
    private bool _clickLeft = false;
    private bool _dragLeft = false;
    private bool _clickRight = false;
    private bool _dragRight = false;
    private bool _clickMiddle = false;
    private bool _dragMiddle = false;
    private bool _controlHold = false;
    private bool _shiftHold = false;
    private bool _altHold = false;
    private Vector2 _mousePosition;
    private Vector2 _clickLeftOrigin;
    private Vector2 _clickRightOrigin;
    private Vector2 _clickMiddleOrigin;
    private Vector2 _cameraMovement;
    private float _cameraZoom;

    public static UnityEvent _onSelectEvent = new UnityEvent();

    private void Awake()
    {
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
        // _cameraZoom = _controls.player.zoom.ReadValue<float>();
    }
    private void FixedUpdate()
    {
        Vector3 cameraPositionDifference = (Vector3)(Time.deltaTime * _cameraMoveSpeed * _camera.orthographicSize * _cameraMovement);

        this.transform.position = this.transform.position + cameraPositionDifference;

        // _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize + 
        //     _camera.orthographicSize * Time.deltaTime * _cameraZoomSpeed * _cameraZoom, 
        //     _cameraZoomMin, _cameraZoomMax);

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
            _selectionBox.transform.position = _mousePosition;;
        }

        if (!_dragRight && _clickRight && _clickRightOrigin != _mousePosition) 
            _dragRight = true;
    }

    private bool IsPointerOverUIElement()
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

    private void MouseLeftDown(InputAction.CallbackContext context)
    {
        if (IsPointerOverUIElement())
            return;

        _clickLeft = true;
        _clickLeftOrigin = _mousePosition;
        _selectionBoxRenderer.enabled = true;
        _selectionBox.transform.position = _clickLeftOrigin;
        _selectionBox.transform.localScale = _selectionBoxDefSize;
    }
    private void MouseLeftUp(InputAction.CallbackContext context)
    {
        //if (IsPointerOverUIElement())
        //    return;

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

    private void CtrlDown(InputAction.CallbackContext context)
    {
        _controlHold = true;
    }
    private void CtrlUp(InputAction.CallbackContext context)
    {
        _controlHold = false;
    }

    private void ShiftDown(InputAction.CallbackContext context)
    {
        _shiftHold = true;
    }
    private void ShiftUp(InputAction.CallbackContext context)
    {
        _shiftHold = false;
    }

    private void OnEnable()
    {
        _controls.Enable();
        _controls.player.leftMouse.started += MouseLeftDown;
        _controls.player.leftMouse.canceled += MouseLeftUp;
        _controls.player.control.started += CtrlDown;
        _controls.player.control.canceled += CtrlUp;
        _controls.player.shift.started += ShiftDown;
        _controls.player.shift.canceled += ShiftUp;
        // _controls.player.alt.started += AltDown;
        // _controls.player.alt.canceled += AltUp;
    }
    private void OnDisable()
    {
        _controls.Disable();
        _controls.player.leftMouse.started -= MouseLeftDown;
        _controls.player.leftMouse.canceled -= MouseLeftUp;
        _controls.player.control.started -= CtrlDown;
        _controls.player.control.canceled -= CtrlUp;
        _controls.player.shift.started -= ShiftDown;
        _controls.player.shift.canceled -= ShiftUp;
        // _controls.player.alt.started -= AltDown;
        // _controls.player.alt.canceled -= AltUp;
    }
}
