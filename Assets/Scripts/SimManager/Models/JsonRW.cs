namespace Anthology.Models 
{
    /** Abstracts away JSON reading for compatibility with legacy JSON readers */
    public abstract class JsonRW 
    {
        /** Parse pathnames of Actions, Agents, and Locations jsons */
        public abstract void InitWorldFromPaths(string pathsFile);

        /** Load actions from the given file path directly into the ActionManager */
        public abstract void LoadActionsFromFile(string path);

        /** Returns a JSON string representing the list of all actions in the ActionManager */
        public abstract string SerializeAllActions();

        /** Load agents from the given file path directly into the AgentManager */
        public abstract void LoadAgentsFromFile(string path);

        /** Returns a JSON string representing the list of all agents in the AgentManager */
        public abstract string SerializeAllAgents();

        /** Load locations from the given file path directly into the LocationManager */
        public abstract void LoadLocationsFromFile(string path);

        /** Returns a JSON string representing the list of all locations in the LocationManager */
        public abstract string SerializeAllLocations();
    }
}