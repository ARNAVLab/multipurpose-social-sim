using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IEndDragHandler
{
    [System.Serializable]
    public class DraggableMovedEvent : UnityEvent<Vector2> { }
    public DraggableMovedEvent DraggableMoved { get; private set; }

    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;
    [SerializeField] private Vector2 hotSpot = Vector2.zero;

    private bool isMouseOn = false;

    private void Awake()
    {
        if (DraggableMoved == null)
            DraggableMoved = new DraggableMovedEvent();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && !isMouseOn)
        {
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
        }
    }

    public void OnPointerEnter(PointerEventData ped)
    {
        isMouseOn = true;
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    public void OnPointerExit(PointerEventData ped)
    {
        isMouseOn = false;
        if (!Input.GetMouseButton(0))
        {
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
        }
    }

    public void OnDrag(PointerEventData ped)
    {
        // If the mouse button assigned to PANNING THE CAMERA is also held, don't drag window
        if (Input.GetMouseButton(2))
        {
            return;
        }

        DraggableMoved.Invoke(ped.delta);
    }

    public void OnEndDrag(PointerEventData ped)
    {
        RectTransform baseRect = transform.parent.GetComponent<RectTransform>();
        Vector2 snapDelta = Vector2.zero;

        Vector2 topRightAnchor = baseRect.position + new Vector3(baseRect.sizeDelta.x / 2, baseRect.sizeDelta.y / 2, 0);
        Vector2 botLeftAnchor = baseRect.position + new Vector3(-baseRect.sizeDelta.x / 2, -baseRect.sizeDelta.y / 2, 0);

        // Detect if the panel is off-screen, and if it is, snap it back in bounds
        if (botLeftAnchor.x < 0)
        {
            // Too far left, snap right
            snapDelta.x += -botLeftAnchor.x;
        }
        else if (topRightAnchor.x > Screen.width)
        {
            // Too far right, snap left
            snapDelta.x -= topRightAnchor.x - Screen.width;
        }

        if (botLeftAnchor.y < 0)
        {
            // Too far down, snap up
            snapDelta.y += -botLeftAnchor.y;
        }
        else if (topRightAnchor.y > Screen.height)
        {
            // Too far up, snap down
            snapDelta.y -= topRightAnchor.y - Screen.height;
        }

        DraggableMoved.Invoke(snapDelta);
    }
}
