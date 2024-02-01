using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/**
 * A modified version of the Text Mesh Pro UI component.
 * Continuously updates its preferred Layout Width to be proportional to the number of characters in the text field.
 * The effect of this is to cause the attached Layout Element to only be resized when the text is beginning to be cut off.
 */
public class ScalingText : TextMeshProUGUI
{
    /**
     * Continuously updates the attached LayoutElement's preferred width, such that this object is
     * only resized when the text is about to be cut off.
     * 
     * TODO: In-line tags that don't appear on-screen unintendedly still increase the preferred width.
     */
    void Update()
    {
        LayoutElement myLayout = GetComponent<LayoutElement>();
        if (myLayout != null)
        {
            myLayout.preferredWidth = fontSize * text.Length;
        }
    }
}
