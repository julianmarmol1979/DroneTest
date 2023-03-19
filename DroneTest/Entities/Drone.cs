namespace DroneTest.Entities {

    /// <summary>
    /// Information of the drone, specially trips for every drone
    /// </summary>
    public class Drone {
        public string Name { get; set; }

        public int Weight { get; set; }

        public List<Trip> Trips { get; set; }

    }
}

