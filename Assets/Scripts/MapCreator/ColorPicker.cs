using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorPicker : MonoBehaviour
{
    [SerializeField] private Image _colorImage;
    [SerializeField] private TMP_InputField _redField;
    [SerializeField] private TMP_InputField _greenField;
    [SerializeField] private TMP_InputField _blueField;

    private Color32 _color;
    public Color32 Color { get { return _color; } set { SetColor(value); } }

    public void SetColor(Color32 color)
    {
        _color = color;
        _color.a = 255;
        _colorImage.color = color;
        _redField.text = color.r.ToString();
        _greenField.text = color.g.ToString();
        _blueField.text = color.b.ToString();
    }

    public void SetDistColor(Color32 color)
    {
        SetColor(color);
        FindObjectOfType<MapCreator>().SetDistColor(color);
    }

    public void SetRed(string input)
    {
        int value;
        if (input == "")
        {
            _color.r = 0;
        }
        else if (int.TryParse(input, out value) && value >= 0 && value <= 255)
        {
            _color.r = (byte)value;
        }
        SetDistColor(_color);
    }

    public void SetGreen(string input)
    {
        int value;
        if (input == "")
        {
            _color.g = 0;
        }
        else if (int.TryParse(input, out value) && value >= 0 && value <= 255)
        {
            _color.g = (byte)value;
        }
        SetDistColor(_color);
    }

    public void SetBlue(string input)
    {
        int value;
        if (input == "")
        {
            _color.b = 0;
        }
        else if (int.TryParse(input, out value) && value >= 0 && value <= 255)
        {
            _color.b = (byte)value;
        }
        SetDistColor(_color);
    }
}
