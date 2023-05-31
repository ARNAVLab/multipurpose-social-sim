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
    [SerializeField] private GameObject content;
    [Tooltip("Local variable tracking whether this panel is currently Shown or Hidden.")]
    [SerializeField] protected bool isShown = false;
    public bool IsShown { get { return isShown; } }

    /**
     * On startup, Panels Show or Hide themselves based on the value of IsShown given to them in the editor.
     */
    private void Start()
    {
        InitPanel();

        if (IsShown)
            Show();
        else
            Hide();
    }

    private void InitPanel()
    {
        //RectTransform barRect = dragBar.GetComponent<RectTransform>();
        //barRect.sizeDelta = new Vector2(totalPnlSize.x, headerHeight);
        //barRect.anchoredPosition = Vector2.zero;

        DraggableUI draggableBar = dragBar.GetComponent<DraggableUI>();
        draggableBar.DraggableMoved.AddListener(MovePanel);

        //RectTransform contentRect = content.GetComponent<RectTransform>();
        //contentRect.sizeDelta = new Vector2(totalPnlSize.x, totalPnlSize.y - headerHeight);
        //contentRect.anchoredPosition = new Vector2(0, 0 - (barRect.sizeDelta.y / 2) - (contentRect.sizeDelta.y / 2));

        //topLeftAnchor = baseRect.position + new Vector3(-totalPnlSize.x / 2, headerHeight / 2, 0);
        //test.GetComponent<RectTransform>().position = topLeftAnchor;
    }

    private void MovePanel(Vector2 delta)
    {
        //topLeftAnchor += delta;
        //test.GetComponent<RectTransform>().position = topLeftAnchor;

        RectTransform baseRect = GetComponent<RectTransform>();
        baseRect.position += new Vector3(delta.x, delta.y, 0);
    }

    private void ResizePanel(Vector2 delta)
    {

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
