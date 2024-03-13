using SimManager.SimulationManager;
using SimManager.HistoryManager;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using UnityEngine;
using UnityEngine.Events;

public class WorldManager : MonoBehaviour
{
    public static WorldManager instance;

    public ButtonManager buttonManager;

    [SerializeField] private GameObject actorPref;
    [SerializeField] private GameObject locationPref;

    public static UnityEvent simUpdated;

    public static Dictionary<int, Actor> actors = new Dictionary<int, Actor>();

    //private string pathsPath = "Assets/Scripts/SimManager/Data/Paths.json";
    private string pathsPath = "Assets/Scripts/SimManager/Data/Paths.json";

    private bool physical;

    private bool emotional;

    private bool social;

    private bool financial;
    
    private bool accomplishment;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            // Jump-starts the Simulation Manager, allowing for communication between the User Interface, Knowledge, and Reality sims.
            SimEngine.Init(pathsPath, typeof(AnthologyRS), typeof(LyraKS), typeof(MongoHM));
            simUpdated = new UnityEvent();
        }
    }

    private void Start()
    {
        InitWorld();
    }

    /**
     * Instantiates objects representing the agents and locations from the backend simulation.
     */
    private void InitWorld()
    {
        foreach (NPC npc in SimEngine.NPCs.Values)
        {
            Actor spawnedActor = Instantiate(actorPref).GetComponent<Actor>();
            if (spawnedActor == null)
            {
                Debug.LogError("Uh oh! Actor prefab doesn't have attached Actor component!");
                break;
            }
            Location.Coords loc = SimEngine.Locations[npc.Location].Coordinates;
            spawnedActor.transform.position = new Vector3(loc.X, loc.Y, 0);
            spawnedActor.Init(npc.Name);
        }

        foreach (Location loc in SimEngine.Locations.Values)
        {
            GameObject spawnedLocation = Instantiate(locationPref);
            spawnedLocation.name = loc.Name;
            spawnedLocation.transform.position = new Vector3(loc.Coordinates.X, loc.Coordinates.Y, 0);
            Place placeComp = spawnedLocation.GetComponent<Place>();
            placeComp.placeName = loc.Name;
            // TODO: Band-aid Code: Place in more appropriate spot later
            if (loc.Tags.Contains("Building"))
                placeComp.ChangeColorToPreset(Place.Preset.BUILDING);
            else if (loc.Tags.Contains("Road"))
                placeComp.ChangeColorToPreset(Place.Preset.ROAD);
            else if (loc.Tags.Contains("Forest"))
                placeComp.ChangeColorToPreset(Place.Preset.FOREST);
            else if (loc.Tags.Contains("Park"))
                placeComp.ChangeColorToPreset(Place.Preset.PARK);
        }
        buttonManager = GetComponent<ButtonManager>();
    }

    /**
     * Attempts to set which Agent is "focused" (is selected/hovered on the frontend).
     * This fails if there is currently a focused Agent.
     * @param targetID is the ID of the Agent to focus.
     * @return whether the currently focused Agent successfully changed.
     */
    //public static bool FocusAgent(int targetID)
    //{
    //    // TODO: Should a check occur for attempting to focus the currently focused Agent (i.e. unfocus it)?
    //    if (focused > -1)
    //        return false; // Focus already occupied by another Agent

    //    focused = targetID;
    //    return true;
    //}

    /**
     * Attempts to unfocus the currently "focused" (is selected/hovered on the frontend) Agent.
     * This fails if the currently focused Agent's ID does not match the passed ID.
     * @param targetID is the ID of the Agent to unfocus.
     * @return whether the currently focused Agent was successfully unfocused.
     */
    //public static bool UnfocusAgent(int targetID)
    //{
    //    if (focused != targetID)
    //        return false; // Focus not occupied by Agent with target ID

    //    focused = -1;
    //    return true;
    //}

    /**
     * Attempts to add a new Agent to the static Dictionary, using its AgentID as the key.
     * This fails if the Dictionary already contains a value with the desired key.
     * @param registree is the Agent to register.
     * @return whether the addition was successful or not.
     */
    public static bool RegisterAgent(Actor registree)
    {
        if (!actors.ContainsKey(registree.AgentID))
        {
            actors.Add(registree.AgentID, registree);
            return true;
        }
        return false;
    }

    public static Actor GetAgent(int agentID)
    {
        return actors[agentID];
    }

    public void SetAgentMotiveNone()
    {
        buttonManager.TogglePhysicalOff();
        buttonManager.ToggleEmotionalOff();
        buttonManager.ToggleSocialOff();
        buttonManager.ToggleFinancialOff();
        buttonManager.ToggleAccomplishmentOff();
        foreach(KeyValuePair<int,Actor> a in actors) {
            Actor act = a.Value;
            act.OnNone();
        }
    }

    public void SetAgentMotivePhysical()
    {
        if(physical){
            SetAgentMotiveNone();
            physical = false;
        } else {
            buttonManager.TogglePhysicalOn();
            buttonManager.ToggleEmotionalOff();
            buttonManager.ToggleSocialOff();
            buttonManager.ToggleFinancialOff();
            buttonManager.ToggleAccomplishmentOff();
            physical = true;
            emotional = false;
            social = false;
            financial = false;
            accomplishment = false;
            foreach(KeyValuePair<int,Actor> a in actors) {
                Actor act = a.Value;
                act.OnPhysical();
            }
        }
    }

    public void SetAgentMotiveEmotional()
    {
        if(emotional){
            SetAgentMotiveNone();
            emotional = false;
            //Change button back to pin
        } else {
            buttonManager.TogglePhysicalOff();
            buttonManager.ToggleEmotionalOn();
            buttonManager.ToggleSocialOff();
            buttonManager.ToggleFinancialOff();
            buttonManager.ToggleAccomplishmentOff();
            physical = false;
            emotional = true;
            social = false;
            financial = false;
            accomplishment = false;
            foreach(KeyValuePair<int,Actor> a in actors) {
                Actor act = a.Value;
                act.OnEmotional();
            }
            //Find canvas button to change to cancel
        }
    }

    public void SetAgentMotiveSocial()
    {
        if(social){
            SetAgentMotiveNone();
            social = false;
        } else {
            buttonManager.TogglePhysicalOff();
            buttonManager.ToggleEmotionalOff();
            buttonManager.ToggleSocialOn();
            buttonManager.ToggleFinancialOff();
            buttonManager.ToggleAccomplishmentOff();
            physical = false;
            emotional = false;
            social = true;
            financial = false;
            accomplishment = false;
            foreach(KeyValuePair<int,Actor> a in actors) {
                Actor act = a.Value;
                act.OnSocial();
            }
        }
    }

    public void SetAgentMotiveFinancial()
    {
        if(financial){
            SetAgentMotiveNone();
            financial = false;
        } else {
            buttonManager.TogglePhysicalOff();
            buttonManager.ToggleEmotionalOff();
            buttonManager.ToggleSocialOff();
            buttonManager.ToggleFinancialOn();
            buttonManager.ToggleAccomplishmentOff();
            physical = false;
            emotional = false;
            social = false;
            financial = true;
            accomplishment = false;
            foreach(KeyValuePair<int,Actor> a in actors) {
                Actor act = a.Value;
                act.OnFinancial();
            }
        }
    }

    public void SetAgentMotiveAccomplishment()
    {
        if(accomplishment){
            SetAgentMotiveNone();
            accomplishment = false;
        } else {
            buttonManager.TogglePhysicalOff();
            buttonManager.ToggleEmotionalOff();
            buttonManager.ToggleSocialOff();
            buttonManager.ToggleFinancialOff();
            buttonManager.ToggleAccomplishmentOn();
            physical = false;
            emotional = false;
            social = false;
            financial = false;
            accomplishment = true;
            foreach(KeyValuePair<int,Actor> a in actors) {
                Actor act = a.Value;
                act.OnAccomplishment();
            }
        }
    }

    void OnApplicationQuit()
    {
        Debug.Log("Application ending after " + Time.time + " seconds");
        SimEngine.ExportLogs();
    }

}
