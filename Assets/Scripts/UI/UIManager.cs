using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Tooltip("The Panel which displays Agent information when one or more is selected.")]
    [SerializeField] private Panel actorInfoPanel;

    private static int nextUIElementID = 0;
    public static int elementInControl = -1;

    private List<Actor> selectedActors = new List<Actor>();
    // The index of the actor in the selectedAgents list who is currently in focus.
    private int focusedAgentIdx = -1;

    private KeyCode nextActor = KeyCode.RightArrow;
    private KeyCode prevActor = KeyCode.LeftArrow;

    private void Start()
    {
        SelectionController._onSelectEvent.AddListener(SelectionListener);
        WorldManager.actorsUpdated.AddListener(ActorUpdateListener);
    }

    private void Update()
    {
        if (Input.GetKeyDown(nextActor))
            ChangeFocusedAgent(1);
        if (Input.GetKeyDown(prevActor))
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
        Actor target = selectedActors[focusedAgentIdx];
        actorInfoPanel.GetComponent<ActorInfoDisplay>().DisplayAgentInfo(target.name);
    }

    /**
     * Selects the agent at a position relative to the currently focused agent in the selected agents list, then displays their info.
     * Because of the call to GetAgentIndex, positions outside of the list bounds are supported.
     * @param indexDelta is the number of positions away from focusedAgentIdx the target Agent is.
     */
    public void ChangeFocusedAgent(int indexDelta)
    {
        selectedActors[focusedAgentIdx].Unfocus();
        focusedAgentIdx = GetAgentIndex(focusedAgentIdx, indexDelta);
        selectedActors[focusedAgentIdx].Focus();
        SelectionController.EnableTracking(selectedActors[focusedAgentIdx].transform);
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
        int clampedDelta = Mathf.Abs(indexDelta) % selectedActors.Count;
        if (indexDelta < 0)
            clampedDelta = -clampedDelta;
        int result = indexStart + clampedDelta;
        // Because we clamped indexDelta, resolving cases where the index is out of bounds is just addition/subtraction.
        if (result < 0)
            result += selectedActors.Count;
        if (result >= selectedActors.Count)
            result -= selectedActors.Count;
        return result;
    }

    private void ClearSelected()
    {
        foreach (Actor a in selectedActors)
        {
            a.Unfocus();
        }
        selectedActors.Clear();
    }

    private void SelectionListener()
    {
        HashSet<Selectable> selected = SelectionManager.Instance.Selected;

        if (selected.Count <= 0)
        {
            // Nothing was selected. Attempt to hide panel.
            actorInfoPanel.Hide();

            ClearSelected();

            focusedAgentIdx = -1;
            SelectionController.DisableTracking();
        }
        else
        {
            // At least one Agent was selected; attempt to show panel.
            actorInfoPanel.Show();

            ClearSelected();

            foreach (Selectable s in selected)
            {
                Actor a = (Actor)s;
                selectedActors.Add(a);
            }

            focusedAgentIdx = 0;
            selectedActors[focusedAgentIdx].Focus();
            SelectionController.EnableTracking(selectedActors[focusedAgentIdx].transform);
            DisplayFocusedActorInfo();
        }
    }

    private void ActorUpdateListener()
    {
        if (focusedAgentIdx > -1)
        {
            DisplayFocusedActorInfo();
        }
    }
}
