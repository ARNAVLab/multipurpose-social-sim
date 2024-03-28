using System.Collections.Generic;
using Anthology.Models;
using AnthologyRelationship = Anthology.Models.Relationship;


namespace SimManager.SimulationManager
{
    /// <summary>
    /// Relationships are composed by agents, so the owning agent will always be the source of the relationship,
    /// eg. an agent that has the 'brother' relationship with Norma is Norma's brother.
    /// </summary>
    public class Relationship
    {
		public static explicit operator Relationship(AnthologyRelationship oldRel)
		{
			return new Relationship{
				Type = oldRel.Type,
				With = oldRel.With,
				Valence = oldRel.Valence
			};
		}
        /// <summary>
        /// The type of relationship, eg. 'student' or 'teacher'.
        /// </summary>
		public string Type { get; set; } = string.Empty;

        /// <summary>
        /// The agent that this relationship is with.
        /// </summary>
		public string With { get; set; } = string.Empty;

        /// <summary>
        /// How strong the relationship is.
        /// </summary>
		public float Valence { get; set; }
    }

}