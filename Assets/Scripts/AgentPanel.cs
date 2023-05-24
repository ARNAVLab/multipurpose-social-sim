using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AgentPanel : Panel
{
    [Header(" --- Agent Info Panel ---")]
    private List<Agent> selectedAgents;
    private int focusedAgentIdx = 0;

    // TODO: These UI objects display the information relating to the currently focused agent.
    // However, since this is intended to be modifiable, in the future these should be instantiated on startup.


    public void ChangeFocusedAgent(int indexDelta)
    {
        // Obtain the actual number of indices moved, after full loop-arounds are taken into account.
        // I'm avoiding using a negative number in the modulo operation because implementations of that math depend on the programming language.
        int clampedDelta = Mathf.Abs(indexDelta) % selectedAgents.Count;
        focusedAgentIdx += clampedDelta;
        // Because we clamped indexDelta, resolving cases where the index is out of bounds is just addition/subtraction.
        if (focusedAgentIdx < 0)
            focusedAgentIdx += selectedAgents.Count;
        if (focusedAgentIdx >= selectedAgents.Count)
            focusedAgentIdx -= selectedAgents.Count;

        DisplayAgentInfo();
    }

    public void DisplayAgentInfo()
    {

    }

    /**
     * Extends the generic implementation of Panel.Show() to include setting a list of selected Agents.
     * @param selectedAgents is a list of Agent objects that represents all Agents currently viewable in the Agent Info Panel.
     */
    public void Show(List<Agent> selectedAgents)
    {
        base.Show();

        this.selectedAgents = selectedAgents;
        focusedAgentIdx = 0;
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
        focusedAgentIdx = 0;
    }
}
