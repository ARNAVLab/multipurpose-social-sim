using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UAgentManager
{
    private static Dictionary<int, UAgent> agents = new Dictionary<int, UAgent>();
    private static HashSet<int> selected = new HashSet<int>();
    private static int focused;

    /**
     * Attempts to set which Agent is "focused" (is selected/hovered on the frontend).
     * This fails if there is currently a focused Agent.
     * @param targetID is the ID of the Agent to focus.
     * @return whether the currently focused Agent successfully changed.
     */
    public static bool FocusAgent(int targetID)
    {
        // TODO: Should a check occur for attempting to focus the currently focused Agent (i.e. unfocus it)?
        if (focused > -1)
            return false; // Focus already occupied by another Agent

        focused = targetID;
        return true;
    }

    /**
     * Attempts to unfocus the currently "focused" (is selected/hovered on the frontend) Agent.
     * This fails if the currently focused Agent's ID does not match the passed ID.
     * @param targetID is the ID of the Agent to unfocus.
     * @return whether the currently focused Agent was successfully unfocused.
     */
    public static bool UnfocusAgent(int targetID)
    {
        if (focused != targetID)
            return false; // Focus not occupied by Agent with target ID

        focused = -1;
        return true;
    }

    /**
     * Attempts to add a new Agent to the static Dictionary, using its AgentID as the key.
     * This fails if the Dictionary already contains a value with the desired key.
     * @param registree is the Agent to register.
     * @return whether the addition was successful or not.
     */
    public static bool RegisterAgent(UAgent registree)
    {
        if (!agents.ContainsKey(registree.AgentID))
        {
            agents.Add(registree.AgentID, registree);
            return true;
        }
        return false;
    }
}
