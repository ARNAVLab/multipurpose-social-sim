using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelationshipDisplay : MonoBehaviour
{
    [SerializeField] private GameObject relationshipEntryPref;
    private List<GameObject> relationshipList = new List<GameObject>();

    public void AddRelationship(string relateType, int relateeID)
    {
        GameObject newEntryObj = Instantiate(relationshipEntryPref);
        RelationshipEntry newEntry = newEntryObj.GetComponent<RelationshipEntry>();
        newEntry.SetRelationshipType(relateType);
        newEntry.SetRelateeName(AgentManager.GetAgent(relateeID).name);
        newEntry.transform.SetParent(GetComponent<Panel>().content.transform);
        newEntry.transform.SetAsLastSibling();
    }
}
