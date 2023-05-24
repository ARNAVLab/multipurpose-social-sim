using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Resizes and recolors the Image component of the GameObject based on a float value.
 * As the value approaches the maximum value, it stretches and shifts in color towards maxColor.
 */
public class UIValueBar : MonoBehaviour
{
    
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
    }

    private void Update()
    {
        // Uncomment this line to manually change the bar's value in the editor
        //SetValue(value);
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
        Debug.Log(valPercent);
        
        myRect.sizeDelta = new Vector2(valPercent * (maxWidth - minWidth) + minWidth, myRect.sizeDelta.y);

        // Lerp between minColor and maxColor to get the new bar color
        Vector4 minColLerp = new Vector4(minColor.r, minColor.g, minColor.b, minColor.a);
        Vector4 maxColLerp = new Vector4(maxColor.r, maxColor.g, maxColor.b, maxColor.a);
        Vector4 targetColLerp = Vector4.Lerp(minColLerp, maxColLerp, valPercent);
        GetComponent<Image>().color = new Color(targetColLerp.x, targetColLerp.y, targetColLerp.z, targetColLerp.w);
    }
}
