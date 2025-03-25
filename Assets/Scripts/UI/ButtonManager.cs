using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] GameObject nonNativeSpeaker;
    [SerializeField] GameObject senior;
    [SerializeField] GameObject inaccessible;
    [SerializeField] GameObject financial;
    [SerializeField] GameObject disabilityMedicalNeeds;

    [SerializeField] Sprite cancelSprite;

    [SerializeField] Sprite pinSprite;


    public void ToggleNonNativeSpeakerOn()
    {
        nonNativeSpeaker.GetComponent<Image>().sprite = cancelSprite;
    }

    public void ToggleNonNativeSpeakerOff()
    {
        nonNativeSpeaker.GetComponent<Image>().sprite = pinSprite;
    }

    public void ToggleSeniorOn()
    {
        senior.GetComponent<Image>().sprite = cancelSprite;
    }

    public void ToggleSeniorOff()
    {
        senior.GetComponent<Image>().sprite = pinSprite;
    }

    public void ToggleInaccessibleOn()
    {
        inaccessible.GetComponent<Image>().sprite = cancelSprite;
    }

    public void ToggleInaccessibleOff()
    {
        inaccessible.GetComponent<Image>().sprite = pinSprite;
    }

    public void ToggleFinancialOn()
    {
        financial.GetComponent<Image>().sprite = cancelSprite;
    }

    public void ToggleFinancialOff()
    {
        financial.GetComponent<Image>().sprite = pinSprite;
    }

    public void ToggleDisabilityMedicalNeedsOn()
    {
        disabilityMedicalNeeds.GetComponent<Image>().sprite = cancelSprite;
    }

    public void ToggleDisabilityMedicalNeedsOff()
    {
        disabilityMedicalNeeds.GetComponent<Image>().sprite = pinSprite;
    }

    


}
