using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SimManager.SimulationManager;

public class ActorInfoDisplay : MonoBehaviour, IInfoDisplay
{
    private NPC displayedNPC;
    [Tooltip("Text box which displays the Actor's name.")]
    [SerializeField] private TextMeshProUGUI actorName;

    [Tooltip("Text box which displays either 'heading towards' or 'currently at' with respect to the Actor's location.")]
    [SerializeField] private TextMeshProUGUI locationPrefix;
    [Tooltip("Text box which displays the Actor's target location.")]
    [SerializeField] private TextMeshProUGUI locationPlace;
    [Tooltip("Text box which displays the Actor's current action.")]
    [SerializeField] private TextMeshProUGUI currentAction;
    [Tooltip("Text box which displays the remaining time steps on the Actor's current action.")]
    [SerializeField] private TextMeshProUGUI actionRemaining;

    [Tooltip("The names of each motive driving this Actor.")]
    [SerializeField] private List<string> motiveKeys;
    [Tooltip("The UI groups that display the current value of each motive, respectively.")]
    [SerializeField] private List<MotiveDisplay> motiveValues;

    private bool motivesInit = false;
    private Dictionary<string, MotiveDisplay> motiveDisplayLookup;

    [Tooltip("The panel which displays Actor relationships.")]
    [SerializeField] RelationshipDisplay relationsDisp;
    [Tooltip("The planel which displays the Actor journal.")]
    [SerializeField] JournalDisplay journalDisp;

    private const string AT_LOCATION = "Currently at";
    private const string TO_LOCATION = "Traveling to";

    public void InitMotiveDisplays()
    {
        motiveDisplayLookup = new Dictionary<string, MotiveDisplay>();
        for (int i = 0; i < motiveKeys.Count; i++)
        {
			string mkey = motiveKeys[i];
			mkey = mkey[0].ToString().ToUpper() + mkey.Substring(1);
            motiveDisplayLookup.Add(mkey, motiveValues[i]);
        }
    }

    public void DisplayInfo(Selectable selected)
    {
        if (!motivesInit)
        {
            motivesInit = true;
            InitMotiveDisplays();
        }

        Actor selectedActor = (Actor) selected;
        SimEngine.NPCs.TryGetValue(selectedActor.name, out displayedNPC);

        if (displayedNPC == null)
            return;

        bool travelling = displayedNPC.Destination.Length != 0;

        locationPrefix.text = travelling ? TO_LOCATION : AT_LOCATION;
        locationPlace.text = travelling ? displayedNPC.Destination : displayedNPC.Location;

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

        relationsDisp.DisplayActorRelations(displayedNPC);

        var journalText = SimManager.SimulationManager.SimEngine.GetLog(displayedNPC.Name);

        journalDisp.DisplayActorJournal(journalText);
    }

    public void OverwriteMotiveValue(string motiveName, float newValue)
    {
        if (displayedNPC == null)
            return;

        displayedNPC.Motives[motiveName] = newValue;
        // Manually setting Dirty to true temporarily, until a setter exists for individual elements in Motives
        displayedNPC.Dirty = true;
    }
}
