using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Script that manages a MotivationField prefab
/// </summary>
public class MotivationField : MonoBehaviour
{
    [SerializeField] [Tooltip("Text component for name of motivation")] 
    private TMP_Text _nameLabel;
    [SerializeField] [Tooltip("Text field for motivation value")]
    private TMP_InputField _valueField;

    /// <summary>
    /// Name of the motivation
    /// </summary>
    private string _name;
    /// <summary>
    /// Value of the motivation for the agent preset
    /// </summary>
    private float _value;

    /// <summary>
    /// Sets the values displayed by the motivation field
    /// </summary>
    /// <param name="name">Name of motivation</param>
    /// <param name="value">Value of motivation</param>
    public void Initialize(string name, float value)
    {
        _name = name;
        _value = value;
        _nameLabel.text = name;
        _valueField.text = value.ToString();
    }

    /// <summary>
    /// Changes the value of the motivation using the provided string
    /// </summary>
    /// <param name="valueString">String containing motivation value to set</param>
    public void ChangeValue(string valueString)
    {
        float value;
        if (!float.TryParse(valueString, out value))
        {
            _valueField.text = _value.ToString();
            return;
        }
        _value = value;
        FindObjectOfType<MapCreator>().SetMotivationValue(_name, value);
    }
}
