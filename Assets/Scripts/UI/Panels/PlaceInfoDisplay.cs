using SimManager.SimulationManager;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlaceInfoDisplay : MonoBehaviour, IInfoDisplay
{
    private Location displayedLocation;

    [SerializeField] private TextMeshProUGUI locationName;
    [SerializeField] private Transform tagCollection;
    [SerializeField] private GameObject tagPref;

    public void DisplayInfo(Selectable selected)
    {
        Place selectedPlace = (Place) selected;
        string selectedName = selectedPlace.placeName;
        Location prospLoc;
        if (!SimEngine.Locations.TryGetValue(selectedName, out prospLoc))
            return;

        locationName.text = prospLoc.Name.FirstCharacterToUpper();

        foreach (Transform child in tagCollection)
        {
            Destroy(child.gameObject);
        }

        foreach (string tag in prospLoc.Tags)
        {
            GameObject tagDisp = Instantiate(tagPref);
            tagDisp.GetComponent<TextMeshProUGUI>().text = tag;
            tagDisp.transform.SetParent(tagCollection);
        }
    }
}
