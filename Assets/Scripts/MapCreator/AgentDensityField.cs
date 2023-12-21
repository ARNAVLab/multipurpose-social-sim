using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// AgentDensityField displays and allows the user to set an agent 
/// preset's density with respect to a distribution
/// </summary>
public class AgentDensityField : MonoBehaviour
{
    
    [SerializeField] [Tooltip("Text component with name of agent preset")]
    private TMP_Text _nameLabel;
    [SerializeField] [Tooltip("Text component with value of agent ")]
    private TMP_InputField _valueField;
    [SerializeField] [Tooltip("Text component with value of consequent total of agent densities")]
    private TMP_Text _consequentLabel;

    /// <summary>
    /// The agent preset button for associated agent preset
    /// </summary>
    private AgentPresetButton _presetbutton;
    /// <summary>
    /// Vavlue of agent's density within distribution
    /// </summary>
    private float _value;

    /// <summary>
    /// Get or set the agent preset name shown
    /// </summary>
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

    /// <summary>
    /// Initializes values of agent density field
    /// </summary>
    /// <param name="name">Name of associated agent preset</param>
    /// <param name="value">Value of agent preset density within distribution</param>
    /// <param name="consequent">Consequent total of all agent densities in distribution</param>
    /// <param name="presetButton">Associated agent preset button</param>
    public void Initialize(string name, float value, float consequent, AgentPresetButton presetButton)
    {
        _value = value;
        _nameLabel.text = name;
        _valueField.text = value.ToString();
        SetRatioConsequent(consequent);
        _presetbutton = presetButton;
    }

    /// <summary>
    /// Sets the agent preset density displayed
    /// </summary>
    /// <param name="input">New density of agent preset within distribution</param>
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

    /// <summary>
    /// Sets the consequent total of agent densities in distribution
    /// that is displayed
    /// </summary>
    /// <param name="consequent">Consequent total of agent densities ratio</param>
    public void SetRatioConsequent(float consequent)
    {
        _consequentLabel.text = ":" + consequent.ToString();
    }
}
