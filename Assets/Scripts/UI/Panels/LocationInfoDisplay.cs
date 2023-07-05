using Anthology.SimulationManager;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LocationInfoDisplay : MonoBehaviour
{
    private Location displayedLocation;

    [SerializeField] private TextMeshProUGUI locationName;
    [SerializeField] private Transform tagCollection;
    [SerializeField] private GameObject tagPref;

    public void DisplayLocationInfo(float xCoord, float yCoord)
    {
        Location.Coords locPos = new Location.Coords((int)xCoord, (int)yCoord);
        Location prospLoc;
        if (!SimManager.Locations.TryGetValue(locPos, out prospLoc))
            return;

        locationName.text = prospLoc.Name.FirstCharacterToUpper();

        foreach (string tag in prospLoc.Tags)
        {
            GameObject tagDisp = Instantiate(tagPref);
            tagDisp.GetComponent<TextMeshProUGUI>().text = tag;
            tagDisp.transform.SetParent(tagCollection);
        }
    }
}
