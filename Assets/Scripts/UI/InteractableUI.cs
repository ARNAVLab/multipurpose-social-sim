using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/**
 * Manages pointer/cursor interactions with the attached UI GameObject, in particular dealing with dragging and releasing.
 * No functionality is provided -- instead, UnityEvents are invoked when actions to interact with this component are taken.
 */
public class InteractableUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // Whether the pointer is currently hovering over the object.
    protected bool isMouseOn = false;
    [Tooltip("The cursor image to switch to when this object is hovered over (as defined in CursorAtlas).")]
    [SerializeField] private string hoverIcon;

    // Whether the pointer is currently in 'dragging mode'.
    protected bool isDragging = false;
    [Tooltip("Should there be some delay between clicking the object and 'dragging' being initiated?")]
    [SerializeField] private bool doInstantDrag;
    [Tooltip("The cursor image to switch to when this object is dragged on (as defined in CursorAtlas).")]
    [SerializeField] private string holdIcon;

    public class DoubleVectorEvent : UnityEvent<Vector2, Vector2> { }
    // UnityEvent invoked whenever the Interactable is dragged. Two Vector2 parameters provided for passing delta and alignment.
    public DoubleVectorEvent Drag { get; private set; }
    // UnityEvent invoked whenever the Interactable stops being dragged. No parameters provided.
    public UnityEvent ReleaseDrag { get; private set; }
    [Tooltip("Multipliers applied to the pointer delta whenever a Drag is detected. Useful for zeroing out a particular axis.")]
    [SerializeField] private Vector2 alignment;

    // A unique ID assigned to this InteractableUI component.
    private int myUIElementID = -1;

    /**
     * On awake, initialize the UnityEvents that will drive interaction functionality.
     */
    private void Awake()
    {
        if (Drag == null)
            Drag = new DoubleVectorEvent();
        if (ReleaseDrag == null)
            ReleaseDrag = new UnityEvent();
    }

    /**
     * On startup, obtains a unique ID from the UIManager for the purposes of allowing only one object to be interacted with at a time.
     */
    private void Start()
    {
        myUIElementID = UIManager.GetNextUIElementID();
    }

    // Something to keep in mind is that there may not be a guarantee as to which of PointerEnter and PointerExit run first
    // if the mouse is moved very quickly. Should be addressed later

    /**
     * Executes whenever the mouse pointer enters the bounds of this Interactable.
     * 
     * @param ped is the pointer data provided by Unity.
     */
    public void OnPointerEnter(PointerEventData ped)
    {
        isMouseOn = true;

        // If this Interactable is not being dragged, and no others are being interacted with...
        if (!isDragging && UIManager.elementInControl == -1)
        {
            CursorAtlas.instance.SetCursor(hoverIcon, CursorMode.Auto);
            EventSystem.current.pixelDragThreshold = doInstantDrag ? 0 : 10;
        }
    }

    /**
     * Executes whenever the mouse pointer leaves the bounds of this Interactable.
     * 
     * @param ped is the pointer data provided by Unity.
     */
    public void OnPointerExit(PointerEventData ped)
    {
        isMouseOn = false;

        // If this Interactable is not being dragged, and no others are being interacted with...
        if (!isDragging && UIManager.elementInControl == -1)
        {
            CursorAtlas.instance.SetCursor("default", CursorMode.Auto);
            EventSystem.current.pixelDragThreshold = 10;
        }
    }

    /**
     * Executes at the beginning of a 'drag', where the primary mouse button is held while the pointer is on this Interactable.
     * 
     * @param ped is the pointer data provided by Unity.
     */
    public void OnBeginDrag(PointerEventData ped)
    {
        // If no other Interactable is being interacted with...
        if (UIManager.elementInControl == -1)
        {
            isDragging = true;
            UIManager.SetElementInControl(myUIElementID);
            CursorAtlas.instance.SetCursor(holdIcon, CursorMode.Auto);
        }
    }

    /**
     * Executes on every frame in which a 'drag' is sustained, where the primary mouse button is held while the pointer is on this Interactable.
     * 
     * @param ped is the pointer data provided by Unity.
     */
    public virtual void OnDrag(PointerEventData ped)
    {
        // If other mouse buttons are held, don't interact
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
            return;

        Vector2 resultantDelta = ped.delta;

        // Apply the alignment multis to the delta
        resultantDelta.x *= alignment.x;
        resultantDelta.y *= alignment.y;

        Drag.Invoke(resultantDelta, alignment);
    }

    /**
     * Executes when a 'drag' is released, after the primary mouse button is held while the pointer is on this Interactable.
     * 
     * @param ped is the pointer data provided by Unity.
     */
    public virtual void OnEndDrag(PointerEventData ped)
    {
        // If this Interactable was previously being interacted with...
        if (UIManager.elementInControl == myUIElementID)
        {
            isDragging = false;
            UIManager.SetElementInControl(-1);
            if (!isMouseOn)
                CursorAtlas.instance.SetCursor("default", CursorMode.Auto);
            else
                CursorAtlas.instance.SetCursor(hoverIcon, CursorMode.Auto);

            ReleaseDrag.Invoke();
        }
    }
}
