using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InteractableUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private CursorAtlas.CursorType hoverIcon;
    [SerializeField] private CursorAtlas.CursorType holdIcon;
    [SerializeField] private bool doInstantDrag;
    protected bool isMouseOn = false;
    protected bool isDragging = false;
    private int myUIElementID = -1;

    [SerializeField] private Vector2 alignment;
    public class DoubleVectorEvent : UnityEvent<Vector2, Vector2> { }
    public DoubleVectorEvent Drag { get; private set; }
    public UnityEvent ReleaseDrag { get; private set; }


    private void Awake()
    {
        if (Drag == null)
            Drag = new DoubleVectorEvent();
        if (ReleaseDrag == null)
            ReleaseDrag = new UnityEvent();
    }

    private void Start()
    {
        myUIElementID = UIManager.GetNextUIElementID();
    }

    // Something to keep in mind is that there may not be a guarantee as to which of PointerEnter and PointerExit run first
    // if the mouse is moved very quickly

    public void OnPointerEnter(PointerEventData ped)
    {
        isMouseOn = true;
        if (!isDragging && UIManager.elementInControl == -1)
        {
            CursorAtlas.instance.SetCursor(hoverIcon, CursorMode.Auto);
            EventSystem.current.pixelDragThreshold = doInstantDrag ? 0 : 10;
        }
    }

    public void OnPointerExit(PointerEventData ped)
    {
        isMouseOn = false;
        if (!isDragging && UIManager.elementInControl == -1)
        {
            CursorAtlas.instance.SetCursor(CursorAtlas.CursorType.DEFAULT, CursorMode.Auto);
            EventSystem.current.pixelDragThreshold = 10;
        }
    }

    public void OnBeginDrag(PointerEventData ped)
    {
        if (UIManager.elementInControl == -1)
            BeginDragInteract();
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        // If other mouse buttons are held, don't interact
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
            return;

        Vector2 resultantDelta = ped.delta;

        resultantDelta.x *= alignment.x;
        resultantDelta.y *= alignment.y;

        Drag.Invoke(resultantDelta, alignment);
    }

    public virtual void OnEndDrag(PointerEventData ped)
    {
        if (UIManager.elementInControl == myUIElementID)
            EndDragInteract();
    }

    private void BeginDragInteract()
    {
        isDragging = true;
        UIManager.elementInControl = myUIElementID;
        CursorAtlas.instance.SetCursor(holdIcon, CursorMode.Auto);
    }

    private void EndDragInteract()
    {
        isDragging = false;
        UIManager.elementInControl = -1;
        if (!isMouseOn)
            CursorAtlas.instance.SetCursor(CursorAtlas.CursorType.DEFAULT, CursorMode.Auto);
        else
            CursorAtlas.instance.SetCursor(hoverIcon, CursorMode.Auto);

        ReleaseDrag.Invoke();
    }
}
