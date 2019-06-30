using System;
using System.Collections.Generic;
using System.Linq;

namespace StarshipTycoon.Utils {
    static class PlanetUtil {
        private static List<Planet> planets;

        public static void init(ref List<Planet> planets) {
            PlanetUtil.planets = planets;
        }

        public static List<Planet> getPlanets() {
            return planets;
        }

        /// <summary>
        /// Gets a random planet in range of the ship. If no
        /// planets are in range, the ship's original destination
        /// is returned.
        /// </summary>
        /// <param name="ship"></param>
        /// <returns></returns>
        public static Planet getRandomValidDestination(Ship ship) {
            List<Planet> validPlanets = planets.FindAll(p => {
                return p != ship.dest && isPlanetInRange(p, ship);
            });

            if (validPlanets.Any()) {
                int index = RandomHelper.Instance.Next(validPlanets.Count);

                return validPlanets[index];
            }

            return ship.dest;
        }

        /// <summary>
        /// Returns a List of Planets that a ship could reach if its fuel was full.
        /// </summary>
        /// <param name="ship"></param>
        /// <returns></returns>
        public static List<Planet> findPlanetsInPotentialRange(Ship ship) {
            return planets.FindAll(planet => isPlanetInRange(planet, ship));
        }

        public static Planet getPlanetMouseHoveringOver() {
            return planets.Find(planet => planet.rectangle.Intersects(InputHandler.Instance.rectangle));
        }

        public static bool isPlanetInRange(Planet planet, Ship ship) {
            double distanceBetween = Math.Sqrt(Math.Pow(planet.rectangle.Center.X - ship.rect.Center.X, 2) +
                Math.Pow(planet.rectangle.Center.Y - ship.rect.Center.Y, 2));
            //TODO: Is there a better way to do this fuel check?
            return distanceBetween <= ship.fuelRemaining;
        }
    }
}