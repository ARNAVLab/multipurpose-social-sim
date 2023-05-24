using UnityEngine;

public class JsonReader
{
    public AgentsInfo GetAgentsInfo(string agentsJson)
    {
        return JsonUtility.FromJson<AgentsInfo>(agentsJson);
    }
}
