using DroneTest.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DroneTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DroneController : ControllerBase
    {
        /// <summary>
        /// Constants section
        /// </summary>
        const string location = "Location";
        const string drone = "Drone";

        /// <summary>
        /// Constructor
        /// </summary>
        public DroneController() {
           
        }


        /// <summary>
        /// Get all routes for every drone/locations provide in the input
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public List<string> Get([FromQuery] string input) {

            var drones = readValuesDrones(input);
            var locations = readValuesForLocations(input);
            var routesDrones = getBestRoutesLocationsForDrones(drones, locations);
            var result = acommodateAllValuestoShowIntheUI (routesDrones);
            return result;
        }

        /// <summary>
        /// Get all values drones from the input
        /// </summary>
        /// <param name="input">text area</param>
        /// <returns>List of drones</returns>
        private List<Drone> readValuesDrones(string input) {
            input = input.Replace("\n", ",");
            var dronesTemporary = input.Split(',');
            var drones = new List<Drone>();
            for (int i = 0; i < dronesTemporary.Length; i++) {

                if (dronesTemporary[i].Contains(drone)) {
                    dronesTemporary[i + 1] = dronesTemporary[i + 1].Replace ("[", string.Empty);
                    dronesTemporary[i + 1] = dronesTemporary[i + 1].Replace("]", string.Empty);

                    drones.Add(new Drone { Trips = new List<Trip>(), Name = dronesTemporary[i] , Weight = int.Parse(dronesTemporary[i + 1])});
                }
            }

            return drones;
        }

        /// <summary>
        /// Get all values locations from the input
        /// </summary>
        /// <param name="input">text area</param>
        /// <returns>List of locations</returns>
        private List<Location> readValuesForLocations(string input) {
            input = input.Replace("\n", ",");
            var locationsTemporary = input.Split(',');
            var locations = new List<Location>();
            for (int i = 0; i < locationsTemporary.Length; i++) {

                if (locationsTemporary[i].Contains(location)) {
                    locationsTemporary[i + 1] = locationsTemporary[i + 1].Replace("[", string.Empty);
                    locationsTemporary[i + 1] = locationsTemporary[i + 1].Replace("]", string.Empty);

                    locations.Add(new Location { Name = locationsTemporary[i], Weight = int.Parse(locationsTemporary[i + 1]) });
                }
            }
            return locations;
        }

        /// <summary>
        /// Main method to get result and for every drone get trips 
        /// </summary>
        /// <param name="drones">drones</param>
        /// <param name="locations">locations</param>
        /// <returns>list of drones</returns>
        private List<Drone> getBestRoutesLocationsForDrones(List<Drone> drones, List<Location> locations) {

            locations = locations.OrderByDescending(l =>l.Weight).ToList();

            //we always start with the heaviest drones
            drones = drones.OrderByDescending(l => l.Weight).ToList();
            //If none of the drone cannot take a pachage we have to remove that locations from the list
            locations = locations.Where(l => l.Weight < drones.First().Weight).ToList();

            //we iterate until we don't have more locations to assign
            while (locations.Count() > 0) {

                // Always we use ALL available drones
                foreach (var drone in drones) {
                     //If one of the package has same weigth of drone then we assign that to the drone
                    var location = locations.Where(l => l.Weight == drone.Weight).FirstOrDefault();
                    if (location != null) {
                        drone.Trips.Add(new Trip { Locations = new List<Location> { location }});
                        locations.Remove(location);
                    } else {
                        // We iterate for temp locations trying to add maximun amount of locations into the drone 
                        var partialSum = 0;
                        drone.Trips.Add(new Trip() { Locations = new List<Location>()});
                        var trip = drone.Trips.Last();
                        var temporaryLocations = locations;
                        //If we have space in the drone we continue adding locations
                        foreach (var item in temporaryLocations.ToList()) {
                            if (partialSum + item.Weight <= drone.Weight) {
                                trip.Locations.Add(new Location { Weight = item.Weight, Name = item.Name });
                                partialSum += item.Weight;
                                locations.Remove(item);
                            } 
                        }
                    }
                }
            }

            return drones;
        }

        /// <summary>
        /// This just acommodate the data to show in the UI
        /// </summary>
        /// <param name="routesDrones"></param>
        /// <returns></returns>
        private List<string> acommodateAllValuestoShowIntheUI(List<Drone> routesDrones) {
          var result = new List<string>();
            foreach (var drone in routesDrones) {
                //First we add name of the drone
                result.Add(drone.Name);
                var counterTrips = 1;
                // second we add name of trip and for evry trip we add the locations
                foreach (var trip in drone.Trips) {
                    if (trip.Locations.Count > 0) {
                        result.Add("Trip #" + counterTrips);
                        var rowLocations = string.Empty;
                        foreach (var location in trip.Locations) {
                            rowLocations = string.Concat(rowLocations, location.Name + " ");
                        }
                        result.Add(rowLocations);
                        counterTrips++;
                    }
                }
            }
          return result;
        }
    }
}