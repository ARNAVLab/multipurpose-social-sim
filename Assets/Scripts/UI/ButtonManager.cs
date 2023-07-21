using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] GameObject physical;
    [SerializeField] GameObject emotional;
    [SerializeField] GameObject social;
    [SerializeField] GameObject financial;
    [SerializeField] GameObject accomplishment;

    [SerializeField] Sprite cancelSprite;

    [SerializeField] Sprite pinSprite;


    public void TogglePhysicalOn()
    {
        physical.GetComponent<Image>().sprite = cancelSprite;
    }

    public void TogglePhysicalOff()
    {
        physical.GetComponent<Image>().sprite = pinSprite;
    }

    public void ToggleEmotionalOn()
    {
        emotional.GetComponent<Image>().sprite = cancelSprite;
    }

    public void ToggleEmotionalOff()
    {
        emotional.GetComponent<Image>().sprite = pinSprite;
    }

    public void ToggleSocialOn()
    {
        social.GetComponent<Image>().sprite = cancelSprite;
    }

    public void ToggleSocialOff()
    {
        social.GetComponent<Image>().sprite = pinSprite;
    }

    public void ToggleFinancialOn()
    {
        financial.GetComponent<Image>().sprite = cancelSprite;
    }

    public void ToggleFinancialOff()
    {
        financial.GetComponent<Image>().sprite = pinSprite;
    }

    public void ToggleAccomplishmentOn()
    {
        accomplishment.GetComponent<Image>().sprite = cancelSprite;
    }

    public void ToggleAccomplishmentOff()
    {
        accomplishment.GetComponent<Image>().sprite = pinSprite;
    }

    


}
