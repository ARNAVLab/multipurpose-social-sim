using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Anthology.SimulationManager;

public class ActorInfoDisplay : MonoBehaviour
{
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
        NPC npc;
        SimManager.NPCs.TryGetValue(agentName, out npc);

        if (npc == null)
        {
            return;
        }

        actorName.text = npc.Name;
        currentAction.text = npc.CurrentAction.Name;
        foreach (KeyValuePair<string, float> entry in npc.Motives)
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
}
