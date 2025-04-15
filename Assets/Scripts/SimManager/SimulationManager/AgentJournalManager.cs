using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

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
    // Name of the main JSON file to store the global journal.
    private string journalFileName = "AgentJournal.json";
    // Full path to the global journal file.
    private string journalFilePath;
    // In-memory copy of the global journal.
    private AgentJournal agentJournal;

    private void Awake()
    {
        // Save the global journal in the "Survey" folder.
        string directoryPath = Path.Combine(Application.dataPath, "Scripts", "SimManager", "Data", "Survey");
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        journalFilePath = Path.Combine(directoryPath, journalFileName);
        Debug.Log("Global journal will be saved at: " + journalFilePath);
        
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
    /// Saves the current global journal to JSON.
    /// </summary>
    private void SaveJournal()
    {
        string json = JsonUtility.ToJson(agentJournal, true);
        File.WriteAllText(journalFilePath, json);
    }

    /// <summary>
    /// Adds a conversation record to the global journal and saves it.
    /// </summary>
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
    /// Debug utility: prints the entire global journal to the Console.
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

    // ============================================================
    // NEW: Per-Agent Journal Saving Functionality
    // ============================================================
    public void SavePersonalJournalForAgent(string agentName, AgentJournal journal)
    {
        // Define the base folder for personal journals.
        string baseFolder = Path.Combine(Application.dataPath, "Scripts", "SimManager", "Data", "Survey", "personalJournals");
        if (!Directory.Exists(baseFolder))
        {
            Directory.CreateDirectory(baseFolder);
        }

        // Create a subfolder for the agent.
        string agentFolder = Path.Combine(baseFolder, agentName);
        if (!Directory.Exists(agentFolder))
        {
            Directory.CreateDirectory(agentFolder);
        }

        // Build the file path.
        string filePath = Path.Combine(agentFolder, $"{agentName}_Journal.json");

        try
        {
            // Delete the file if it already exists.
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            // Convert the journal to JSON.
            string json = JsonUtility.ToJson(journal, true);
            // Write the new journal to file (this will create a new file).
            File.WriteAllText(filePath, json);
            Debug.Log($"Journal for {agentName} saved at: {filePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save journal for {agentName}: {e.Message}");
        }
    #if UNITY_EDITOR
        AssetDatabase.Refresh();
    #endif
    }

}
