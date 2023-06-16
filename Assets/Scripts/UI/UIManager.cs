using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Tooltip("The Panel which displays Agent information when one or more is selected.")]
    [SerializeField] private Panel actorInfoPanel;

    private static int nextUIElementID = 0;
    public static int elementInControl = -1;

    private List<Actor> selectedAgents = new List<Actor>();
    // The index of the actor in the selectedAgents list who is currently in focus.
    private int focusedAgentIdx = 0;

    private KeyCode nextActor = KeyCode.RightArrow;
    private KeyCode prevActor = KeyCode.LeftArrow;

    private void Start()
    {
        SelectionController._onSelectEvent.AddListener(SelectionListener);
    }

    private void Update()
    {
        if (Input.GetKeyDown(nextActor))
        {
            ChangeFocusedAgent(1);
        }
        if (Input.GetKeyDown(prevActor))
        {
            ChangeFocusedAgent(-1);
        }
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
        Actor target = selectedAgents[focusedAgentIdx];
        actorInfoPanel.GetComponent<ActorInfoDisplay>().DisplayAgentInfo(target.name);
    }

    /**
     * Selects the agent at a position relative to the currently focused agent in the selected agents list, then displays their info.
     * Because of the call to GetAgentIndex, positions outside of the list bounds are supported.
     * @param indexDelta is the number of positions away from focusedAgentIdx the target Agent is.
     */
    public void ChangeFocusedAgent(int indexDelta)
    {
        selectedAgents[focusedAgentIdx].Unfocus();
        focusedAgentIdx = GetAgentIndex(focusedAgentIdx, indexDelta);
        selectedAgents[focusedAgentIdx].Focus();
        Camera.main.transform.position = new Vector3(selectedAgents[focusedAgentIdx].transform.position.x, selectedAgents[focusedAgentIdx].transform.position.y, Camera.main.transform.position.z);
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
        int clampedDelta = Mathf.Abs(indexDelta) % selectedAgents.Count;
        if (indexDelta < 0)
            clampedDelta = -clampedDelta;
        int result = indexStart + clampedDelta;
        // Because we clamped indexDelta, resolving cases where the index is out of bounds is just addition/subtraction.
        if (result < 0)
            result += selectedAgents.Count;
        if (result >= selectedAgents.Count)
            result -= selectedAgents.Count;
        return result;
    }

    private void SelectionListener()
    {
        HashSet<Selectable> selected = SelectionManager.Instance.Selected;

        Debug.Log(selected.Count);

        if (selected.Count <= 0)
        {
            // Nothing was selected. Attempt to hide panel.
            actorInfoPanel.Hide();
        }
        else
        {
            // At least one Agent was selected; attempt to show panel.
            actorInfoPanel.Show();

            selectedAgents.Clear();

            foreach (Selectable s in selected)
            {
                Actor a = (Actor)s;
                selectedAgents.Add(a);
            }

            focusedAgentIdx = 0;
            ChangeFocusedAgent(0);
        }
    }
}
