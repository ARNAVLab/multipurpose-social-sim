using SimManager.HistoryManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace SimManager.SimulationManager
{
    /// <summary>
    /// The SimManager (or simulation manager) is used to coordinate the activities and data of two 
    /// different simulations in order to connect their functionality and offer support for 
    /// front-end applications.
    /// </summary>
    public static class SimEngine
    {
        /// <summary>
        /// Name of log to store history
        /// </summary>
        private const string LOG_PATH = "NPC History";

        /// <summary>
        /// Collection of NPCs coupled with data only pertinent to connecting simulations and the frontend.
        /// </summary>
        public static Dictionary<string, NPC> NPCs { get; set; } = new();

        /// <summary>
        /// Collection of Locations as they exist for use by the frontend and for synchronization with the simulations.
        /// </summary>
        public static Dictionary<string, Location> Locations { get; set; } = new();

        /// <summary>
        /// The simulation used for updating NPC actions, locations, and other physical traits.
        /// </summary>
        public static RealitySim Reality { get; set; }

        /// <summary>
        /// The simulation used for updating NPC knowledge, opinions, and beliefs.
        /// </summary>
        public static KnowledgeSim Knowledge { get; set; }

        /// <summary>
        /// The logger used for keeping track of the simulation's history and saving and loading states.
        /// </summary>
        public static HistoryLogger History { get; set; }

        /// <summary>
        /// The number of iterations run since the initializaztion of the simulation manager.
        /// </summary>
        public static uint NumIterations { get; set; }

        /// <summary>
        /// Initializes the simulation manager using the given file pathname and types of 
        /// Reality and Knowledge sims
        /// 
        /// To use custom Reality or Knowledge Sim implementations, this method should be called and 
        /// modified in any user-facing or client programs
        /// 
        /// Example usage: SimManager.Init("myPath.json", typeof(MyRealitySim), typeof(MyKnowledgeSim)
        /// </summary>
        /// <param name="JSONfile">Path of JSON file containing sim data.</param>
        /// <param name="reality">The reailty sim type to use.</param>
        /// <param name="knowledge">The knowledge sim type to use.</param>
        /// <param name="history">The history manager type to use.</param>
        public static void Init(string JSONfile, Type reality, Type knowledge, Type history)
        {
            
            if (reality.IsSubclassOf(typeof(RealitySim)))
            {
                Reality = Activator.CreateInstance(reality) as RealitySim;
                if (Reality == null)
                    throw new NullReferenceException("Could not create reality sim");
                Reality.Init(JSONfile);
				Reality.LoadLocations(Locations);
                Reality.LoadNpcs(NPCs);
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
            }
            //else
                // throw new InvalidCastException("Failed to recognize knowledge sim type"); ignored until LyraKS is implemented
            if (history.IsSubclassOf(typeof(HistoryLogger)))
            {
                History = Activator.CreateInstance(history) as HistoryLogger;
                if (History == null)
                    throw new NullReferenceException("Could not create history logger");
                History.ClearLog(LOG_PATH);
            }
            else 
                throw new InvalidCastException("Failed to recognize history logger");
        }

        /// <summary>
        /// Runs the specified number of steps of both the reality and knowledge simulations,
        /// if they exist, and obtains any modifications to NPCs from both sims.
        /// </summary>
        /// <param name="steps">Number of steps to advance the simulation.</param>
        public static void GetIteration(int steps = 1)
        {
            for (int i = 0; i < steps; i++)
            {
                NumIterations++;
                Reality?.Run();
                foreach (NPC npc in NPCs.Values)
                {
                    Reality?.UpdateNpc(npc);
                    Knowledge?.UpdateNpc(npc);
                }
                History?.LogNpcStates(LOG_PATH);
            }
        }

        /// <summary>
        /// Sends the specified NPC to both the reality and knowledge simulations in order to update 
        /// any information on their end.
        /// Should be used whenever manual changes are made from a user interface.
        /// </summary>
        /// <param name="npc">The NPC to be updated on the reality and knowledge sims.</param>
        public static void PushUpdatedNpc(NPC npc)
        {
            if (npc.Dirty)
            {
                Reality?.PushUpdatedNpc(npc);
                Knowledge?.PushUpdatedNpc(npc);
                npc.Dirty = false;
            }
        }

        /// <summary>
        /// Saves the current state in the history manager.
        /// </summary>
        /// <param name="stateName">The name of the current state to save.</param>
        public static void SaveState(string stateName = "")
        {
            History?.SaveState(stateName);
        }

        /// <summary>
        /// Loads a state via the history manager and loads all NPCs and locations from
        /// that state.
        /// </summary>
        /// <param name="stateName">Name of the state to load.</param>
        public static void LoadState(string stateName)
        {
            SimState state = History?.LoadState(stateName);
            HashSet<Location> locations = state?.Locations;
            if (locations != null)
            {
                foreach (Location newLoc in locations)
                    Locations[newLoc.Name] = newLoc;
                Reality?.LoadLocations(Locations);
            }
            
            HashSet<NPC> npcs = state.NPCs;

            if (npcs != null)
            {
                foreach (NPC newNPC in npcs)
                {
                    NPCs[newNPC.Name] = newNPC;
                    Reality?.PushUpdatedNpc(newNPC);
                }
            }
        }

        /// <summary>
        /// Exports History Logs WIP
        /// that state.
        /// </summary>
        public static void ExportLogs()
        {
            History.ExportCollection();
        }

        public static string GetLog(string actorName)
        {
            var npcLogCursor = History.GetActorJson(actorName);
            return History.JsonToNPCLog(npcLogCursor, actorName);
        }
    }
}
