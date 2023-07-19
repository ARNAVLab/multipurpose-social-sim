using Anthology.Models;
using Anthology.SimulationManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelationshipDisplay : MonoBehaviour
{
    [SerializeField] private GameObject relationshipEntryPref;
    private Panel relationPanel;
    private List<GameObject> relationshipList = new List<GameObject>();

    private void Start()
    {
        relationPanel = GetComponent<Panel>();
        //WorldManager.simUpdated.AddListener(SimUpdateListener);
    }

    //private void SimUpdateListener()
    //{
    //    if (relationPanel.IsShown)
    //    {
    //        SimManager.NPCs.
    //    }
    //}

    public void DisplayActorRelations(NPC npc)
    {
        foreach (Relationship r in npc.Relationships)
        {
            
        }
    }

    public void AddRelationship(string relateType, int relateeID)
    {
        GameObject newEntryObj = Instantiate(relationshipEntryPref);
        RelationshipEntry newEntry = newEntryObj.GetComponent<RelationshipEntry>();
        newEntry.SetRelationshipType(relateType);
        newEntry.SetRelateeName(WorldManager.GetAgent(relateeID).name);
        newEntry.transform.SetParent(GetComponent<Panel>().content.transform);
        newEntry.transform.SetAsLastSibling();
    }
}
