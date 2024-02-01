using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputBox : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private InputField inputField;

    public void OnPointerDown(PointerEventData eventData)
    {
        inputField.Select();
    }
}
