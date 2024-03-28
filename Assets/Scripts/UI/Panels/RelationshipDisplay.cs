using Anthology.Models;
using SimManager.SimulationManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelationshipDisplay : MonoBehaviour
{
    [SerializeField] private GameObject relationshipEntryPref;
    private Panel relationPanel;
    private List<GameObject> relationshipList = new List<GameObject>();
    private Transform relationshipEntryContainer;

    private void Start()
    {
        relationPanel = GetComponent<Panel>();
        relationshipEntryContainer = GetComponent<Panel>().content.transform;
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
        foreach (GameObject relationshipObj in relationshipList)
        {
            Destroy(relationshipObj);
        }
        relationshipList.Clear();
        foreach (SimManager.SimulationManager.Relationship r in npc.Relationships)
        {
            AddRelationship(r.Type, r.With);
        }
    }

    public void AddRelationship(string relateType, string relateeName)
    {
        GameObject newEntryObj = Instantiate(relationshipEntryPref);
        RelationshipEntry newEntry = newEntryObj.GetComponent<RelationshipEntry>();
        newEntry.SetRelationshipType(relateType);
        newEntry.SetRelateeName(relateeName);
        
        newEntryObj.transform.SetParent(relationshipEntryContainer);
        //newEntry.transform.SetAsLastSibling();
        newEntryObj.transform.localScale = Vector3.one;
        relationshipList.Add(newEntryObj);
    }
}
