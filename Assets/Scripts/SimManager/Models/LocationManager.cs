using System;
using System.Collections.Generic;
using System.Linq;

namespace Anthology.Models
{
    /** Provides functionality for checking location-centric conditions */
    public static class LocationManager
    {
        /** Locations in the simulation as a set for set operations */
        public static HashSet<SimLocation> LocationSet { get; set; } = new HashSet<SimLocation>();

        /** Locations in the simulation as a grid for coordinate access */
        public static Dictionary<int, Dictionary<int, SimLocation>> LocationGrid { get; set; } = new Dictionary<int, Dictionary<int, SimLocation>>();

        /** Initialize/reset all static location manager variables and fill an empty N x N grid */
        public static void Init(int n, string path)
        {
            LocationSet.Clear();
            LocationGrid.Clear();
            for (int i = 0; i < n; i++)
            {
                LocationGrid[i] = new Dictionary<int, SimLocation>();
                for (int k = 0; k < n; k++)
                {
                    LocationGrid[i][k] = new SimLocation();
                }
            }
            World.ReadWrite.LoadLocationsFromFile(path);
        }

        /** Add a location to both the location set and the location grid */
        public static void AddLocation(SimLocation location)
        {
            foreach (Agent a in AgentManager.Agents)
            {
                if (a.XLocation == location.X && a.YLocation == location.Y)
                {
                    location.AgentsPresent.Add(a.Name);
                }
            }
            LocationSet.Add(location);
            int max = Math.Max(location.X, location.Y);
            if (max >= UI.GridSize)
            {
                for (int i = UI.GridSize; i <= max; i++)
                {
                    LocationGrid.Add(i, new());
                    for (int k = 0; k <= max; k++)
                    {
                        LocationGrid[i].Add(k, new());
                    }
                }
                for (int i = 0; i < UI.GridSize; i++)
                {
                    for (int k = UI.GridSize; k <= max; k++)
                    {
                        LocationGrid[i].Add(k, new());
                    }
                }
                UI.GridSize = max + 1;
            }
            LocationGrid[location.X][location.Y] = location;
        }

        /** Create and add a location to both the location set and the location grid */
        public static void AddLocation(string name, int x, int y, IEnumerable<string> tags)
        {
            HashSet<string> newTags = new();
            newTags.UnionWith(tags);
            AddLocation(new() { Name = name, X = x, Y = y, Tags = newTags });
        }

        /** Finds the location with the matching name */
        public static SimLocation GetSimLocationByName(string name)
        {
            bool IsNameMatch(SimLocation location)
            {
                return location.Name == name;
            }

            SimLocation location = LocationSet.First(IsNameMatch);
            return location;
        }

        /** Gets the set of all named locations in the square area defined by the given center coordinates and radius */
        public static HashSet<SimLocation> GetSimLocationsByArea(int centerX, int centerY, int radius)
        {
            HashSet<SimLocation> areaSet = new();
            int left = centerX - radius;
            int right = centerX + radius;

            int ya = centerY;
            int yb = centerY;
            SimLocation loc;

            // Right half and center vertical
            for (int x = right; x >= centerX; x--)
            {
                if (x < UI.GridSize)
                {
                    for (int y = yb; y <= ya; y++)
                    {
                        if (y >= 0 && y < UI.GridSize)
                        {
                            loc = LocationGrid[x][y];
                            if (loc.Name != string.Empty)
                                areaSet.Add(loc);
                        }
                    }
                }
                ya++;
                yb--;
            }
            // Left half, skip center vertical
            for (int x = left; x < centerX; x++)
            {
                if (x >= 0)
                {
                    for (int y = yb; y <= ya; y++)
                    {
                        if (y >= 0 && y < UI.GridSize)
                        {
                            loc = LocationGrid[x][y];
                            if (loc.Name != string.Empty)
                                areaSet.Add(loc);
                        }
                    }
                }
                ya--;
                yb++;
            }
            return areaSet;
        }

        /** Gets the set of all named locations in the square (not the area) defined by the given center coordinates and radius */
        public static HashSet<SimLocation> GetSimLocationsByRange(int centerX, int centerY, int dist)
        {
            HashSet<SimLocation> rangeSet = new();

            int top = centerY + dist;
            int bot = centerY - dist;
            int left = centerX - dist;
            int right = centerX + dist;

            int ya = top;
            int yb = bot;
            SimLocation loc;

            // Right half, top and bottom vertices
            for (int x = centerX; x <= right; x++)
            {
                if (x < UI.GridSize)
                {
                    if (ya < UI.GridSize)
                    {
                        loc = LocationManager.LocationGrid[x][ya];
                        if (loc.Name != string.Empty)
                            rangeSet.Add(loc);
                    }
                    if (yb >= 0 && yb != ya)
                    {
                        loc = LocationManager.LocationGrid[x][yb];
                        if (loc.Name != string.Empty)
                            rangeSet.Add(loc);
                    }
                }
                ya--;
                yb++;
            }
            ya = top - 1;
            yb = bot + 1;

            // Left half, skip top and bottom vertices
            for (int x = centerX - 1; x >= left; x--)
            {
                if (x >= 0)
                {
                    if (ya < UI.GridSize)
                    {
                        loc = LocationManager.LocationGrid[x][ya];
                        if (loc.Name != string.Empty)
                            rangeSet.Add(loc);
                    }
                    if (yb >= 0 && yb != ya)
                    {
                        loc = LocationManager.LocationGrid[x][yb];
                        if (loc.Name != string.Empty)
                            rangeSet.Add(loc);
                    }
                }
                ya--;
                yb++;
            }
            return rangeSet;
        }

