using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StarshipTycoon.InfoMenus;
using StarshipTycoon.Players;
using StarshipTycoon.Utils;
using System.Collections.Generic;

namespace StarshipTycoon {
    class Player : BasePlayer {
        private InputHandler input = InputHandler.Instance;
        private Ship shipLookingForDestination = null;
        private Texture2D hoverPlanetErrorText;
        private Texture2D hoverPlanetSuccessText;

        public Player(Texture2D hoverPlanetErrorText, Texture2D hoverPlanetSuccessText) : base() {
            this.hoverPlanetErrorText = hoverPlanetErrorText;
            this.hoverPlanetSuccessText = hoverPlanetSuccessText;
        }

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
                    ShipInfo info = new ShipInfo(ship,
                        () => { shipLookingForDestination = ship; },
                        () => { buyFuel(ship); });

                    ModalUtil.addModal(info);
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
        }

        public override void draw(SpriteBatch sb) {
            ships.ForEach(ship => ship.draw(sb));
        }

        public void drawNoTransform(SpriteBatch sb) {
            //Draw line from ship to mouse
            if (shipLookingForDestination != null) {
                Color lineColor = Color.Red;
                Planet hoverPlanet = PlanetUtil.getPlanetMouseHoveringOver();

                if (hoverPlanet != null) {
                    if (PlanetUtil.isPlanetInRange(hoverPlanet, shipLookingForDestination)) {
                        lineColor = Color.Green;
                        InputHandler.Instance.tempMouseTexture = this.hoverPlanetSuccessText;
                    } else {
                        InputHandler.Instance.tempMouseTexture = this.hoverPlanetErrorText;
                    }
                }
                DrawUtil.drawLine(sb, shipLookingForDestination.getCollisionRectangle().Center.ToVector2(), input.pos, lineColor);
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