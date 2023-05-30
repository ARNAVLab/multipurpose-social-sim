using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : Selectable
{
    private static int nextUnusedID = 0;

    [SerializeField] private GameObject selectionOutline;
    [SerializeField] private Color colorHover;
    [SerializeField] private Color colorSelect;
    [SerializeField] private Color colorFocus;

    public bool isFocused { get; private set; } = false;

    public int AgentID { get; set; }
    public AgentInfo Info;
    public Color displayColor;

    private void Start()
    {
        // Assign a unique identifier to this Agent
        AgentID = nextUnusedID;
        nextUnusedID++;

        // Register this Agent with the AgentManager (this will add it to a static Dictionary, keyed by ID)
        AgentManager.RegisterAgent(this);

        GetComponent<SpriteRenderer>().color = displayColor;
    }

    private enum OutlinePreset { NONE, HOVER, SELECT, FOCUS }
    private void SetOutline(OutlinePreset preset)
    {
        switch (preset)
        {
            case OutlinePreset.NONE:
                {
                    selectionOutline.SetActive(false);
                    break;
                }
            case OutlinePreset.HOVER:
                {
                    selectionOutline.SetActive(true);
                    selectionOutline.GetComponent<SpriteRenderer>().color = colorHover;
                    break;
                }
            case OutlinePreset.SELECT:
                {
                    selectionOutline.SetActive(true);
                    selectionOutline.GetComponent<SpriteRenderer>().color = colorSelect;
                    break;
                }
            case OutlinePreset.FOCUS:
                {
                    selectionOutline.SetActive(true);
                    selectionOutline.GetComponent<SpriteRenderer>().color = colorFocus;
                    break;
                }
        }
    }

    public override void OnHover()
    {
        // There is never a situation where isFocused is true AND isSelected isn't.
        if (!isSelected)
        {
            SetOutline(OutlinePreset.HOVER);
        }
    }
    public override void OnUnhover()
    {
        if (!isSelected)
        {
            SetOutline(OutlinePreset.NONE);
        }
    }
    public override void OnSelect() 
    {
        SetOutline(OutlinePreset.SELECT);
    }
    public override void OnDeselect()
    {
        SetOutline(OutlinePreset.NONE);
    }

    public void Focus()
    {
        isFocused = true;
        SetOutline(OutlinePreset.FOCUS);
    }

    public void Unfocus()
    {
        isFocused = false;
        SetOutline(OutlinePreset.SELECT);
    }
}