        /** 
         * Filter given set of locations to find those locations that satisfy conditions specified in the location requirement
         * Returns a set of locations that match the HasAllOf, HasOneOrMOreOf, and HasNoneOf constraints
         * Returns all the locations that satisfied the given requirement, or an empty set is none match.
         */
        public static HashSet<SimLocation> LocationsSatisfyingLocationRequirement(HashSet<SimLocation> locations, RLocation requirements, string agent = "")
        {
            bool IsLocationInvalid(SimLocation location)
            {   
                if (agent == "" || location.AgentsPresent.Contains(agent))
                {
                    return !location.SatisfiesRequirements(requirements);
                }
                else
                {
                    location.AgentsPresent.Add(agent);
                    bool invalid = !location.SatisfiesRequirements(requirements);
                    location.AgentsPresent.Remove(agent);
                    return invalid;
                }
            }

            HashSet<SimLocation> satisfactoryLocations = new();
            satisfactoryLocations.UnionWith(locations);
            satisfactoryLocations.RemoveWhere(IsLocationInvalid);

            return satisfactoryLocations;
        }

        /**
         * Filter given set of locations to find those locations that satisfy conditions specified in the people requirement
         * Returns a set of locations that match the MinNumPeople, MaxNumPeople, SpecificPeoplePresent, SpecificPeopleAbsent,
         * RelationshipsPresent, and RelationshipsAbsent requirements
         * Returns all the locations that satisfied the given requirement, or an empty set is none match.
         */
        public static HashSet<SimLocation> LocationsSatisfyingPeopleRequirement(HashSet<SimLocation> locations, RPeople requirements, string agent = "")
        {
            bool IsLocationInvalid(SimLocation location)
            {
                if (agent == "" || location.AgentsPresent.Contains(agent))
                {
                    return !location.SatisfiesRequirements(requirements);
                }
                else
                {
                    location.AgentsPresent.Add(agent);
                    bool invalid = !location.SatisfiesRequirements(requirements);
                    location.AgentsPresent.Remove(agent);
                    return invalid;
                }
            }

            HashSet<SimLocation> satisfactoryLocations = new();
            satisfactoryLocations.UnionWith(locations);
            satisfactoryLocations.RemoveWhere(IsLocationInvalid);

            return satisfactoryLocations;
        }

        /** Returns the SimLocation at the given (X,Y) coordinates, or null if one does not exist */
        public static SimLocation? FindSimLocationAt(HashSet<SimLocation> locations, float x, float y)
        {
            foreach (SimLocation loc in locations)
            {
                if (loc.X == x && loc.Y == y)
                {
                    return loc;
                }
            }
            return null;
        }

        /** Returns the SimLocation nearest the given SimLocation, or null if one does not exist */
        public static SimLocation? FindNearestLocationFrom(HashSet<SimLocation> locations, SimLocation from)
        {
            return FindNearestLocationXY(locations, from.X, from.Y);
        }

        /** Returns the SimLocation nearest the given Agent, or null if one does not exist */
        public static SimLocation? FindNearestLocationFrom(HashSet<SimLocation> locations, Agent from)
        {
            SimLocation locFrom = LocationGrid[from.XLocation][from.YLocation];
            return FindNearestLocationFrom(locations, locFrom);
        }


        /** Find the manhattan distance between two locations */
        public static int FindManhattanDistance(SimLocation a, SimLocation b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        /** Find the manhattan distance between a location and specified (X,Y) coordinates */
        public static int FindManhattanDistance(SimLocation loc, int x, int y)
        {
            return Math.Abs(loc.X - x) + Math.Abs(loc.Y - y);
        }

        /** Helper function that finds the location nearest to the given (X,Y) coordinate */
        private static SimLocation? FindNearestLocationXY(HashSet<SimLocation> locations, int x, int y)
        {
            HashSet<SimLocation> locationsToCheck = new();
            locationsToCheck.UnionWith(locations);

            if (locationsToCheck.Count == 0) return null;

            HashSet<SimLocation> closestSet = new();
            int closestDist = int.MaxValue;

            foreach (SimLocation loc in locationsToCheck)
            {
                int dist = FindManhattanDistance(loc, x, y);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestSet.Clear();
                    closestSet.Add(loc);
                }
                else if (dist == closestDist)
                {
                    closestSet.Add(loc);
                }
            }

            Random r = new();
            int idx = r.Next(0, closestSet.Count);
            return closestSet.ElementAt(idx);
        }
    }
}
