using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TraitUI : MonoBehaviour
{
    public TextMeshProUGUI traitNameText;
    public Button removeButton;

    private string traitName;
    private System.Action<string> onRemoveCallback;

    public void Setup(string name, System.Action<string> onRemove)
    {
        traitName = name;
        traitNameText.text = name;
        onRemoveCallback = onRemove;

        removeButton.onClick.RemoveAllListeners();
        removeButton.onClick.AddListener(() => onRemoveCallback?.Invoke(traitName));
    }
}
