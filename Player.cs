using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace StarshipTycoon {
    class Player {
        public List<Ship> ships { get; set; }
        public int money { get; set; }

        public Player() {
            ships = new List<Ship>();
            money = 1000;
        }

        public void update() {
            ships.ForEach(ship => {
                ship.update();
                if (ship.isDocked) {
                    purchaseFuel(ship);
                    money += ship.sellCargo();
                }
            });
        }

        private void purchaseFuel(Ship ship) {
            double fuelToBuy = ship.fuelCapacity - ship.fuelRemaining;

            //TODO: Determine cost of fuel based on current planet
            if (money > 0 && fuelToBuy > 0) {
                int moneyToSpend = (int) Math.Min(money, fuelToBuy);
                money -= moneyToSpend;
                ship.fuelRemaining += moneyToSpend;
                Console.Out.WriteLine("Purchased " + moneyToSpend + " for ship " + ship.name + ".");
            }
        }

        public void draw(SpriteBatch spriteBatch) {
            ships.ForEach(ship => ship.draw(spriteBatch));
        }
    }
}
