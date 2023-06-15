using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Anthology.SimulationManager
{
    /**
     * An Agent/Actor/Individual/Unit/NPC to be maintained by the simulation manager
     * Contains data necessary for coordinating behavior across simulations and for
     * displaying information on the frontend
     */
    public class NPC
    {
        /** The name of the NPC */
        private string name = string.Empty;
        public string Name
        {
            get { return name; }
            set { Dirty = true; name = value; }
        }

        /** The (X,Y) coordinate location of the NPC */
        private Vector2 coordinates;
        public ref Vector2 Coordinates
        {
            get { Dirty = true; return ref coordinates; }
        }

        /** Data representing the knowledge/beliefs/opinions of the NPC */
        private Dictionary<string, float> knowledge = new();
        public Dictionary<string, float> Knowledge
        {
            get { return knowledge; }
            set { Dirty = true; knowledge = value; }
        }

        /** Data representing the motivations/statuses of the NPC */
        private Dictionary<string, float> motives = new();
        public Dictionary<string, float> Motives
        {
            get { return motives; }
            set { Dirty = true; motives = value; }
        }

        /** The action currently being performed by the NPC */
        private Action currentAction = new();
        public Action CurrentAction
        {
            get { return currentAction; }
            set { Dirty = true; currentAction = value; }
        }

        /** The remaining ticks until the completion of the current action */
        private int actionCounter = 0;
        public int ActionCounter
        {
            get { return actionCounter; }
            set { Dirty = true; actionCounter = value; }
        }

        /** The destination of the NPC if the NPC is travelling */
        private string destination = string.Empty;
        public string Destination
        {
            get { return destination; }
            set { Dirty = true; destination = value; }
        }

        /** Whether or not this NPC has been modified and needs to have its update pushed */
        public bool Dirty { get; set; } = false;

        /**
         * Gets a string representation of the NPC, in the following format:
         * 
         * "Name: {name}
         *  X: {x}, Y: {y}
         *  Current Action: {name of action}"
         */
        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendFormat("Name: {0}, ", Name);
            sb.AppendFormat("X: {0}, Y: {1}, ", Coordinates.X, Coordinates.Y);
            sb.AppendFormat("Motives: ", Motives.ToString());
            sb.AppendFormat("Current Action: {0}", CurrentAction.Name);
            sb.AppendFormat("Current Destination: {0}", Destination);
            return sb.ToString();
        }
    }
}
