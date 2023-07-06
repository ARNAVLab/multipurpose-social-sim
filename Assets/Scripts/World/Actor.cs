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
        gameObject.name = actorName;
        Info.name = actorName;
        mainSprite.color = Random.ColorHSV();

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

    public override void Focus()
    {
        isFocused = true;

        mainSprite.sortingOrder = short.MaxValue;
        interiorOutline.sortingOrder = short.MaxValue - 1;
        selectionOutline.sortingOrder = short.MaxValue - 2;

        SetOutline(OutlinePreset.FOCUS);
    }

    public override void Unfocus()
    {
        isFocused = false;

        mainSprite.sortingOrder = 2;
        interiorOutline.sortingOrder = 1;
        selectionOutline.sortingOrder = 0;

        SetOutline(isSelected ? OutlinePreset.SELECT : OutlinePreset.NONE);
    }
}
