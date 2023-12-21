using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Script that allows selection of distribution color using RGB values
/// </summary>
public class ColorPicker : MonoBehaviour
{
    [SerializeField] [Tooltip("Image component displaying selected color")]
    private Image _colorImage;
    [SerializeField] [Tooltip("Text field for red component of color")]
    private TMP_InputField _redField;
    [SerializeField] [Tooltip("Text field for green component of color")]
    private TMP_InputField _greenField;
    [SerializeField] [Tooltip("Text field for blue component of color")]
    private TMP_InputField _blueField;

    /// <summary>
    /// Current color of color picker
    /// </summary>
    private Color32 _color;
    public Color32 Color { get { return _color; } set { SetColor(value); } }

    /// <summary>
    /// Sets the color of the color picker
    /// </summary>
    /// <param name="color">Color to set</param>
    public void SetColor(Color32 color)
    {
        _color = color;
        _color.a = 255;
        _colorImage.color = color;
        _redField.text = color.r.ToString();
        _greenField.text = color.g.ToString();
        _blueField.text = color.b.ToString();
    }

    /// <summary>
    /// Sets the distribution color based on color picker
    /// </summary>
    /// <param name="color"></param>
    public void SetDistColor(Color32 color)
    {
        SetColor(color);
        FindObjectOfType<MapCreator>().SetDistColor(color);
    }

    /// <summary>
    /// Sets the red component of the color
    /// </summary>
    /// <param name="input">Red component</param>
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

    /// <summary>
    /// Sets the green component of the color
    /// </summary>
    /// <param name="input">Green component</param>
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

    /// <summary>
    /// Sets the blue component of the color
    /// </summary>
    /// <param name="input">Blue component</param>
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
