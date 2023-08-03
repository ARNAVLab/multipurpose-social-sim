using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentDistribution
{
    public string Name;
    public Color Color;
    public float AgentDensity;
    public Dictionary<AgentPresetButton, float> AgentWeights;
}
