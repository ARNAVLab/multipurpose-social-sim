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

        /** The (X,Y) position of the location */
        public Vector2 Coordinates { get; set; }

        /** Arbitrary set of tags associated with the location */
        public HashSet<string> Tags { get; set; } = new();
    }
}
