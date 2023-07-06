using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Anthology.SimulationManager
{
    /** 
     * Actions are performed by NPCs and are informative for the frontend only.
     * SimManager's actions have no functionality and will not be carried out, although
     * for event-based knowledge or reality sims, passing actions between may be helpful
     * for facilitating event triggers.
     */
    public class Action
    {
        /** The name of the action */
        public string Name { get; set; } = string.Empty;

        /** Any additional actors involved in this action with its owning NPC */
        public string[]? Coactors { get; set; }

        /** Any requirements associated with this action */
        public string[]? Requirements { get; set; }

        /** Any additional tags or information about this action */
        public string[]? Tags { get; set; }
    }
}
