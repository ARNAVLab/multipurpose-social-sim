using UnityEngine;

public class JsonReader
{
    public UAgentsInfo GetAgentsInfo(string agentsJson)
    {
        return JsonUtility.FromJson<UAgentsInfo>(agentsJson);
    }
}
