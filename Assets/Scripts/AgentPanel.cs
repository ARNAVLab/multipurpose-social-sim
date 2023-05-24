using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentPanel : Panel
{
    [Header(" --- Agent Info Panel ---")]
    private List<Agent> selectedAgents;
    private int focusedAgentIdx;
   
    /**
     * Extends the generic implementation of Panel.Show() to include setting a list of selected Agents.
     * @param selectedAgents is a list of Agent objects that represents all Agents currently viewable in the Agent Info Panel.
     */
    public void Show(List<Agent> selectedAgents)
    {
        base.Show();

        this.selectedAgents = selectedAgents;
    }

    /**
     * Overrides the generic implementation of Panel.Show() to disallow its use.
     * An AgentPanel must be provided with a list of selected Agents when Shown.
     */
    public override void Show()
    {
        // If no list of Agents is provided, the AgentPanel does not become Shown.
        Debug.LogWarning("Attempted to Show Agent Panel without providing a list of Agents.");
    }

    /**
     * Extends the generic implementation of Panel.Hide() to include clearing the selected Agents list.
     */
    public override void Hide()
    {
        base.Hide();

        selectedAgents.Clear();
    }
}
