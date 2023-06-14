using System.Collections.Generic;

namespace Anthology.SimulationManager
{
    /**
     * This class should be inherited from and implemented to use as a wrapper
     * for connecting a knowledge simulation into the SimManager.
     * An example implementation can be found in LyraKS.cs 
     */
    public abstract class KnowledgeSim
    {
        /** Used to initialize the knowledge sim, optionally loading data from the given file path. */
        public abstract void Init(string pathFile = "");

        /** Used to populate the SimManager's collection of NPCs from the knowledge sim */
        public abstract void LoadNpcs(Dictionary<string, NPC> npcs);

        /** Updates the given NPC to math the knowledge sim's version */ 
        public abstract void UpdateNpc(NPC npc);

        /** Updates the knowledge sim's version of the given NPC to math the SimManager's */
        public abstract void PushUpdatedNpc(NPC npc);

        /** Runs the knowledge sim as many times as specified (or once, if unspecified) */
        public abstract void Run(int steps = 1);
    }
}
