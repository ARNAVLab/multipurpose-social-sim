using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelationshipDisplay : MonoBehaviour
{
    [SerializeField] private GameObject relationshipEntryPref;
    private List<GameObject> relationshipList = new List<GameObject>();

    private void Start()
    {
        WorldManager.simUpdated.AddListener(SimUpdateListener);
    }

    private void SimUpdateListener()
    {
        if (UIManager.GetInstance().GetSelectMode() == UIManager.SelectType.ACTORS)
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
