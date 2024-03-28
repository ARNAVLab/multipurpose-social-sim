using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SimManager.SimulationManager
{
    /// <summary>
    /// An Agent/Actor/Individual/Unit/NPC to be maintained by the simulation manager.
    /// Contains data necessary for coordinating behavior across simulations and for
    /// displaying information on the frontend.
    /// </summary>
    public class NPC
  {
        /// <summary>
        /// The name of the NPC.
        /// </summary>
        private string name = string.Empty;

        /// <summary>
        /// Get and sets the name of the NPC.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { Dirty = true; name = value; }
        }

        private string location = string.Empty;
        /// <summary>
        /// The name of the current location of the NPC
        /// </summary>
        public string Location
        {
            get { return location; }
            set { Dirty = true; location = value; }
        }

        /// <summary>
        /// Data representing the knowledge/beliefs/opinions of the NPC.
        /// </summary>
        private Dictionary<string, float> knowledge = new();

        /// <summary>
        /// Get and sets the knowledge dictionary.
        /// </summary>
        public Dictionary<string, float> Knowledge
        {
            get { return knowledge; }
            set { Dirty = true; knowledge = value; }
        }

        // private Dictionary<string, string> relationships = new();

		private List<Relationship> relationships = new();
        public List<Relationship> Relationships
        {
            get { return relationships; }
            set { Dirty = true; relationships = value; }
        }

        /// <summary>
        /// Add a subject to the NPC's knowledge with a given amount.
        /// </summary>
        /// <param name="subject">New subject to add.</param>
        /// <param name="amount">Amount of subject.</param>
        public void SetKnowledgeSubject(string subject, float amount)
        {
            knowledge[subject] = amount;
            Dirty = true;
        }

        /// <summary>
        /// Change an existing knowledge subject by the given amount.
        /// </summary>
        /// <param name="subject">Subject to change amount.</param>
        /// <param name="delta">Amount to add to subject.</param>
        public void ChangeKnowledgeSubject(string subject, float delta)
        {
            knowledge[subject] += delta;
            Dirty = true;
        }

        /// <summary>
        /// Data representing the motivations/statuses of the NPC.
        /// </summary>
        private Dictionary<string, float> motives = new();

        /// <summary>
        /// Get and sets the dictionary of motives.
        /// </summary>
        public Dictionary<string, float> Motives
        {
            get { return motives; }
            set { Dirty = true; motives = value; }
        }

        /// <summary>
        /// Set the motivation to the given amount. 
        /// </summary>
        /// <param name="motivation">The motivation to set.</param>
        /// <param name="amount">Motivation's new amount.</param>
        public void SetMotivationStatus(string motivation, float amount)
        {
            motives[motivation] = amount;
            Dirty = true;
        }

        /// <summary>
        /// Change an existing motive by the given amount.
        /// </summary>
        /// <param name="motivation">The motivation to add to.</param>
        /// <param name="delta">Amount of motivation to add.</param>
        public void ChangeMotivation(string motivation, float delta)
        {
            motives[motivation] += delta;
            Dirty = true;
        }

        /// <summary>
        /// The action currently being performed by the NPC.
        /// </summary>
        private Action currentAction = new();

        /// <summary>
        /// Get and sets the current action of the NPC.
        /// </summary>
        public Action CurrentAction
        {
            get { return currentAction; }
            set { Dirty = true; currentAction = value; }
        }

        /// <summary>
        /// The remaining ticks until the completion of the current action.
        /// </summary>
        private int actionCounter = 0;

        /// <summary>
        /// Get and sets the remaining ticks until the completion of the current action.
        /// </summary>
        public int ActionCounter
        {
            get { return actionCounter; }
            set { Dirty = true; actionCounter = value; }
        }

        /// <summary>
        /// The destination of the NPC if the NPC is travelling.
        /// </summary>
        private string destination = string.Empty;

        /// <summary>
        /// Get and sets the destionation of the NPC if traveling.
        /// </summary>
        public string Destination
        {
            get { return destination; }
            set { Dirty = true; destination = value; }
        }

        /// <summary>
        /// Whether or not this NPC has been modified and needs to have its update pushed.
        /// </summary>
        public bool Dirty { get; set; } = false;

        /// <summary>
        /// Gets a string representation of the NPC, in the following format
        /// 
        ///  Name: {name}
        ///  X: {x}, Y: {y}
        ///  Current Action: {name of action}
        /// </summary>
        /// <returns>String representation of the NPC.</returns>
        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendFormat("Name: {0}, ", Name);
            sb.AppendFormat("Current Location: ", Location);
            sb.AppendFormat("Motives: ", Motives.ToString());
            sb.AppendFormat("Current Action: {0}", CurrentAction.Name);
            sb.AppendFormat("Current Destination: {0}", Destination);
            return sb.ToString();
        }
    }
}
