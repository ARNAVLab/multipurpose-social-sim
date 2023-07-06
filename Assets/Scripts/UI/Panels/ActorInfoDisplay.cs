using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Anthology.SimulationManager;
using System;

public class ActorInfoDisplay : MonoBehaviour
{
    private NPC displayedNPC;
    [SerializeField] private TextMeshProUGUI actorName;

    [SerializeField] private TextMeshProUGUI locationPrefix;
    [SerializeField] private TextMeshProUGUI locationPlace;

    [SerializeField] private TextMeshProUGUI currentAction;
    [SerializeField] private TextMeshProUGUI actionRemaining;

    [SerializeField] private List<string> motiveKeys;
    [SerializeField] private List<MotiveDisplay> motiveValues;
    private Dictionary<string, MotiveDisplay> motiveDisplayLookup;

    private const string AT_LOCATION = "Currently at";
    private const string TO_LOCATION = "Traveling to";

    private void Start()
    {
        InitMotiveDisplays();
    }

    public void InitMotiveDisplays()
    {
        motiveDisplayLookup = new Dictionary<string, MotiveDisplay>();
        for (int i = 0; i < motiveKeys.Count; i++)
        {
            motiveDisplayLookup.Add(motiveKeys[i], motiveValues[i]);
        }
    }

    public void DisplayAgentInfo(string agentName)
    {
        SimManager.NPCs.TryGetValue(agentName, out displayedNPC);

        if (displayedNPC == null)
            return;

        bool travelling = displayedNPC.Destination.Length != 0;

        locationPrefix.text = travelling ? TO_LOCATION : AT_LOCATION;

        string currentLocationName = "ERR: Coord Mismatch";
        Location currentLocation;
        if (SimManager.Locations.TryGetValue(new Location.Coords((int)displayedNPC.Coordinates.X, (int)displayedNPC.Coordinates.Y), out currentLocation))
            currentLocationName = currentLocation.Name;
        locationPlace.text = travelling ? displayedNPC.Destination : currentLocationName;

        actorName.text = displayedNPC.Name;
        currentAction.text = displayedNPC.CurrentAction.Name;
        actionRemaining.text = "for " + displayedNPC.ActionCounter.ToString() + " steps";

        foreach (KeyValuePair<string, float> entry in displayedNPC.Motives)
        {
            MotiveDisplay md;
            motiveDisplayLookup.TryGetValue(entry.Key, out md);

            if (md != null)
            {
                md.SetMotiveValue(entry.Value);
            }
            else
            {
                Debug.LogError("Motive name mismatch!!");
            }
        }
    }

    public void OverewriteMotiveValue(string motiveName, float newValue)
    {
        if (displayedNPC == null)
            return;

        displayedNPC.Motives[motiveName] = newValue;
        // Manually setting Dirty to true temporarily, until a setter exists for individual elements in Motives
        displayedNPC.Dirty = true;
    }
}
