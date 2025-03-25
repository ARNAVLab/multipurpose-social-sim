using System;

[Serializable]
public class ActorInfo

{
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
        public float disabilityMedicalNeeds;
        public float inaccessible;
        public float nonNativeSpeaker;
        public float senior;
        public float financial;
    }

    public string name;
    public Motive motive;
    public string currentLocation;
    public string destination;
    public int occupiedCounter;
    public string currentAction;
    public Relationship[] relationships;
}
