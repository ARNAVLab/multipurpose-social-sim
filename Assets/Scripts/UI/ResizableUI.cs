using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// OBSOLETED BY MERGING FUNCTIONALITY WITH DRAGGABLEUI INTO INTERACTABLEUI.
// THIS CLASS WILL BE DELETED IN A FUTURE COMMIT.
public class ResizableUI : InteractableUI
{
    [SerializeField] private Vector2 alignment;

    [System.Serializable]
    public class ResizeableMovedEvent : UnityEvent<Vector2> { }
    public ResizeableMovedEvent ResizeableMoved { get; private set; }
    public UnityEvent ResizableReleased { get; private set; }

    private void Awake()
    {
        if (ResizeableMoved == null)
            ResizeableMoved = new ResizeableMovedEvent();
        if (ResizableReleased == null)
            ResizableReleased = new UnityEvent();
    }

    public override void OnDrag(PointerEventData ped)
    {
        // If other mouse buttons are held, don't interact
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
            return;

        Vector2 resultantDelta = ped.delta;

        resultantDelta.x *= alignment.x;
        resultantDelta.y *= alignment.y;

        ResizeableMoved.Invoke(resultantDelta);
    }

    public override void OnEndDrag(PointerEventData ped)
    {
        base.OnEndDrag(ped);

        ResizableReleased.Invoke();

        //RectTransform baseRect = transform.parent.GetComponent<RectTransform>();
        //Vector2 snapDelta = Vector2.zero;

        //Vector2 topRightAnchor = baseRect.position + new Vector3(baseRect.sizeDelta.x / 2, baseRect.sizeDelta.y / 2, 0);
        //Vector2 botLeftAnchor = baseRect.position + new Vector3(-baseRect.sizeDelta.x / 2, -baseRect.sizeDelta.y / 2, 0);

        //// Detect if the panel is off-screen, and if it is, snap it back in bounds
        //if (botLeftAnchor.x < 0)
        //{
        //    // Too far left, snap right
        //    snapDelta.x += -botLeftAnchor.x;
        //}
        //else if (topRightAnchor.x > Screen.width)
        //{
        //    // Too far right, snap left
        //    snapDelta.x -= topRightAnchor.x - Screen.width;
        //}

        //if (botLeftAnchor.y < 0)
        //{
        //    // Too far down, snap up
        //    snapDelta.y += -botLeftAnchor.y;
        //}
        //else if (topRightAnchor.y > Screen.height)
        //{
        //    // Too far up, snap down
        //    snapDelta.y -= topRightAnchor.y - Screen.height;
        //}

        //ResizeableMoved.Invoke(snapDelta);
    }
}
