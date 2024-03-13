using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace SimManager.SimulationManager
{
    /// <summary>
    /// Locations as they exist between the frontend and backend simulations.
    /// Currently these are expected to come exclusively from the RealitySim
    /// and are accessible to the frontend, but future implementations should
    /// support the frontend informing the manager of locations as well.
    /// </summary>
    public class Location
    {
        /// <summary>
        /// The name of the location.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Internal Vector2 type that satisfies Bson construction.
        /// </summary>
        public struct Coords
        {
            /// <summary>
            /// X-component of coordinate. 
            /// </summary>
            public float X { get; set; }
            /// <summary>
            /// Y-component of coordinate.
            /// </summary>
            public float Y { get; set; }

            /// <summary>
            /// Bson version of constructor. 
            /// </summary>
            /// <param name="x">X-component of coordinate.</param>
            /// <param name="y">Y-component of coordinate.</param>
            [BsonConstructor]
            public Coords(float x, float y)
            {
                X = x;
                Y = y;
            }
        }

        /// <summary>
        /// The (X,Y) position of the location.
        /// </summary>
        [BsonId]
        public Coords Coordinates { get; set; } = new();

        /// <summary>
        /// Arbitrary set of tags associated with the location.
        /// </summary>
        public HashSet<string> Tags { get; set; } = new();

        /// <summary>
        /// Directly pathable connections between locations and their distances
        /// Effectively out-edges in graph theory
        /// </summary>
        public Dictionary<string, float> Connections { get; set; } = new();

        /// <summary>
        /// The agents located at this location.
        /// </summary>
        public HashSet<string> AgentsPresent { get; set; } = new();
    }
}
