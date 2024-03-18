using System.Collections.Generic;

namespace SimManager.SimulationManager
{
    /// <summary>
    /// This class should be inherited from and implemented to use as a wrapper
    /// for connecting a reality simulation into the SimManager.
    /// An example implementation can be found in AnthologyRS.cs.
    /// </summary>
    public abstract class RealitySim
    {
        /// <summary>
        /// Used to initialize the reality sim, optionally loading data from the given file path
        /// </summary>
        /// <param name="pathFile">The path of the JSON file to load from.</param>
        public abstract void Init(string pathFile = "");

        /// <summary>
        /// Used to populate the SimManager's collection of NPCs from the reality sim.
        /// </summary>
        /// <param name="npcs">The SimManager's dictionary to populate.</param>
        public abstract void LoadNpcs(Dictionary<string, NPC> npcs);

        /// <summary>
        /// Used to populate the SimManager's collection of Locations from the relaity sim.
        /// </summary>
        /// <param name="locations">The SimManager's dictionary to populate.</param>
        public abstract void LoadLocations(Dictionary<string, Location> locations);

        /// <summary>
        /// Used to update the reality sim's location models to match the SimManager's collection of Locations.
        /// </summary>
        public abstract void PushLocations();

        /// <summary>
        /// Updates the given NPC to match the reality sim's version.
        /// </summary>
        /// <param name="npc">The SimManager's NPC to update.</param>
        public abstract void UpdateNpc(NPC npc);

        /// <summary>
        /// Updates the reality sim's version of the given NPC to match the SimManager's.
        /// </summary>
        /// <param name="npc">The reality sim's NPC to update.</param>
        public abstract void PushUpdatedNpc(NPC npc);

        /// <summary>
        /// Runs the reality sim as many times as specified (or once, if unspecified).
        /// </summary>
        /// <param name="steps">Number of steps to advance the reality sim.</param>
        public abstract void Run(int steps = 1);
    }
}
