using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // The UIManager Singleton instance.
    private static UIManager instance;

    [Tooltip("The Panel which displays Actor information when one or more is selected.")]
    [SerializeField] private ActorInfoDisplay actorInfo;
    [Tooltip("The Panel which displays Location information when one or more is selected.")]
    [SerializeField] private PlaceInfoDisplay placeInfo;

    [Tooltip("The button which changes the selection mode to 'Actors Only'.")]
    [SerializeField] private Button actorModeBtn;
    [Tooltip("The button which changes the selection mode to 'Locations Only'.")]
    [SerializeField] private Button placeModeBtn;

    // Sprites to telegraph which selection mode is currently selected.
    [SerializeField] private Sprite actorModeOn;
    [SerializeField] private Sprite actorModeOff;
    [SerializeField] private Sprite placeModeOn;
    [SerializeField] private Sprite placeModeOff;

    public enum SelectType
    {
        ACTORS,
        PLACES
    }
    private static SelectType selectMode = SelectType.ACTORS;

    // These track which UI element is currently being interacted with using a unique identifier.
    // This is such that several UI elements are not interacted with at once when they are stacked on one another.
    private static int nextUIElementID = 0;
    public static int elementInControl = -1;

    private List<Selectable> selected = new List<Selectable>();

    // The index of the selectable in the 'selected' list which is currently in focus.
    private int focusedIdx = -1;

    private KeyCode nextSelection = KeyCode.RightArrow;
    private KeyCode prevSelection = KeyCode.LeftArrow;

    public static UIManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        instance = this;
        SelectionController._onSelectEvent.AddListener(SelectionListener);
        WorldManager.simUpdated.AddListener(SimUpdateListener);
    }

    private void Update()
    {
        if (Input.GetKeyDown(nextSelection))
            ChangeFocusedAgent(1);
        if (Input.GetKeyDown(prevSelection))
            ChangeFocusedAgent(-1);
    }

    public static void SetElementInControl(int newUIElementID)
    {
        elementInControl = newUIElementID;
    }

    public static int GetNextUIElementID()
    {
        nextUIElementID++;
        return nextUIElementID - 1;
    }

    public void DisplayFocusedInfo()
    {
        // Debug.Log("Count is " + selected.Count);
        // Debug.Log("Focus is " + focusedIdx);
        if (selectMode == SelectType.ACTORS)
            actorInfo.DisplayInfo(selected[focusedIdx]);
        else
            placeInfo.DisplayInfo(selected[focusedIdx]);
    }

    /**
     * Selects the agent at a position relative to the currently focused agent in the selected agents list, then displays their info.
     * Because of the call to GetAgentIndex, positions outside of the list bounds are supported.
     * @param indexDelta is the number of positions away from focusedAgentIdx the target Agent is.
     */
    public void ChangeFocusedAgent(int indexDelta)
    {
        if (focusedIdx < 0)
            return;

        selected[focusedIdx].Unfocus();
        focusedIdx = GetAgentIndex(focusedIdx, indexDelta);
        selected[focusedIdx].Focus();
        SelectionController.EnableTracking(selected[focusedIdx].transform);
        DisplayFocusedInfo();
    }

    /**
     * Increments a starting index by a certain delta, looping around to ensure that the result is always
     * a valid index of the selected agents list.
     * @param indexStart is the starting index to increment by indexDelta.
     * @param indexDelta is the number of positions to move from indexStart.
     * @return startIndex + indexDelta, constrained to the bounds of the selected agents list.
     */
    public int GetAgentIndex(int indexStart, int indexDelta)
    {
        // Obtain the actual number of indices moved, after full loop-arounds are taken into account.
        // I'm avoiding using a negative number in the modulo operation because implementations of that math depend on the programming language.
        int clampedDelta = Mathf.Abs(indexDelta) % selected.Count;
        if (indexDelta < 0)
            clampedDelta = -clampedDelta;
        int result = indexStart + clampedDelta;
        // Because we clamped indexDelta, resolving cases where the index is out of bounds is just addition/subtraction.
        if (result < 0)
            result += selected.Count;
        if (result >= selected.Count)
            result -= selected.Count;
        return result;
    }

    /**
     * Deselects and unfocuses all Selectables in the list, then clears the list.
     */
    private void ClearSelected()
    {
        foreach (Selectable s in selected)
        {
            s.Deselect();
            s.Unfocus();
        }
        selected.Clear();
        focusedIdx = -1;
        // Debug.Log("cleared");
    }

    /**
     * Invoked whenever the mouse button is released while dragging the selection box.
     * If nothing was selected, clear all selecteds and hide info panels.
     * If something was selected, show the appropriate info panel.
     */
    private void SelectionListener()
    {
        HashSet<Selectable> selectedSet = SelectionManager.Instance.Selected;

        if (selectedSet.Count <= 0)
        {
            // Nothing was selected. Attempt to hide panels.
            actorInfo.GetComponent<Panel>().Hide();
            placeInfo.GetComponent<Panel>().Hide();

            ClearSelected();

            focusedIdx = -1;
            SelectionController.DisableTracking();
        }
        else
        {
            // At least one Agent was selected; attempt to show respective panel.
            if (selectMode == SelectType.ACTORS)
                actorInfo.GetComponent<Panel>().Show();
            else
                placeInfo.GetComponent<Panel>().Show();

            ClearSelected();

            foreach (Selectable s in selectedSet)
            {
                selected.Add(s);
                s.Select();
            }

            focusedIdx = 0;
            selected[focusedIdx].Focus();
            SelectionController.EnableTracking(selected[focusedIdx].transform);
            DisplayFocusedInfo();
        }
    }

    private void SimUpdateListener()
    {
        if (focusedIdx > -1)
        {
            DisplayFocusedInfo();
        }
    }

    public SelectType GetSelectMode()
    {
        return selectMode;
    }

    public void SetSelectMode(int type)
    {
        selectMode = (SelectType) type;

        ClearSelected();

        actorInfo.gameObject.SetActive(false);
        placeInfo.gameObject.SetActive(false);

        actorModeBtn.image.sprite = selectMode == SelectType.ACTORS ? actorModeOn : actorModeOff;
        placeModeBtn.image.sprite = selectMode == SelectType.PLACES ? placeModeOn : placeModeOff;

        if (selectMode == SelectType.ACTORS) 
            actorInfo.GetComponent<RectTransform>().anchoredPosition = new Vector3(135, -85, 0);
        else
            placeInfo.GetComponent<RectTransform>().anchoredPosition = new Vector3(135, -85, 0);

        SelectionController.GetInstance().SetSelectBoxLayer(selectMode == SelectType.ACTORS ? "Agent" : "Location");
    }
}
