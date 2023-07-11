using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class MotiveDisplay : MonoBehaviour
{
    [SerializeField] private ActorInfoDisplay parentPanel;
    private string motiveName;
    [SerializeField] private TextMeshProUGUI motiveNameBox;
    [SerializeField] private TextMeshProUGUI motiveValue;
    [SerializeField] private UIValueBar motiveBar;

    public void SetMotiveName(string inputName, int padLen)
    {
        motiveName = inputName;

        StringBuilder sb = new StringBuilder();
        sb.Append(inputName);
        int padCount = padLen - inputName.Length;

        if (padCount <= 0)
        {
            motiveNameBox.text = inputName;
            return;
        }

        for (int i = 0; i < padCount; i++)
        {
            sb.Append(".");
        }
        motiveNameBox.text = sb.ToString();
    }

    public void SetMotiveValue(float inputVal)
    {
        motiveValue.text = motiveBar.SetValue(inputVal).ToString("F2");
    }

    public void OverwriteMotiveValue(float inputVal)
    {
        parentPanel.OverwriteMotiveValue(motiveName, inputVal);
    }
}
