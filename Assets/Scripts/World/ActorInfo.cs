using System;

[Serializable]
public struct ActorsInfo
{
    public ActorInfo[] actors;
}

[Serializable]
public class ActorInfo

{
    [Serializable]
    public struct Location
    {
        public float xPos;
        public float yPos;
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
        public float accomplishment;
        public float social;
        public float physical;
        public float emotional;
        public float financial;
    }

    public string name;
    public Motive motive;
    public Location currentLocation;
    public int occupiedCounter;
    public string currentAction;
    public Location destination;
    public Relationship[] relationships;
}
