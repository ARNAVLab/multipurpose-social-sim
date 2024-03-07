using SimManager.SimulationManager;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class Actor : Selectable
{
    private static int nextUnusedID = 0;

    [Tooltip("The primary SpriteRenderer representing this Actor.")]
    [SerializeField] private SpriteRenderer mainSprite;
    [Tooltip("The background SpriteRenderer that forms an outline around this Actor when visible.")]
    [SerializeField] private SpriteRenderer interiorOutline;
    [SerializeField] private Color colorHigh;
    [SerializeField] private Color colorMid;
    [SerializeField] private Color colorLow;

    public int AgentID { get; set; }
    public ActorInfo Info;
    public Color displayColor;

    public MotivePreset currentMotiveDisplay;

    private void Start()
    {
        // Assign a unique identifier to this Agent
        AgentID = nextUnusedID;
        nextUnusedID++;

        // Register this Agent with the AgentManager (this will add it to a static Dictionary, keyed by ID)
        WorldManager.RegisterAgent(this);
        WorldManager.simUpdated.AddListener(ReceiveAgentUpdates);
    }

    public void Init(string actorName)
    {
        gameObject.name = actorName;
        Info.name = actorName;
        displayColor = Random.ColorHSV();
        mainSprite.color = displayColor;
        currentMotiveDisplay = MotivePreset.NONE;

        ReceiveAgentUpdates();
    }

    public void ReceiveAgentUpdates()
    {
        NPC npcData;
        SimEngine.NPCs.TryGetValue(Info.name, out npcData);

        if (npcData == null)
        {
            Debug.LogError("Uh oh! Name '" + Info.name + "' not found in NPC Dictionary.");
            return;
        }

        // Update internal state to reflect npc data
        Info.name = npcData.Name;
        Info.currentLocation = npcData.Location;
        Info.destination = npcData.Destination;
        // TODO: Relationships, also need Destination and occupiedCounter from frontend
        npcData.Motives.TryGetValue("physical", out Info.motive.physical);
        npcData.Motives.TryGetValue("emotional", out Info.motive.emotional);
        npcData.Motives.TryGetValue("social", out Info.motive.social);
        npcData.Motives.TryGetValue("financial", out Info.motive.financial);
        npcData.Motives.TryGetValue("accomplishment", out Info.motive.accomplishment);
        Info.currentAction = npcData.CurrentAction.Name;
        SetMotiveColor(currentMotiveDisplay);

        // transform.position = new Vector3(Info.currentLocation.xPos + Random.Range(-0.2f, 0.2f), Info.currentLocation.yPos + Random.Range(-0.2f, 0.2f), 0);
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
        currentMotiveDisplay = MotivePreset.NONE;
    }
    public void OnPhysical() 
    {
        SetMotiveColor(MotivePreset.PHYSICAL);
        currentMotiveDisplay = MotivePreset.PHYSICAL;
    }
    public void OnEmotional() 
    {
        SetMotiveColor(MotivePreset.EMOTIONAL);
        currentMotiveDisplay = MotivePreset.EMOTIONAL;
    }
    public void OnSocial() 
    {
        SetMotiveColor(MotivePreset.SOCIAL);
        currentMotiveDisplay = MotivePreset.SOCIAL;
    }
    public void OnFinancial() 
    {
        SetMotiveColor(MotivePreset.FINANCIAL);
        currentMotiveDisplay = MotivePreset.FINANCIAL;
    }
    public void OnAccomplishment() 
    {
        SetMotiveColor(MotivePreset.ACCOMPLISHMENT);
        currentMotiveDisplay = MotivePreset.ACCOMPLISHMENT;
    }
    
    public override void Focus()
    {
        mainSprite.sortingOrder = short.MaxValue;
        interiorOutline.sortingOrder = short.MaxValue - 1;
        selectionOutline.sortingOrder = short.MaxValue - 2;

        base.Focus();
    }

    public override void Unfocus()
    {
        mainSprite.sortingOrder = 2;
        interiorOutline.sortingOrder = 1;
        selectionOutline.sortingOrder = 0;

        base.Unfocus();
    }
}
