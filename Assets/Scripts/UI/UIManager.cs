using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Tooltip("The Panel which displays Agent information when one or more is selected.")]
    [SerializeField] private AgentPanel agentPanel;
    [SerializeField] private GameObject agentPref;

    // DEBUG MEMBERS; THESE WILL SLOWLY BE REPLACED WITH OTHER FRONTEND ELEMENT INTEGRATION
    private KeyCode openAgentPanelDebug = KeyCode.A;
    private KeyCode selectAgentsDebug = KeyCode.B;
    private KeyCode nextAgentDebug = KeyCode.RightArrow;
    private KeyCode prevAgentDebug = KeyCode.LeftArrow;

    private void Update()
    {
        //if (Input.GetKeyDown(openAgentPanelDebug))
        //{
        //    ToggleAgentPanel();
        //}
        if (Input.GetKeyDown(selectAgentsDebug))
        {
            if (agentPanel.IsShown)
                agentPanel.Hide();
            else
                SelectDebugAgents();
        }
        if (agentPanel.IsShown)
        {
            if (Input.GetKeyDown(nextAgentDebug))
                agentPanel.ChangeFocusedAgent(1);
            if (Input.GetKeyDown(prevAgentDebug))
                agentPanel.ChangeFocusedAgent(-1);
        }
    }

    public void ToggleAgentPanel()
    {
        //agentPanel.Toggle();
    }

    private void SelectDebugAgents()
    {
        //// Randomly creates some Agents to debug with.
        //List<Agent> agents = new List<Agent>();
        //int numAgents = 10;
        //for (int i = 0; i < numAgents; i++)
        //{
        //    GameObject newAgentObj = Instantiate(agentPref);
        //    newAgentObj.transform.position = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        //    Agent newAgent = newAgentObj.GetComponent<Agent>();
        //    newAgent.Info.name = "Test Agent " + i;
        //    newAgent.Info.motive.physical = Random.Range(1, 6);
        //    newAgent.Info.motive.emotional = Random.Range(1, 6);
        //    newAgent.Info.motive.social = Random.Range(1, 6);
        //    newAgent.Info.motive.financial = Random.Range(1, 6);
        //    newAgent.Info.motive.accomplishment = Random.Range(1, 6);
        //    // Pick a random, saturated and not-too-dark color
        //    newAgent.displayColor = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f);
        //    agents.Add(newAgent);
        //}
        //agentPanel.Show(agents);
    }
}
