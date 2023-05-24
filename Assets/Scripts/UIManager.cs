using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Tooltip("The Panel which displays Agent information when one or more is selected.")]
    [SerializeField] private Panel agentPanel;

    // DEBUG MEMBERS; THESE WILL SLOWLY BE REPLACED WITH OTHER FRONTEND ELEMENT INTEGRATION
    private KeyCode openAgentPanelDebug = KeyCode.A;
    private KeyCode nextAgentDebug = KeyCode.RightArrow;
    private KeyCode prevAgentDebug = KeyCode.LeftArrow;

    private void Update()
    {
        if (Input.GetKeyDown(openAgentPanelDebug))
        {
            ToggleAgentPanel();
        }
    }

    public void ToggleAgentPanel()
    {
        agentPanel.Toggle();
    }
}
