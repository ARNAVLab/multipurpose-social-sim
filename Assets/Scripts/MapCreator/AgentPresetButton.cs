using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Button containing information about an agent preset
/// </summary>
public class AgentPresetButton : MonoBehaviour
{
    [SerializeField] [Tooltip("Text component of agent preset button")]
    private TMP_Text _nameLabel;
    
    /// <summary>
    /// Agent preset associated with button
    /// </summary>
    public AgentPreset Preset;

    /// <summary>
    /// Sets or gets the name of agent preset
    /// </summary>
    public string Name {
        get
        {
            return Preset.Name;
        }
        set
        {
            Preset.Name = value;
            _nameLabel.text = value;
        }
    }

    /// <summary>
    /// Map creator reference
    /// </summary>
    private MapCreator _mapCreator;

    private void Awake()
    {
        _mapCreator = FindObjectOfType<MapCreator>();
    }

    /// <summary>
    /// Selects the associated agent in map creator
    /// </summary>
    public void SelectAgent()
    {
        _mapCreator.SelectAgent(this);
    }
}
