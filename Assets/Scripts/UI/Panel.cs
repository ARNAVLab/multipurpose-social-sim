using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel : MonoBehaviour
{
    [Header(" --- Generic Panel ---")]
    [Tooltip("A list of Panels that should Hide when this object Hides.\n" +
        "(These objects do not necessarily show when this object shows, hence the distinction from in-scene parenting!)")]
    [SerializeField] private List<Panel> connectedPanels;
    [SerializeField] private GameObject dragBar;
    [SerializeField] private GameObject[] sizeBars;
    [SerializeField] private GameObject content;
    [SerializeField] private Vector2 minSize;

    private Vector3 currPos;
    private Vector2 currSize;

    private RectTransform baseRect = null;

    [Tooltip("Local variable tracking whether this panel is currently Shown or Hidden.")]
    [SerializeField] protected bool isShown = false;
    public bool IsShown { get { return isShown; } }

    /**
     * On startup, Panels Show or Hide themselves based on the value of IsShown given to them in the editor.
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

    private void InitPanel()
    {
        InteractableUI draggableBar = dragBar.GetComponent<InteractableUI>();
        draggableBar.Drag.AddListener(MovePanel);

        for (int i = 0; i < sizeBars.Length; i++)
        {
            InteractableUI resizableBar = sizeBars[i].GetComponent<InteractableUI>();
            resizableBar.Drag.AddListener(ResizePanel);
            resizableBar.ReleaseDrag.AddListener(ResetTransform);
        }

        ResetTransform();
    }

    private void MovePanel(Vector2 delta, Vector2 alignment)
    {
        //topLeftAnchor += delta;
        //test.GetComponent<RectTransform>().position = topLeftAnchor;

        baseRect.position += new Vector3(delta.x, delta.y, 0);
    }

    private void ResizePanel(Vector2 delta, Vector2 alignment)
    {
        float canvaScale = transform.parent.GetComponent<Canvas>().scaleFactor;
        Debug.Log(canvaScale);

        currSize += delta / canvaScale;
        //currSize += scaledDelta;

        Vector2 prospSize = Vector2.zero;
        prospSize.x = Mathf.Clamp(currSize.x, minSize.x, Screen.width);
        prospSize.y = Mathf.Clamp(currSize.y, minSize.y, Screen.height);

        Vector2 prospDelta = prospSize - baseRect.sizeDelta;

        //prospectiveDelta.x = Mathf.Clamp(currSize.x + delta.x, minSize.x, Screen.width) - baseRect.sizeDelta.x;
        //prospectiveDelta.y = Mathf.Clamp(currSize.y + delta.y, minSize.y, Screen.height) - baseRect.sizeDelta.y;

        baseRect.sizeDelta += prospDelta;
        //baseRect.position += new Vector3(alignment.x * delta.x, alignment.y * delta.y, 0);
        baseRect.position += new Vector3(alignment.x * canvaScale * prospDelta.x / 2, alignment.y * canvaScale * prospDelta.y / 2, 0);
    }

    public void ResetTransform()
    {
        currPos = baseRect.position;
        currSize = baseRect.sizeDelta;
    }

    //private void GenerateDragBar()
    //{
    //    RectTransform panelRect = GetComponent<RectTransform>();

    //    dragBar = new GameObject();
    //    dragBar.transform.SetParent(transform.parent);
    //    transform.SetParent(dragBar.transform);
    //    RectTransform dragBarRect = dragBar.AddComponent<RectTransform>();
    //    dragBarRect.localScale = new Vector3(1, 1, 1);
    //    dragBarRect.sizeDelta = new Vector2(panelRect.sizeDelta.x, 10);
    //    dragBarRect.localPosition = new Vector3(0, panelRect.sizeDelta.x / 2, 0);
    //    dragBar.AddComponent<CanvasRenderer>();
    //    Image dragBarImg = dragBar.AddComponent<Image>();
    //    dragBarImg.sprite = windowBG;
    //    dragBarImg.color = new Color(1, 1, 1, 100f / 255f);
    //    dragBarImg.type = Image.Type.Sliced;
    //}

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
}
