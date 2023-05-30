using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

/**
 * Resizes and recolors the Image component of the GameObject based on a float value.
 * As the value approaches the maximum value, it stretches and shifts in color towards maxColor.
 */
public class UIValueBar : MonoBehaviour
{
    private enum BarType
    {
        ALIGN_LEFT,
        ALIGN_RIGHT,
        ZERO_CENTER
    }

    [SerializeField] private BarType barType;

    [SerializeField] private float value;

    [Header("Value Min/Max")]
    [SerializeField] private float minVal;
    [SerializeField] private float maxVal;

    [Header("Bar Width Min/Max")]
    [SerializeField] private float minWidth;

    [SerializeField] private float maxWidth;

    [Header("Bar Color Min/Max")]
    [SerializeField] private Color minColor;
    [SerializeField] private Color maxColor;

    private void Start()
    {
        RectTransform myRect = GetComponent<RectTransform>();
        switch (barType)
        {
            case BarType.ALIGN_LEFT:
                {
                    myRect.pivot = new Vector2(0, 0.5f);
                    break;
                }
            case BarType.ALIGN_RIGHT:
                {
                    myRect.pivot = new Vector2(1, 0.5f);
                    GetComponent<RectTransform>().anchoredPosition += Vector2.right * GetComponent<RectTransform>().sizeDelta.x;
                    break;
                }
            case BarType.ZERO_CENTER:
                {
                    myRect.pivot = new Vector2(0.5f, 0.5f);
                    GetComponent<RectTransform>().anchoredPosition += Vector2.right * GetComponent<RectTransform>().sizeDelta.x / 2;
                    break;
                }
        }        
    }

    private void Update()
    {
        // Uncomment this line to manually change the bar's value in the editor
        SetValue(value);
    }

    public void SetValue(float newVal)
    {
        newVal = Mathf.Clamp(newVal, minVal, maxVal);
        value = newVal;
        UpdateAppearance();
    }

    private void UpdateAppearance()
    {
        RectTransform myRect = GetComponent<RectTransform>();

        float valPercent = (value - minVal) / (maxVal - minVal);
        
        if (barType == BarType.ZERO_CENTER)
        {
            myRect.sizeDelta = new Vector3(Mathf.Abs(valPercent - 0.5f) * (maxWidth - minWidth) + minWidth, myRect.sizeDelta.y);
            bool hangsLeft = valPercent - 0.5f < 0;
            myRect.anchoredPosition = new Vector3((hangsLeft ? -0.5f : 0.5f) * myRect.sizeDelta.x, myRect.anchoredPosition.y);
        }
        else
        {
            myRect.sizeDelta = new Vector2(valPercent * (maxWidth - minWidth) + minWidth, myRect.sizeDelta.y);
        }

        // Lerp between minColor and maxColor to get the new bar color
        Vector4 minColLerp = new Vector4(minColor.r, minColor.g, minColor.b, minColor.a);
        Vector4 maxColLerp = new Vector4(maxColor.r, maxColor.g, maxColor.b, maxColor.a);
        Vector4 targetColLerp = Vector4.Lerp(minColLerp, maxColLerp, valPercent);
        GetComponent<Image>().color = new Color(targetColLerp.x, targetColLerp.y, targetColLerp.z, targetColLerp.w);
    }
}
