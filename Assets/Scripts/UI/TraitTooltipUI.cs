using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TraitTooltipUI : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI tooltipText;

    void Update()
    {
       
    }

    public void Show(string text)
    {
        tooltipText.text = text;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
