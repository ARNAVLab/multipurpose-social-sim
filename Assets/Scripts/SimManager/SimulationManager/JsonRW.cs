namespace SimManager.Models 
{
    /// <summary>
    /// Abstracts away JSON reading for compatibility with legacy JSON readers.
    /// </summary>
    public abstract class JsonRW 
    {
        /// <summary>
        /// Parse pathnames of Actions, Agents, and Locations jsons.
        /// </summary>
        /// <param name="pathsFile">Path of JSON file containing other paths to relevant JSON files.</param>
        public abstract void InitWorldFromPaths(string pathsFile);

        /// <summary>
        /// Loads actions from the given file path directly into the ActionManager.
        /// </summary>
        /// <param name="path">Path to actions JSON file.</param>
        public abstract void LoadActionsFromFile(string path);

        /// <summary>
        /// Returns a JSON string representing the list of all actions in the ActionManager.
        /// </summary>
        /// <returns>String representation of serialized list of actions.</returns>
        public abstract string SerializeAllActions();

        /// <summary>
        /// Loads agents from the given file path directly into the AgentManager.
        /// </summary>
        /// <param name="path">Path of agents JSON file.</param>
        public abstract void LoadAgentsFromFile(string path);

        /// <summary>
        /// Returns a JSON string representing the list of all agents in the AgentManager.
        /// </summary>
        /// <returns>String representation of all serialized agents.</returns>
        public abstract string SerializeAllAgents();

        /// <summary>
        /// Loads locations from the given file path directly into the LocationManager.
        /// </summary>
        /// <param name="path">Path of locations JSON file.</param>
        public abstract void LoadLocationsFromFile(string path);

        /// <summary>
        /// Returns a JSON string representing the list of all locations in the LocationManager.
        /// </summary>
        /// <returns>String representation of all serialized locations.</returns>
        public abstract string SerializeAllLocations();
    }
}