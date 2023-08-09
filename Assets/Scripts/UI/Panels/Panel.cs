using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Panel : MonoBehaviour
{
    [Header(" --- Generic Panel ---")]
    [Tooltip("A list of Panels that should Hide when this object Hides.\n" +
        "(These objects do not necessarily show when this object shows, hence the distinction from in-scene parenting!)")]
    [SerializeField] private List<Panel> connectedPanels;
    [Tooltip("The TextMeshProUGUI corresponding to this Panel's window header.")]
    [SerializeField] private TextMeshProUGUI title;
    [Tooltip("InteractableUI objects that, when dragged, move the panel around the screen.")]
    [SerializeField] private GameObject[] dragZones;
    [Tooltip("InteractableUI objects that, when dragged, resize the panel.")]
    [SerializeField] private GameObject[] sizeBars;
    [Tooltip("The container object for all the Panel's primary content.")]
    [SerializeField] public GameObject content;
    [Tooltip("The minimum size allowed for this Panel.")]
    [SerializeField] private Vector2 minSize;

    // The current size of the Panel (assuming it were expanded; this value does not change when a Panel is collapsed)
    private Vector2 currSize;

    // A reference to the RectTransform component of the Panel, for ease of access.
    private RectTransform baseRect = null;

    [Tooltip("Whether this panel is currently visible on-screen.")]
    [SerializeField] protected bool isShown = false;
    public bool IsShown { get { return isShown; } }

    /**
     * On startup, Panels Show/Hide and Expand/Collapse themselves based on the values given to them in the editor.
     */
    private void Start()
    {
        baseRect = GetComponent<RectTransform>();

        InitPanel();

        if (IsShown)
            Show();
        else
            Hide();
    }

    /**
     * Initializes listeners for InteractableUI components of this Panel, then calls ResetTransform().
     */
    private void InitPanel()
    {
        for (int i = 0; i < dragZones.Length; i++)
        {
            InteractableUI draggableBar = dragZones[i].GetComponent<InteractableUI>();
            draggableBar.Drag.AddListener(MovePanel);
            draggableBar.ReleaseDrag.AddListener(ResetTransform);
        }

        for (int i = 0; i < sizeBars.Length; i++)
        {
            InteractableUI resizableBar = sizeBars[i].GetComponent<InteractableUI>();
            resizableBar.Drag.AddListener(ResizeAction);
            resizableBar.ReleaseDrag.AddListener(ResetTransform);
        }

        ResetTransform();
    }

    /**
     * Changes the Panel's position by the given delta. The alignment parameter is unused.
     * 
     * @param delta is the distance to move the Panel
     * @param alignment is a byproduct of a UnityEvent with two parameters
     */
    private void MovePanel(Vector2 delta, Vector2 alignment)
    {
        baseRect.position += new Vector3(delta.x, delta.y, 0);
    }

    /**
     * Receives a delta and an alignment from an InteractableUI event, applies the canvas scale factor, then delegates to ResizePanel with updateSize true.
     * 
     * @param pointerDelta is the distance the resize anchor point was moved
     * @param alignment is the alignment of the resize action
     */
    private void ResizeAction(Vector2 pointerDelta, Vector2 alignment)
    {
        float canvaScale = transform.parent.GetComponent<Canvas>().scaleFactor;

        Vector2 targetSize = currSize + (pointerDelta / canvaScale);

        ResizePanel(targetSize, alignment, true);
    }

    /**
     * Changes the Panel's size to the given size, then repositions the Panel to accomodate the change.
     * 
     * @param targetSize is the intended size of the Panel
     * @param alignment is a multiplier for the adjustments to the Panel's position
     * @param doUpdateSize is whether currSize should be updated to reflect resulting size
     */
    private void ResizePanel(Vector2 targetSize, Vector2 alignment, bool doUpdateSize)
    {
        float canvaScale = transform.parent.GetComponent<Canvas>().scaleFactor;

        Vector2 targetDelta = targetSize - baseRect.sizeDelta;

        baseRect.sizeDelta += targetDelta;
        //baseRect.position += new Vector3(alignment.x * delta.x, alignment.y * delta.y, 0);
        baseRect.position += new Vector3(alignment.x * canvaScale * targetDelta.x / 2, alignment.y * canvaScale * targetDelta.y / 2, 0);

        if (doUpdateSize)
            //currSize = targetSize;
            ResetTransform();
    }

    /**
     * Clamps the Panel's position and size based on the current screen size, then updates the internal value for size.
     */
    public void ResetTransform()
    {
        // --- Clamp Position ---

        Vector2 posDelta = Vector2.zero;

        float canvaScale = transform.parent.GetComponent<Canvas>().scaleFactor;
        Vector2 scaledSize = baseRect.sizeDelta * canvaScale;

        Vector2 topRightAnchor = baseRect.position + new Vector3(scaledSize.x / 2, scaledSize.y / 2, 0);
        Vector2 botLeftAnchor = baseRect.position + new Vector3(-scaledSize.x / 2, -scaledSize.y / 2, 0);

        // Detect if the panel is off-screen, and if it is, snap it back in bounds
        if (botLeftAnchor.x < 0)
            // Too far left, snap right
            posDelta.x += -botLeftAnchor.x;
        else if (topRightAnchor.x > Screen.width)
            // Too far right, snap left
            posDelta.x -= topRightAnchor.x - Screen.width;

        if (botLeftAnchor.y < 0)
            // Too far down, snap up
            posDelta.y += -botLeftAnchor.y;
        else if (topRightAnchor.y > Screen.height)
            // Too far up, snap down
            posDelta.y -= topRightAnchor.y - Screen.height;

        MovePanel(posDelta, Vector2.one);

        // --- Clamp Size ---

        Vector2 sizeDelta = new Vector2();

        if (baseRect.sizeDelta.x < minSize.x)
            sizeDelta.x += minSize.x - baseRect.sizeDelta.x;
        else if (baseRect.sizeDelta.x > Screen.width / canvaScale)

        if (baseRect.sizeDelta.y < minSize.y)
            sizeDelta.y += minSize.y - baseRect.sizeDelta.y;

        baseRect.sizeDelta = new Vector2(Mathf.Clamp(baseRect.sizeDelta.x, minSize.x, Screen.width / canvaScale), Mathf.Clamp(baseRect.sizeDelta.y, minSize.y, Screen.height / canvaScale));

        currSize = baseRect.sizeDelta;
    }

    /**
     * Looks at the given index of the Connected Panels list, and calls Toggle for that Panel.
     * Logs an error and does nothing if the index is invalid.
     * @param idx is the target Panel's index in the Connected Panels list.
     */
    public void ToggleConnected(int idx)
    {
        if (idx < 0 || idx >= connectedPanels.Count)
        {
            Debug.LogError("Panel tried to toggle Connected Panel #" + idx + ", but no such Panel exists!");
            return;
        }

        if (connectedPanels[idx].IsShown)
            connectedPanels[idx].Hide();
        else
            connectedPanels[idx].Show();
    }

    /**
     * Makes this Panel's GameObject visible and records that it is Shown.
     */
    public virtual void Show()
    {
        isShown = true;
        gameObject.SetActive(true);
    }

    /**
     * Makes this Panel's GameObject invisible and records that it is Hidden.
     * Additionally, broadcasts the call to Hide to all Connected Panels.
     */
    public virtual void Hide()
    {
        isShown = false;
        gameObject.SetActive(false);
        foreach (Panel p in connectedPanels)
        {
            p.Hide();
        }
    }

    /**
     * Changes the text contained in this Panel's title.
     */
    public void SetTitle(string newTitle)
    {
        title.text = newTitle;
    }
}
