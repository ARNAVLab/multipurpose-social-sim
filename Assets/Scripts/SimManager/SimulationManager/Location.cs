using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Numerics;

namespace Anthology.SimulationManager
{
    /**
     * Locations as they exist between the frontend and backend simulations.
     * Currently these are expected to come exclusively from the RealitySim
     * and are accessible to the frontend, but future implementations should
     * support the frontend informing the manager of locations as well
     */
    public class Location
    {
        /** The name of the location */
        public string Name { get; set; } = string.Empty;

        public struct Coords
        {
            public int X { get; set; }
            public int Y { get; set; }

            [BsonConstructor]
            public Coords(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        [BsonId]
        /** The (X,Y) position of the location */
        public Coords Coordinates { get; set; } = new();

        /** Arbitrary set of tags associated with the location */
        public HashSet<string> Tags { get; set; } = new();
    }
}
