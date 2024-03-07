using System.Collections.Generic;

namespace SimManager.SimulationManager
{
    /// <summary>
    /// This class should be inherited from and implemented to use as a wrapper
    /// for connecting a knowledge simulation into the SimManager.
    /// An example implementation can be found in LyraKS.cs.
    /// </summary>
    public abstract class KnowledgeSim
    {
        /// <summary>
        /// Used to initialize the knowledge sim, optionally loading data from the given file path.
        /// </summary>
        /// <param name="pathFile">path of the data file to init from.</param>
        public abstract void Init(string pathFile = "");

        /// <summary>
        /// Used to populate the SimManager's collection of NPCs from the knowledge sim.
        /// </summary>
        /// <param name="npcs">SimManager's dictionary of NPCs to populate.</param>
        public abstract void LoadNpcs(Dictionary<string, NPC> npcs);

        /// <summary>
        /// Updates the given NPC to match the knowledge sim's version.
        /// </summary>
        /// <param name="npc">The NPC to update on the SimManager.</param>
        public abstract void UpdateNpc(NPC npc);

        /// <summary>
        /// Updates the knowledge sim's version of the given NPC to match the SimManager's.
        /// </summary>
        /// <param name="npc">The NPC to update on the knowledge sim. </param>
        public abstract void PushUpdatedNpc(NPC npc);

        /// <summary>
        /// Runs the knowledge sim as many times as specified (or once, if unspecified).
        /// </summary>
        /// <param name="steps">Number of steps to advance the simulation.</param>
        public abstract void Run(int steps = 1);
    }
}
