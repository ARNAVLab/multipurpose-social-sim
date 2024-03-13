using Anthology.Models;
using SimManager.SimulationManager;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PopulationNumber : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI populationText;
    //[SerializeField] Location location;
    public int population;
    [SerializeField] private GameObject popSqure;
    int tmpPop;
    Place place;

    // Start is called before the first frame update
    void Start()
    {
        place = GetComponent<Place>();
        population = SimEngine.Locations[place.name].AgentsPresent.Count;
        UpdatePopulation();
        Debug.Log(place.name + " " + population);
    }

    public void UpdatePopulation()
    {

        populationText.text = population.ToString();

        if (population == 0 || population == 1)
        {
            populationText.enabled = false;
            popSqure.GetComponent<SpriteRenderer>().enabled = false;
        } else
        {
            populationText.enabled = true;
            popSqure.GetComponent<SpriteRenderer>().enabled = true;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        tmpPop = SimEngine.Locations[place.name].AgentsPresent.Count;
        //Update the population text
        population = tmpPop;
        UpdatePopulation();
        
    }
}
