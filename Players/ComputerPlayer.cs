using System;
using Microsoft.Xna.Framework.Graphics;
using StarshipTycoon.Players;

namespace StarshipTycoon {
    class ComputerPlayer : BasePlayer {

        public override void update() {
            ships.ForEach(ship => {
                ship.update();
                if (ship.isDocked) {
                    if (!ship.marketStepComplete) {
                        purchaseFuel(ship);
                        ship.marketStepComplete = true;
                    }
                    //Even though these are market steps, there could be a case where
                    //the player has no money for the ship to buy items, but another
                    //ship hasn't reached port and sold their items yet. Just because
                    //we don't have money now doesn't mean another ship won't make some.
                    money += ship.sellCargo();
                    money = ship.buyCargo(money);
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

        public override void draw(SpriteBatch spriteBatch) {
            ships.ForEach(ship => ship.draw(spriteBatch));
        }
    }
}