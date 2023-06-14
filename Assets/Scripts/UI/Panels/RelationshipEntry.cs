using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RelationshipEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI relateTypeText;
    [SerializeField] private TextMeshProUGUI relateeNameText;
    [SerializeField] private Image relateePortrait;
    [SerializeField] private TextMeshProUGUI relationValenceText;

    public void SetRelationshipType(string input)
    {
        relateTypeText.text = input;
    }

    public void SetRelateeName(string input)
    {
        relateeNameText.text = input;
    }

    public void SetRelationshipValence(float value)
    {
        relationValenceText.text = value.ToString("#.##");
    }
}
