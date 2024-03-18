using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SimManager.SimulationManager
{
    /// <summary>
    /// Actions are performed by NPCs and are informative for the frontend only.
    /// SimManager's actions have no functionality and will not be carried out, although
    /// for event-based knowledge or reality sims, passing actions between may be helpful
    /// for facilitating event triggers.
    /// </summary>
    public class Action
    {
        /// <summary>
        /// The name of the action.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Any additional actors involved in this action with its owning NPC.
        /// </summary>
        public string[] Coactors { get; set; }

        /// <summary>
        /// Any requirements associated with this action.
        /// </summary>
        public string[] Requirements { get; set; }

        /// <summary>
        /// Any additional tags or information about this action.
        /// </summary>
        public string[] Tags { get; set; }
    }
}
