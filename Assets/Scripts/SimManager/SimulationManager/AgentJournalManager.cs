using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class Turn
{
    public string speaker;  // e.g., "Eleanor"
    public string dialog;   // e.g., "Hello, how are you?"
}

[Serializable]
public class Conversation
{
    public string type;       // e.g., "communication"
    public string withAgent;  // e.g., "Thomas"
    public string topic;      // e.g., "Evacuation Plans"
    public List<Turn> turns;
    public string timestamp;  // e.g., "2025-03-25 14:05:00"
}

[Serializable]
public class AgentJournal
{
    public List<Conversation> conversations = new List<Conversation>();
}

/// <summary>
/// Manages logging agent conversations to a JSON file (AgentJournal.json).
/// </summary>
public class AgentJournalManager : MonoBehaviour
{
    // Name of the JSON file to store the journal
    private string journalFileName = "AgentJournal.json";
    // Full path to the JSON file
    private string journalFilePath;
    // In-memory copy of the journal
    private AgentJournal agentJournal;

    private void Awake()
    {
        // If you want to save in Assets/Scripts/SimManager/Data/Survey:
        string directoryPath = Path.Combine(Application.dataPath, "Scripts", "SimManager", "Data", "Survey");
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        journalFilePath = Path.Combine(directoryPath, journalFileName);
        Debug.Log("Journal will be saved at: " + journalFilePath);
        
        // Alternatively, use persistentDataPath:
        // journalFilePath = Path.Combine(Application.persistentDataPath, journalFileName);

        LoadJournal();
    }

    /// <summary>
    /// Loads the journal from JSON if it exists; otherwise, creates a new journal.
    /// </summary>
    private void LoadJournal()
    {
        if (File.Exists(journalFilePath))
        {
            string json = File.ReadAllText(journalFilePath);
            agentJournal = JsonUtility.FromJson<AgentJournal>(json);
            if (agentJournal == null)
            {
                agentJournal = new AgentJournal();
            }
        }
        else
        {
            agentJournal = new AgentJournal();
        }
    }

    /// <summary>
    /// Saves the current journal to JSON.
    /// </summary>
    private void SaveJournal()
    {
        string json = JsonUtility.ToJson(agentJournal, true);
        File.WriteAllText(journalFilePath, json);
    }

    /// <summary>
    /// Adds a conversation record to the journal and saves it.
    /// </summary>
    /// <param name="type">Type of conversation (e.g., communication).</param>
    /// <param name="withAgent">The other agent's name.</param>
    /// <param name="topic">Topic of the conversation.</param>
    /// <param name="turns">List of dialog turns.</param>
    /// <param name="timeStamp">Time of the conversation.</param>
    public void AddConversation(string type, string withAgent, string topic, List<Turn> turns, DateTime timeStamp)
    {
        Conversation conversation = new Conversation
        {
            type = type,
            withAgent = withAgent,
            topic = topic,
            turns = turns,
            timestamp = timeStamp.ToString("yyyy-MM-dd HH:mm:ss")
        };

        agentJournal.conversations.Add(conversation);

        Debug.Log($"Added conversation: {type} with {withAgent} about {topic} at {timeStamp}");
        SaveJournal();
    }


    /// <summary>
    /// Debug utility: prints the entire journal to the Console.
    /// </summary>
    public void PrintJournalToConsole()
    {
        foreach (var convo in agentJournal.conversations)
        {
            Debug.Log($"Type: {convo.type}, With: {convo.withAgent}, Topic: {convo.topic}, Timestamp: {convo.timestamp}");
            if (convo.turns != null)
            {
                foreach (var turn in convo.turns)
                {
                    Debug.Log($"  {turn.speaker}: {turn.dialog}");
                }
            }
        }
    }
}
