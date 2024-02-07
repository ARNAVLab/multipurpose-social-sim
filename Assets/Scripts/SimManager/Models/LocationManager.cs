using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace Anthology.Models
{
    /// <summary>
    /// Provides functionality for checking location-centric conditions.
    /// </summary>
    public static class LocationManager
    {
        /// <summary>
        /// Locations in the simulation stored by name.
        /// </summary>
        public static Dictionary<string, LocationNode> LocationsByName { get; set; } = new();

        /// <summary>
        /// Locations in the simulation stored by (X,Y) position.
        /// </summary>
        public static Dictionary<Vector2, LocationNode> LocationsByPosition { get; set; } = new();

        /// <summary>
        /// Locations in the simulation stored by tags for action selection.
        /// </summary>
        public static Dictionary<string, List<LocationNode>> LocationsByTag { get; set; } = new();

        /// <summary>
        /// The total number of locations in the simulation.
        /// </summary>
        public static int LocationCount { get; set; } = 0;

        /// <summary>
        /// The directed distance matrix for determining the travel distance between
        /// any two locations. To obtain the distance from A to B, index into the
        /// matrix like: [A.ID * LocationCount + B.ID].
        /// </summary>
        public static float[] DistanceMatrix { get; set; } = Array.Empty<float>();

        /// <summary>
        /// Initialize/reset all static location manager variables compute the distance matrix.
        /// </summary>
        /// <param name="path">Path of JSON file to load locations from.</param>
        public static void Init(string path)
        {
            Reset();
            World.ReadWrite.LoadLocationsFromFile(path);
            UpdateDistanceMatrix();
        }

        /// <summary>
        /// Resets all storage of locations in the simulation.
        /// </summary>
        public static void Reset()
        {
            LocationsByName.Clear();
            LocationsByPosition.Clear();
            LocationsByTag.Clear();
            DistanceMatrix = Array.Empty<float>();
            LocationCount = 0;
        }

        /// <summary>
        /// Adds a location accordingly to each location data structure.
        /// </summary>
        /// <param name="node">The location to add to the simulation.</param>
        public static void AddLocation(LocationNode node)
        {
            LocationsByName.Add(node.Name, node);
            LocationsByPosition.Add(new(node.X, node.Y), node);
            node.ID = LocationCount++;

            foreach (string tag in node.Tags)
            {
                if (!LocationsByTag.ContainsKey(tag))
                    LocationsByTag.Add(tag, new());
                LocationsByTag[tag].Add(node);
            }
            foreach (Agent agent in AgentManager.Agents)
            {
                if (agent.CurrentLocation == node.Name)
                    node.AgentsPresent.AddLast(agent.Name);
            }
        }

        /// <summary>
        /// Creates and adds a location to each of the static data structures.
        /// </summary>
        /// <param name="name">Name of the location.</param>
        /// <param name="x">X-coordinate of the location.</param>
        /// <param name="y">Y-coordinate of the location.</param>
        /// <param name="tags">Relevant tags of the location.</param>
        /// <param name="connections">Connections from the location to others.</param>
        public static void AddLocation(string name, float x, float y, IEnumerable<string> tags, Dictionary<string, float> connections)
        {
            List<string> newTags = new();
            newTags.AddRange(tags);
            AddLocation(new() { Name = name, X = x, Y = y, Tags = newTags, Connections = connections });
        }

        /// <summary>
        /// Resets and populates the static distance matrix with all-pairs-shortest-path
        /// via the Floyd-Warshall algorithm.
        /// </summary>
        public static void UpdateDistanceMatrix()
        {
            DistanceMatrix = new float[LocationCount * LocationCount];
            Parallel.For(0, LocationCount, loc1 =>
            {
                for (int loc2 = 0; loc2 < LocationCount; loc2++)
                {
                    if (loc1 == loc2) DistanceMatrix[loc1 * LocationCount + loc2] = 0;
                    else DistanceMatrix[loc1 * LocationCount + loc2] = (float.MaxValue / 2f) - 1f;
                }
            });
            IEnumerable<LocationNode> locationNodes = LocationsByName.Values;
            Parallel.ForEach(locationNodes, loc1 =>
            {
                foreach (KeyValuePair<string, float> con in loc1.Connections)
                {
                    LocationNode loc2 = LocationsByName[con.Key];
                    DistanceMatrix[loc1.ID * LocationCount + loc2.ID] = con.Value;
                }
            });
            for (int k = 0; k < LocationCount; k++)
            {
                Parallel.For(0, LocationCount, i =>
                {
                    for (int j = 0; j < LocationCount; j++)
                    {
                        float d = DistanceMatrix[i * LocationCount + k] + DistanceMatrix[k * LocationCount + j];
                        if (DistanceMatrix[i * LocationCount + j] > DistanceMatrix[i * LocationCount + k] + DistanceMatrix[k * LocationCount + j])
                            DistanceMatrix[i * LocationCount + j] = DistanceMatrix[i * LocationCount + k] + DistanceMatrix[k * LocationCount + j];
                    }
                });
            }
        }

        /// <summary>
        /// Filter all locations to find those locations that satisfy conditions specified in the location requirement.
        /// Returns an enumerable of locations that match the HasAllOf, HasOneOrMOreOf, and HasNoneOf constraints.
        /// </summary>
        /// <param name="requirements">Requirements that locations must satisfy in order to be returned.</param>
        /// <returns>Returns all the locations that satisfied the given requirement, or an empty enumerable if none match.</returns>
        public static IEnumerable<LocationNode> LocationsSatisfyingLocationRequirement(RLocation requirements)
        {
            List<LocationNode> matches = new();
            if (requirements.HasOneOrMoreOf.Count > 0)
            {
                foreach (string tag in requirements.HasOneOrMoreOf)
                {
                    matches.AddRange(LocationsByTag[tag]);
                }
            }
            else
            {
                matches.AddRange(LocationsByName.Values);
            }
            if (requirements.HasAllOf.Count > 0)
            {
                foreach (string tag in requirements.HasAllOf)
                {
                    matches = matches.Intersect(LocationsByTag[tag]).ToList();
                }
            }
            if (requirements.HasNoneOf.Count > 0)
            {
                foreach (string tag in requirements.HasNoneOf)
                {
                    matches = matches.Except(LocationsByTag[tag]).ToList();
                }
            }
            return matches;
        }

        /// <summary>
        /// Filter given locations to find those locations that satisfy conditions specified in the people requirement.
        /// Returns locations that match the MinNumPeople, MaxNumPeople, SpecificPeoplePresent, SpecificPeopleAbsent,
        /// RelationshipsPresent, and RelationshipsAbsent requirements.
        /// </summary>
        /// <param name="locations">The set of locations to filter.</param>
        /// <param name="requirements">Requirements that locations must satisfy to be returned.</param>
        /// <param name="agent">Agent relevant for handling agent requirement(s).</param>
        /// <returns>Returns all the locations that satisfied the given requirement, or an empty enumerable if none match.</returns>
        public static IEnumerable<LocationNode> LocationsSatisfyingPeopleRequirement(IEnumerable<LocationNode> locations, RPeople requirements, string agent = "")
        {
            bool IsLocationValid(LocationNode location)
            {
                if (agent == "" || location.AgentsPresent.Contains(agent))
                {
                    return location.SatisfiesRequirements(requirements);
                }
                else
                {
                    location.AgentsPresent.AddLast(agent);
                    bool valid = location.SatisfiesRequirements(requirements);
                    location.AgentsPresent.RemoveLast();
                    return valid;
                }
            }

            List<LocationNode> matches = new();
            foreach (LocationNode location in locations)
            {
                if (IsLocationValid(location)) matches.Add(location);
            }

            return matches;
        }

        /// <summary>
        /// Finds the nearest location of a given set from a specified location.
        /// </summary>
        /// <param name="from">The source location.</param>
        /// <param name="locations">The locations to filter for the closest.</param>
        /// <returns></returns>
        public static LocationNode FindNearestLocationFrom(LocationNode from, IEnumerable<LocationNode> locations)
        {
            IEnumerator<LocationNode> enumerator = locations.GetEnumerator();
            enumerator.MoveNext();
            LocationNode nearest = enumerator.Current;
            float dist = DistanceMatrix[from.ID * LocationCount + nearest.ID];
            int row = from.ID * LocationCount;
            int i;
            while (enumerator.MoveNext())
            {
                i = row + enumerator.Current.ID;
                if (dist > DistanceMatrix[i])
                {
                    nearest = enumerator.Current;
                    dist = DistanceMatrix[i];
                }
            }

            return nearest;
        }

        public static void GetPathFromTo(LocationNode From, LocationNode To)
        {
            
            return; //Return List of LocationNodes
            //Create random output
        }
    }
}
