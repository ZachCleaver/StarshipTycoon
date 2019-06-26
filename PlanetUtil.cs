using System;
using System.Collections.Generic;
using System.Linq;

namespace StarshipTycoon {
    static class PlanetUtil {
        private static Random random;
        private static List<Planet> planets;

        public static void init(ref List<Planet> planets) {
            random = new Random();
            PlanetUtil.planets = planets;
        }

        public static Planet getDestination(Ship ship) {
            List<Planet> validPlanets = planets.FindAll(p => {
                return p != ship.dest &&
                (p.rectangle.Location.ToVector2() - ship.rect.Location.ToVector2()).Length() <= ship.fuelRemaining;
                //TODO: Is there a better way to do this fuel check?
            });

            if (validPlanets.Any()) {
                int index = random.Next(validPlanets.Count);

                return validPlanets[index];
            }

            return null;
        }
    }
}