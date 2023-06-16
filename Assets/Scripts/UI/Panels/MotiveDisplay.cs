using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class MotiveDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI motiveName;
    [SerializeField] private TextMeshProUGUI motiveValue;
    [SerializeField] private UIValueBar motiveBar;

    public void SetMotiveName(string inputName, int padLen)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(inputName);
        int padCount = padLen - inputName.Length;

        if (padCount <= 0)
        {
            motiveName.text = inputName;
            return;
        }

        for (int i = 0; i < padCount; i++)
        {
            sb.Append(".");
        }
        motiveName.text = sb.ToString();
    }

    public void SetMotiveValue(float inputVal)
    {
        motiveValue.text = motiveBar.SetValue(inputVal).ToString("#.##");
    }
}
