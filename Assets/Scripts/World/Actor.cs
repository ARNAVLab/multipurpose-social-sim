using Anthology.SimulationManager;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class Actor : Selectable
{
    private static int nextUnusedID = 0;

    [SerializeField] private SpriteRenderer mainSprite;
    [SerializeField] private SpriteRenderer interiorOutline;
    [SerializeField] private SpriteRenderer selectionOutline;

    [SerializeField] private Color colorHover;
    [SerializeField] private Color colorSelect;
    [SerializeField] private Color colorFocus;

    [SerializeField] private Color colorHigh;
    [SerializeField] private Color colorMid;
    [SerializeField] private Color colorLow;

    public bool isFocused { get; private set; } = false;

    public int AgentID { get; set; }
    public ActorInfo Info;
    public Color displayColor;

    private void Start()
    {
        // Assign a unique identifier to this Agent
        AgentID = nextUnusedID;
        nextUnusedID++;

        // Register this Agent with the AgentManager (this will add it to a static Dictionary, keyed by ID)
        WorldManager.RegisterAgent(this);
        WorldManager.actorsUpdated.AddListener(ReceiveAgentUpdates);
    }

    public void Init(string actorName)
    {
        // Update game object properties -- use Tilemap!!!
        gameObject.name = actorName;
        Info.name = actorName;
        displayColor = Random.ColorHSV();
        mainSprite.color = displayColor;

        ReceiveAgentUpdates();
    }

    public void ReceiveAgentUpdates()
    {
        NPC npcData;
        SimManager.NPCs.TryGetValue(Info.name, out npcData);

        if (npcData == null)
        {
            Debug.LogError("Uh oh! Name '" + Info.name + "' not found in NPC Dictionary.");
            return;
        }

        // Update internal state to reflect npc data
        Info.name = npcData.Name;
        Info.currentLocation.xPos = npcData.Coordinates.X;
        Info.currentLocation.yPos = npcData.Coordinates.Y;
        // TODO: Relationships, also need Destination and occupiedCounter from frontend
        npcData.Motives.TryGetValue("physical", out Info.motive.physical);
        npcData.Motives.TryGetValue("emotional", out Info.motive.emotional);
        npcData.Motives.TryGetValue("social", out Info.motive.social);
        npcData.Motives.TryGetValue("financial", out Info.motive.financial);
        npcData.Motives.TryGetValue("accomplishment", out Info.motive.accomplishment);
        Info.currentAction = npcData.CurrentAction.Name;

        // transform.position = new Vector3(Info.currentLocation.xPos + Random.Range(-0.2f, 0.2f), Info.currentLocation.yPos + Random.Range(-0.2f, 0.2f), 0);
    }

    private enum OutlinePreset { NONE, HOVER, SELECT, FOCUS }
    private void SetOutline(OutlinePreset preset)
    {
        switch (preset)
        {
            case OutlinePreset.NONE:
                {
                    selectionOutline.gameObject.SetActive(false);
                    break;
                }
            case OutlinePreset.HOVER:
                {
                    selectionOutline.gameObject.SetActive(true);
                    selectionOutline.GetComponent<SpriteRenderer>().color = colorHover;
                    break;
                }
            case OutlinePreset.SELECT:
                {
                    selectionOutline.gameObject.SetActive(true);
                    selectionOutline.GetComponent<SpriteRenderer>().color = colorSelect;
                    break;
                }
            case OutlinePreset.FOCUS:
                {
                    selectionOutline.gameObject.SetActive(true);
                    selectionOutline.GetComponent<SpriteRenderer>().color = colorFocus;
                    break;
                }
        }
    }

    public enum MotivePreset { PHYSICAL, EMOTIONAL, SOCIAL, FINANCIAL, ACCOMPLISHMENT, NONE}
    public void SetMotiveColor(MotivePreset preset) 
    {
        switch (preset)
        {
            case MotivePreset.NONE:
                {
                    mainSprite.GetComponent<SpriteRenderer>().color = displayColor;
                    break;
                }
            case MotivePreset.PHYSICAL:
                {
                    if(Info.motive.physical <= 2.5) {
                        mainSprite.GetComponent<SpriteRenderer>().color = colorLow;
                    } else if(Info.motive.physical >= 3.5) {
                        mainSprite.GetComponent<SpriteRenderer>().color = colorHigh;
                    } else {
                        mainSprite.GetComponent<SpriteRenderer>().color = colorMid;
                    }
                    break;
                }
            case MotivePreset.EMOTIONAL:
                {
                    if(Info.motive.emotional <= 2.5) {
                        mainSprite.GetComponent<SpriteRenderer>().color = colorLow;
                    } else if(Info.motive.emotional >= 3.5) {
                        mainSprite.GetComponent<SpriteRenderer>().color = colorHigh;
                    } else {
                        mainSprite.GetComponent<SpriteRenderer>().color = colorMid;
                    }
                    break;
                }
            case MotivePreset.SOCIAL:
                {
                    if(Info.motive.social <= 2.5) {
                        mainSprite.GetComponent<SpriteRenderer>().color = colorLow;
                    } else if(Info.motive.social >= 3.5) {
                        mainSprite.GetComponent<SpriteRenderer>().color = colorHigh;
                    } else {
                        mainSprite.GetComponent<SpriteRenderer>().color = colorMid;
                    }
                    break;
                }
            case MotivePreset.FINANCIAL:
                {
                    if(Info.motive.financial <= 2.5) {
                        mainSprite.GetComponent<SpriteRenderer>().color = colorLow;
                    } else if(Info.motive.financial >= 3.5) {
                        mainSprite.GetComponent<SpriteRenderer>().color = colorHigh;
                    } else {
                        mainSprite.GetComponent<SpriteRenderer>().color = colorMid;
                    }
                    break;
                }
            case MotivePreset.ACCOMPLISHMENT:
                {
                    if(Info.motive.accomplishment <= 2.5) {
                        mainSprite.GetComponent<SpriteRenderer>().color = colorLow;
                    } else if(Info.motive.accomplishment >= 3.5) {
                        mainSprite.GetComponent<SpriteRenderer>().color = colorHigh;
                    } else {
                        mainSprite.GetComponent<SpriteRenderer>().color = colorMid;
                    }
                    break;
                }
        }
    }

    public void OnNone() 
    {
        SetMotiveColor(MotivePreset.NONE);
    }
    public void OnPhysical() 
    {
        SetMotiveColor(MotivePreset.PHYSICAL);
    }
    public void OnEmotional() 
    {
        SetMotiveColor(MotivePreset.EMOTIONAL);
    }
    public void OnSocial() 
    {
        SetMotiveColor(MotivePreset.SOCIAL);
    }
    public void OnFinancial() 
    {
        SetMotiveColor(MotivePreset.FINANCIAL);
    }
    public void OnAccomplishment() 
    {
        SetMotiveColor(MotivePreset.ACCOMPLISHMENT);
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
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        SetOutline(OutlinePreset.FOCUS);
    }

    public void Unfocus()
    {
        isFocused = false;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        SetOutline(OutlinePreset.SELECT);
    }
}
