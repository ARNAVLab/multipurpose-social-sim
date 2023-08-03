using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AgentPresetButton : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameLabel;
    
    public AgentPreset Preset;

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

    private MapCreator _mapCreator;

    private void Awake()
    {
        _mapCreator = FindObjectOfType<MapCreator>();
    }

    public void SelectAgent()
    {
        _mapCreator.SelectAgent(this);
    }
}
