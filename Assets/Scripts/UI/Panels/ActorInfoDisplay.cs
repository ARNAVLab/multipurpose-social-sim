using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Anthology.SimulationManager;

public class ActorInfoDisplay : MonoBehaviour
{
    private NPC displayedNPC;
    [SerializeField] private TextMeshProUGUI actorName;
    [SerializeField] private TextMeshProUGUI currentAction;
    [SerializeField] private TextMeshProUGUI actionRemaining;

    [SerializeField] private List<string> motiveKeys;
    [SerializeField] private List<MotiveDisplay> motiveValues;
    private Dictionary<string, MotiveDisplay> motiveDisplayLookup;

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
        {
            return;
        }

        actorName.text = displayedNPC.Name;
        currentAction.text = displayedNPC.CurrentAction.Name;
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
