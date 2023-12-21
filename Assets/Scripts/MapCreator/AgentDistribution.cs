using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains information about agent distribution
/// </summary>
public class AgentDistribution
{
    /// <summary>
    /// Name of agent distribution
    /// </summary>
    public string Name;
    /// <summary>
    /// Color associated with distribution
    /// </summary>
    public Color Color;
    /// <summary>
    /// Overall density of agents in distribution
    /// </summary>
    public float AgentDensity;
    /// <summary>
    /// Wieghts of agents indexed by agent preset button
    /// </summary>
    public Dictionary<AgentPresetButton, float> AgentWeights;
}
