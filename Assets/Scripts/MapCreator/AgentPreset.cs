using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains information about agent preset
/// </summary>
public struct AgentPreset
{
    /// <summary>
    /// Name of agent preset
    /// </summary>
    public string Name;
    /// <summary>
    /// Motivation values of agent indexed by motivation name
    /// </summary>
    public Dictionary<string,float> Motivations;
}
