using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AgentPanel : Panel
{
    [Header(" --- Agent Info Panel ---")]
    // A list of Agents currently selected by the user in the scene.
    private List<Actor> selectedAgents = new List<Actor>();
    // The index of the Agent in the selected agents list whose information is currently being displayed.
    private int focusedAgentIdx = 0;

    // TODO: These UI objects display the information relating to the currently focused agent.
    // However, since this is intended to be modifiable, in the future these should be instantiated on startup.
    [SerializeField] private GameObject[] agentSprites;
    [SerializeField] private GameObject nameBox;
    [SerializeField] private GameObject[] motiveNums;
    [SerializeField] private GameObject[] motiveBars;
    [SerializeField] private GameObject[] opinionNums;
    [SerializeField] private GameObject[] opinionBars;

    private void Start()
    {
        SelectionController._onSelectEvent.AddListener(SelectionListener);
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
        DisplayAgentInfo();
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

    /**
     * Updates the UI elements to reflect the information stored in the currently focused Agent.
     */
    public void DisplayAgentInfo()
    {
        Actor focusedAgent = selectedAgents[focusedAgentIdx];

        agentSprites[0].GetComponent<Image>().color = selectedAgents[GetAgentIndex(focusedAgentIdx, -1)].displayColor;
        agentSprites[1].GetComponent<Image>().color = focusedAgent.displayColor;
        agentSprites[2].GetComponent<Image>().color = selectedAgents[GetAgentIndex(focusedAgentIdx, 1)].displayColor;

        nameBox.GetComponent<TextMeshProUGUI>().text = focusedAgent.Info.name;

        motiveNums[0].GetComponent<TextMeshProUGUI>().text = focusedAgent.Info.motive.physical.ToString();
        motiveNums[1].GetComponent<TextMeshProUGUI>().text = focusedAgent.Info.motive.emotional.ToString();
        motiveNums[2].GetComponent<TextMeshProUGUI>().text = focusedAgent.Info.motive.social.ToString();
        motiveNums[3].GetComponent<TextMeshProUGUI>().text = focusedAgent.Info.motive.financial.ToString();
        motiveNums[4].GetComponent<TextMeshProUGUI>().text = focusedAgent.Info.motive.accomplishment.ToString();

        motiveBars[0].GetComponent<UIValueBar>().SetValue(focusedAgent.Info.motive.physical);
        motiveBars[1].GetComponent<UIValueBar>().SetValue(focusedAgent.Info.motive.emotional);
        motiveBars[2].GetComponent<UIValueBar>().SetValue(focusedAgent.Info.motive.social);
        motiveBars[3].GetComponent<UIValueBar>().SetValue(focusedAgent.Info.motive.financial);
        motiveBars[4].GetComponent<UIValueBar>().SetValue(focusedAgent.Info.motive.accomplishment);
    }

    private void SelectionListener()
    {
        HashSet<Selectable> selected = SelectionManager.Instance.Selected;

        Debug.Log(selected.Count);

        if (selected.Count <= 0)
        {
            // Nothing was selected. Attempt to hide panel.
            Hide();
        }
        else
        {
            // At least one Agent was selected; attempt to show panel.
            Show();

            agentSprites[0].SetActive(selected.Count > 1);
            agentSprites[2].SetActive(selected.Count > 1);

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

    /**
     * Extends the generic implementation of Panel.Hide() to include clearing the selected Agents list.
     */
    public override void Hide()
    {
        base.Hide();

        //// TEMPORARY ! Until selection is implemented, the agent panel will spawn and delete ten randomized agents
        //while (selectedAgents.Count > 0)
        //{
        //    GameObject toDestroy = selectedAgents[0].gameObject;
        //    selectedAgents.RemoveAt(0);
        //    Destroy(toDestroy);
        //}
        //// TEMPORARY ! Until selection is implemented, the agent panel will spawn and delete ten randomized agents

        selectedAgents.Clear();
        focusedAgentIdx = 0;
    }
}
