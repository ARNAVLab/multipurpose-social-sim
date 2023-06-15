using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Numerics;

namespace Anthology.SimulationManager
{
    /** 
     * The SimManager (or simulation manager) is used to coordinate the activities and data of two 
     * different simulations in order to connect their functionality and offer support for 
     * front-end applications
     */
    public static class SimManager
    {
        /** Collection of NPCs coupled with data only pertinent to connecting simulations and the frontend */
        public static Dictionary<string, NPC> NPCs { get; set; } = new();

        /** Collection of Locations as they exist for use by the frontend and for synchronization with the simulations */
        public static Dictionary<Vector2, Location> Locations { get; set; } = new();

        /** The simulation used for updating NPC actions, locations, and other physical traits */
        public static RealitySim? Reality { get; set; }

        /** The simulation used for updating NPC knowledge, opinions, and beliefs */
        public static KnowledgeSim? Knowledge { get; set; }

        /** The number of iterations run since the initializaztion of the simulation manager */
        public static uint NumIterations { get; set; }

        /** 
         * Initializes the simulation manager using the given file pathname and types of 
         * Reality and Knowledge sims
         * 
         * To use custom Reality or Knowledge Sim implementations, this method should be called and 
         * modified in any user-facing or client programs
         * 
         * Example usage: SimManager.Init("myPath.json", typeof(MyRealitySim), typeof(MyKnowledgeSim)
         */
        public static void Init(string JSONfile, Type reality, Type knowledge)
        {
            if (reality.IsSubclassOf(typeof(RealitySim)))
            {
                Reality = Activator.CreateInstance(reality) as RealitySim;
                if (Reality == null)
                    throw new NullReferenceException("Could not create reality sim");
                Reality.Init(JSONfile);
                Reality.LoadNpcs(NPCs);
                Reality.LoadLocations(Locations);
            }
            else
                throw new InvalidCastException("Failed to recognize reality sim type");
            if (false && knowledge.IsSubclassOf(typeof(KnowledgeSim))) // short circuited to skip step until LyraKS is implemented
            {
                Knowledge = Activator.CreateInstance(knowledge) as KnowledgeSim;
                if (Knowledge == null)
                    throw new NullReferenceException("Could not create knowledge sim");
                Knowledge?.Init(JSONfile);
                Knowledge?.LoadNpcs(NPCs);
                NumIterations = 0;
            }
            else
                // throw new InvalidCastException("Failed to recognize knowledge sim type"); ignored until LyraKS is implemented
                ;
        }

        /**
         * Runs the specified number of steps of both the reality and knowledge simulations,
         * if they exist, and obtains any modifications to NPCs from both sims
         */
        public static void GetIteration(int steps = 1)
        {
            NumIterations += (uint)steps;
            Reality?.Run(steps);
            Knowledge?.Run(steps);
            Debug.WriteLine(string.Format("--- NPC Information for Iteration {0} ---", NumIterations));
            foreach (NPC npc in NPCs.Values)
            {
                Reality?.UpdateNpc(npc);
                Knowledge?.UpdateNpc(npc);
                // Print npc info for now
                Debug.WriteLine(npc);
            }
            Debug.WriteLine("*** End NPC Information ***");
        }

        /**
         * Sends the specified NPC to both the reality and knowledge simulations in order to update 
         * any information on their end.
         * Should be used whenever manual changes are made from a user interface
         */
        public static void PushUpdatedNpc(NPC npc)
        {
            if (npc.Dirty)
            {
                Reality?.PushUpdatedNpc(npc);
                Knowledge?.PushUpdatedNpc(npc);
                npc.Dirty = false;
            }
        }
    }
}
