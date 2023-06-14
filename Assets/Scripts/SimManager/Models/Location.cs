using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Anthology.Models
{
    public class SimLocation
    {
        /** optional name of the location. Eg. Restaurant, Home, Movie Theatre, etc */
        public string Name { get; set; } = string.Empty;

        /** x-coordinate of the location */
        public int X { get; set; }

        /** y-coordinate of the location */
        public int Y { get; set; }

        /** optional set of tags associated with the location. Eg. Restaurant could have 'food', 'delivery' as tags */
        public HashSet<string> Tags { get; set; } = new();

        /** set of agents at the location */
        [JsonIgnore]
        public HashSet<string> AgentsPresent { get; set; } = new HashSet<string>();

        /** returns true if the specified agent is at this location */
        public bool IsAgentHere(Agent npc)
        {
            return AgentsPresent.Contains(npc.Name);
        }

        /** checks if this location satisfies all of the passed location requirements */
        public bool SatisfiesRequirements(RLocation reqs)
        {
            return HasAllOf(reqs.HasAllOf) &&
                   HasOneOrMoreOf(reqs.HasOneOrMoreOf) &&
                   HasNoneOf(reqs.HasNoneOf);
        }

        /** checks if this location satisfies all of the passed people requirements */
        public bool SatisfiesRequirements(RPeople reqs)
        {
            return HasMinNumPeople(reqs.MinNumPeople) &&
                   HasNotMaxNumPeople(reqs.MaxNumPeople) &&
                   SpecificPeoplePresent(reqs.SpecificPeoplePresent) &&
                   SpecificPeopleAbsent(reqs.SpecificPeopleAbsent) &&
                   RelationshipsPresent(reqs.RelationshipsPresent);
        }

        /** checks if this location satisfies the passed HasAllOf requirement */
        private bool HasAllOf(HashSet<string> hasAllOf)
        {
            return hasAllOf.IsSubsetOf(Tags);
        }

        /** checks if this location satisfies the HasOneOrMOreOf requirement */
        private bool HasOneOrMoreOf(HashSet<string> hasOneOrMoreOf)
        {
            if (hasOneOrMoreOf.Count == 0) { return true; }
            return hasOneOrMoreOf.Overlaps(Tags);
        }

        /** checks if this location satisfies the HasNoneOf requirement */
        private bool HasNoneOf(HashSet<string> hasNoneOf)
        {
            if (hasNoneOf.Count == 0) { return true; }
            return !hasNoneOf.Overlaps(Tags);
        }

        /** checks if this location satisfies the MinNumPeople requirement */
        private bool HasMinNumPeople(short minNumPeople)
        {
            return minNumPeople <= AgentsPresent.Count;
        }

        /** checks if this location satifies the MaxNumPeople requirement */
        private bool HasNotMaxNumPeople(short maxNumPeople)
        {
            return maxNumPeople >= AgentsPresent.Count;
        }

        /** checks if this location satifies the SpecificPeoplePresent requirement */
        private bool SpecificPeoplePresent(HashSet<string> specificPeoplePresent)
        {
            if (specificPeoplePresent.Count == 0) { return true; }
            return specificPeoplePresent.IsSubsetOf(AgentsPresent);
        }

        /** checks if this location satisfies the SpecificPeopleAbsent requirement */
        private bool SpecificPeopleAbsent(HashSet<string> specificPeopleAbsent)
        {
            if (specificPeopleAbsent.Count == 0) { return true; }
            return !specificPeopleAbsent.Overlaps(AgentsPresent);
        }

        /** checks if this location satifies the RelationshipsPresent requirement */
        private bool RelationshipsPresent(HashSet<string> relationshipsPresent)
        {
            if (relationshipsPresent.Count == 0) { return true; }
            HashSet<string> relationshipsHere = new();
            foreach (string name in AgentsPresent)
            {
                HashSet<Relationship> ar = AgentManager.GetAgentByName(name).Relationships;
                foreach (Relationship r in ar)
                {
                    if (AgentsPresent.Contains(r.With))
                    {
                        relationshipsHere.Add(r.Type);
                    }
                }
            }
            return relationshipsPresent.IsSubsetOf(relationshipsHere);
        }
    }
}
