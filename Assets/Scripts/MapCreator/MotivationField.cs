using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MotivationField : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameLabel;
    [SerializeField] private TMP_InputField _valueField;
    private string _name;
    private float _value;

    public void Initialize(string name, float value)
    {
        _name = name;
        _value = value;
        _nameLabel.text = name;
        _valueField.text = value.ToString();
    }

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
