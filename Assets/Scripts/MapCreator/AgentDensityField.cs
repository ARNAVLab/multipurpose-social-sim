using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AgentDensityField : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameLabel;
    [SerializeField] private TMP_InputField _valueField;
    [SerializeField] private TMP_Text _consequentLabel;
    private AgentPresetButton _presetbutton;
    private float _value;

    public string Name {
        get
        {
            return _nameLabel.text;
        }
        set
        {
            _nameLabel.text = value;
        }
    }

    public void Initialize(string name, float value, float consequent, AgentPresetButton presetButton)
    {
        _value = value;
        _nameLabel.text = name;
        _valueField.text = value.ToString();
        SetRatioConsequent(consequent);
        _presetbutton = presetButton;
    }

    public void SetDensity(string input)
    {
        float value;
        if (!float.TryParse(input, out value))
        {
            _valueField.text = _value.ToString();
            return;
        }
        _value = value;
        FindObjectOfType<MapCreator>().SetDensityValue(_presetbutton, value);
    }

    public void SetRatioConsequent(float consequent)
    {
        _consequentLabel.text = ":" + consequent.ToString();
    }
}
