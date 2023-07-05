using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;
using System.Globalization;

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

    [Tooltip("The GameObject whose Image component will be changed to reflect the tracked value.")]
    [SerializeField] private GameObject barImg;
    [Tooltip("What alignment the bar should be given within the bounds of this GameObject.")]
    [SerializeField] private BarType barType;

    // The internal value used to scale the bar.
    private float value = -1;


    [Tooltip("The minimum value able to be assigned to the bar.")]
    [SerializeField] private float minVal;
    [Tooltip("The maximum value able to be assigned to the bar.")]
    [SerializeField] private float maxVal;
    [Tooltip("The minimum width that the bar can be resized to.")]
    [SerializeField] private float minWidth;
    [Tooltip("The color of the bar when its value is at its lowest.")]
    [SerializeField] private Color minColor;
    [Tooltip("The color of the bar when its value is at its highest.")]
    [SerializeField] private Color maxColor;

    /**
     * On startup, modifies the pivot and position of the bar object's RectTransform according to
     * the selected alignment.
     */
    private void Start()
    {
        RectTransform barRect = barImg.GetComponent<RectTransform>();
        switch (barType)
        {
            case BarType.ALIGN_LEFT:
                {
                    barRect.pivot = new Vector2(0, 0.5f);
                    //barRect.anchorMin = new Vector2(0, 0.5f);
                    //barRect.anchorMax = new Vector2(0, 0.5f);
                    barRect.anchoredPosition = Vector2.zero;
                    break;
                }
            case BarType.ALIGN_RIGHT:
                {
                    barRect.pivot = new Vector2(1, 0.5f);
                    //barRect.anchorMin = new Vector2(1, 0.5f);
                    //barRect.anchorMax = new Vector2(1, 0.5f);
                    barRect.anchoredPosition = Vector2.zero;
                    //GetComponent<RectTransform>().anchoredPosition += Vector2.right * GetComponent<RectTransform>().sizeDelta.x;
                    break;
                }
            case BarType.ZERO_CENTER:
                {
                    barRect.pivot = new Vector2(0.5f, 0.5f);
                    GetComponent<RectTransform>().anchoredPosition += Vector2.right * GetComponent<RectTransform>().sizeDelta.x / 2;
                    break;
                }
        }        
    }

    private void Update()
    {
        // Uncomment this line to manually change the bar's value in play mode. Otherwise it's just unnecessary overhead
        //SetValue(value);
    }

    /**
     * Assigns a new value to 'value', bounded by its min and max, then updates the appearance of the bar.
     * 
     * @param newVal is the new value to assign.
     */
    public float SetValue(float newVal)
    {
        newVal = Mathf.Clamp(newVal, minVal, maxVal);
        if (value != newVal)
        {
            //Debug.Log("About to Overwrite value from " + value + " to " + newVal);
            value = newVal;
            UpdateAppearance();
        }
        return value;
    }

    /**
     * Updates the appearance of the bar to reflect the current value and alignment type.
     */
    private void UpdateAppearance()
    {
        RectTransform barRect = barImg.GetComponent<RectTransform>();
        RectTransform myRect = GetComponent<RectTransform>();

        float valPercent = (value - minVal) / (maxVal - minVal);
        
        if (barType == BarType.ZERO_CENTER)
        {
            barRect.sizeDelta = new Vector3(Mathf.Abs(valPercent - 0.5f) * (myRect.sizeDelta.x - minWidth) + minWidth, myRect.sizeDelta.y);
            bool hangsLeft = valPercent - 0.5f < 0;
            barRect.anchoredPosition = new Vector3((hangsLeft ? -0.5f : 0.5f) * barRect.sizeDelta.x, barRect.anchoredPosition.y);
        }
        else
        {
            barRect.localScale = new Vector3(valPercent, 1, 1);
            //barRect.sizeDelta = new Vector2(valPercent * (myRect.sizeDelta.x - minWidth) + minWidth, myRect.sizeDelta.y);
        }

        // Lerp between minColor and maxColor to get the new bar color
        Vector4 minColLerp = new Vector4(minColor.r, minColor.g, minColor.b, minColor.a);
        Vector4 maxColLerp = new Vector4(maxColor.r, maxColor.g, maxColor.b, maxColor.a);
        Vector4 targetColLerp = Vector4.Lerp(minColLerp, maxColLerp, valPercent);
        barImg.GetComponent<Image>().color = new Color(targetColLerp.x, targetColLerp.y, targetColLerp.z, targetColLerp.w);
    }
}
