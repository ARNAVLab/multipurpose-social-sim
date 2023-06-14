using UnityEngine;

public class JsonReader
{
    public ActorsInfo GetAgentsInfo(string agentsJson)
    {
        return JsonUtility.FromJson<ActorsInfo>(agentsJson);
    }
}
