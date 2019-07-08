using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StarshipTycoon.Players;
using StarshipTycoon.Utils;
using System.Collections.Generic;

namespace StarshipTycoon {
    class Player : BasePlayer {
        private InputHandler input = InputHandler.Instance;
        private HashSet<Ship> displayShipInfo = new HashSet<Ship>();
        private List<Ship> shipInfoToRemove = new List<Ship>();
        private Ship shipLookingForDestination = null;

        public override void update() {
            ships.ForEach(ship => {
                if (!ship.isDocked) {
                    ship.move();
                }
            });

            if (input.wasKeyPressedAndReleased(Keys.C)) {
                shipLookingForDestination = null;
            }

            if (input.wasLeftButtonClicked()) {
                //TODO: How do we handle displays for multiple ships?
                //Additional popup asking them to choose which ship?
                List<Ship> selectedShips = ships.FindAll(ship => input.rectangle.Intersects(ship.getCollisionRectangle()));
                selectedShips.ForEach(ship => {
                    displayShipInfo.Add(ship);
                });
                
                //Player clicked the 'Select Destination' button on ShipInfo
                if (shipLookingForDestination != null) {
                    //Make sure the Player clicked a planet to set the destination
                    Planet selectedPlanet = PlanetUtil.getPlanets().Find(planet => planet.getCollisionRectangle().Intersects(input.rectangle));
                    if (selectedPlanet != null) {
                        if (PlanetUtil.isPlanetInRange(selectedPlanet, shipLookingForDestination)) {
                            shipLookingForDestination.setNewDestination(selectedPlanet);
                        } else {
                            //TODO: Alert that planet is out of range
                        }
                    }
                    //Cancel looking for a destination whether a planet is clicked or not
                    shipLookingForDestination = null;
                }
            }

            shipInfoToRemove.ForEach(ship => {
                displayShipInfo.Remove(ship);
            });
            shipInfoToRemove.Clear();
        }

        public override void draw(SpriteBatch sb) {
            ships.ForEach(ship => ship.draw(sb));
        }

        public void drawNoTransform(SpriteBatch sb) {
            //Draw line from ship to mouse
            //TODO: Add target on mouse if over valid planet, error sign on mouse over invalid planet?
            if (shipLookingForDestination != null) {
                Color lineColor = Color.Red;
                Planet hoverPlanet = PlanetUtil.getPlanetMouseHoveringOver();

                if (hoverPlanet != null) {
                    if (PlanetUtil.isPlanetInRange(hoverPlanet, shipLookingForDestination)) {
                        lineColor = Color.Green;
                    } else {
                        //TODO: Change mouse to error
                    }
                }
                DrawUtil.drawLine(sb, shipLookingForDestination.getCollisionRectangle().Center.ToVector2(), input.pos, lineColor);
            }

            foreach (Ship ship in displayShipInfo) {
                //TODO: Don't make this static. Make it a class and add the actions once...
                ShipInfo.draw(sb, ship,
                    () => { shipInfoToRemove.Add(ship); },
                    () => { shipLookingForDestination = ship; },
                    () => { buyFuel(ship); });
            }
        }

        private void buyFuel(Ship ship) {
            if (ship.fuelRemaining < ship.fuelCapacity && this.money >= ship.dest.fuelCost) {
                money -= ship.dest.fuelCost;
                ship.fuelRemaining++;
            }
        }
    }
}