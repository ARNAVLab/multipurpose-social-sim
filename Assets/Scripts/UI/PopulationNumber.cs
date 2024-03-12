using Anthology.Models;
using SimManager.SimulationManager;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PopulationNumber : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI population;

    // Start is called before the first frame update
    void Start()
    {
        UpdatePopulation();
    }

    void UpdatePopulation()
    {

        //Place selectedPlace = (Place)selected;
        //string selectedName = selectedPlace.placeName;
        //Location prospLoc;
        //if (!SimEngine.Locations.TryGetValue(selectedName, out prospLoc))
        //    return;

        //locationName.text = prospLoc.Name.FirstCharacterToUpper();

        if (population.text == "0")
        {
            population.enabled = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
