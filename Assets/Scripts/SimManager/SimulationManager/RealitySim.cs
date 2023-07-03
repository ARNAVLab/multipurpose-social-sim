using System.Collections.Generic;
using System.Numerics;

namespace Anthology.SimulationManager
{
    /** 
     * This class should be inherited from and implemented to use as a wrapper
     * for connecting a reality simulation into the SimManager.
     * An example implementation can be found in AnthologyRS.cs
     */
    public abstract class RealitySim
    {
        /** Used to initialize the reality sim, optionally loading data from the given file path */
        public abstract void Init(string pathFile = "");

        /** Used to populate the SimManager's collection of NPCs from the reality sim */
        public abstract void LoadNpcs(Dictionary<string, NPC> npcs);

        /** Used to populate the SimManager's collection of Locations from the relaity sim */
        public abstract void LoadLocations(Dictionary<Location.Coords, Location> locations);

        /** Updates the given NPC to match the reality sim's version */
        public abstract void UpdateNpc(NPC npc);

        /** Updates the reality sim's version of the given NPC to match the SimManager's */
        public abstract void PushUpdatedNpc(NPC npc);

        /** Runs the reality sim as many times as specified (or once, if unspecified) */
        public abstract void Run(int steps = 1);
    }
}
