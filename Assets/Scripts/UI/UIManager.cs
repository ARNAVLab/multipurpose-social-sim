using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Tooltip("The Panel which displays Agent information when one or more is selected.")]
    [SerializeField] private ActorInfoDisplay actorInfo;
    [SerializeField] private PlaceInfoDisplay placeInfo;

    private enum SelectType
    {
        ACTORS,
        PLACES
    }
    private SelectType selectMode = SelectType.ACTORS;

    private static int nextUIElementID = 0;
    public static int elementInControl = -1;

    private List<Selectable> selected = new List<Selectable>();

    // The index of the selectable in the 'selected' list which is currently in focus.
    private int focusedIdx = -1;

    private KeyCode nextSelection = KeyCode.RightArrow;
    private KeyCode prevSelection = KeyCode.LeftArrow;

    private void Start()
    {
        SelectionController._onSelectEvent.AddListener(SelectionListener);
        WorldManager.actorsUpdated.AddListener(ActorUpdateListener);
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

    public void DisplayFocusedActorInfo()
    {
        switch (selectMode)
        {
            case SelectType.ACTORS:
                {
                    actorInfo.DisplayAgentInfo(selected[focusedIdx]);
                    break;
                }
            case SelectType.PLACES:
                {
                    placeInfo.DisplayLocationInfo(selected[focusedIdx]);
                    break;
                }
        }
    }

    /**
     * Selects the agent at a position relative to the currently focused agent in the selected agents list, then displays their info.
     * Because of the call to GetAgentIndex, positions outside of the list bounds are supported.
     * @param indexDelta is the number of positions away from focusedAgentIdx the target Agent is.
     */
    public void ChangeFocusedAgent(int indexDelta)
    {
        selected[focusedIdx].Unfocus();
        focusedIdx = GetAgentIndex(focusedIdx, indexDelta);
        selected[focusedIdx].Focus();
        SelectionController.EnableTracking(selected[focusedIdx].transform);
        DisplayFocusedActorInfo();
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

    private void ClearSelected()
    {
        foreach (Selectable s in selected)
        {
            s.Unfocus();
        }
        selected.Clear();
    }

    private void SelectionListener()
    {
        HashSet<Selectable> selectedSet = SelectionManager.Instance.Selected;

        if (selectedSet.Count <= 0)
        {
            // Nothing was selected. Attempt to hide panel.
            actorInfo.GetComponent<Panel>().Hide();

            ClearSelected();

            focusedIdx = -1;
            SelectionController.DisableTracking();
        }
        else
        {
            // At least one Agent was selected; attempt to show panel.
            actorInfo.GetComponent<Panel>().Show();

            ClearSelected();

            foreach (Selectable s in selectedSet)
            {
                selected.Add(s);
            }

            focusedIdx = 0;
            selected[focusedIdx].Focus();
            SelectionController.EnableTracking(selected[focusedIdx].transform);
            DisplayFocusedActorInfo();
        }
    }

    private void ActorUpdateListener()
    {
        if (focusedIdx > -1)
        {
            DisplayFocusedActorInfo();
        }
    }
}
