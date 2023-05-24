using System;

[Serializable]
public struct AgentsInfo
{
    public AgentInfo[] agents;
}

[Serializable]
public class AgentInfo
{
    [Serializable]
    public struct Location
    {
        public int xPos;
        public int yPos;
    }
    [Serializable]
    public struct Relationship
    {
        public string type;
        public string with;
        public int valence;
    }
    [Serializable]
    public struct Motive
    {
        public int accommplishment;
        public int social;
        public int physical;
        public int emotional;
        public int financial;
    }

    public string name;
    public Motive motive;
    public Location currentLocation;
    public int occupiedCounter;
    public string currentAction;
    public Location destination;
    public Relationship[] relationships;
}
