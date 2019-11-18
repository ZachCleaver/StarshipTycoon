using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarshipTycoon.Utils;
using System;

namespace StarshipTycoon.InfoMenus {
    class ShipInfo : IShipInfo {
        public Ship ship { get; private set; }

        private Action setDestinationAction;
        private Action buyFuelAction;

        Rectangle exitRec;
        private Point exitRecOffset;
        Rectangle setDestinationRectangle;
        private Point setDestRecOffset;
        //TODO: Should this go on planet menu? Yeah, it should.....
        //TODO: Make button class since we're doing that with buyFuel and setDestination???
        Rectangle buyFuelRectangle;
        private Point buyFuelRecOffset;

        //NOTE: This identifier passed to base will have to change if the player is allowed to update the name
        public ShipInfo(Ship ship, Action setDestinationAction, Action buyFuelAction)
            : base("ShipInfo_" + ship.name, ship.getCollisionRectangle().X, ship.getCollisionRectangle().Y) {

            this.ship = ship;
            this.setDestinationAction = setDestinationAction;
            this.buyFuelAction = buyFuelAction;

            this.exitRecOffset = new Point(rect.Width - 8, 0);
            this.exitRec = new Rectangle(base.rect.Location + exitRecOffset, new Point(8, 8));
            this.setDestRecOffset = new Point(3, 40);
            this.setDestinationRectangle = new Rectangle(rect.Location + setDestRecOffset, new Point(140, 20));
            this.buyFuelRecOffset = new Point(3, 65);
            this.buyFuelRectangle = new Rectangle(rect.Location + buyFuelRecOffset, new Point(160, 20));
        }
        
        //TODO: Don't make these values in rectangles so hardcoded
        public override void draw(SpriteBatch sb) {
            int x = base.rect.X;
            int y = base.rect.Y;

            sb.Draw(texture, base.rect, Color.White * 0.5f);
            sb.DrawString(font, "Name: " + ship.name, new Vector2(x + 3, y + 3), Color.Black);
            sb.DrawString(font, "Fuel: " + (int)ship.fuelRemaining + "/" + ship.fuelCapacity, new Vector2(x + 3, y + 20), Color.Black);

            //TODO: Give this its own texture so it looks like an 'X'
            sb.Draw(texture, exitRec, Color.White);

            Color setDestinationColor = Color.AliceBlue;

            if (input.rectangle.Intersects(setDestinationRectangle)) {
                setDestinationColor = Color.DarkSeaGreen;
            }
            sb.Draw(texture, setDestinationRectangle, setDestinationColor);
            sb.DrawString(font, "Select Destination", new Vector2(x + 5, y + 43), Color.Black);

            Color buyFuelColor = Color.AliceBlue;

            if (input.rectangle.Intersects(buyFuelRectangle)) {
                buyFuelColor = Color.DarkSeaGreen;
            }
            sb.Draw(texture, buyFuelRectangle, buyFuelColor);
            sb.DrawString(font, "Buy Fuel for " + ship.dest.fuelCost + " per unit.", new Vector2(x + 5, y + 68), Color.Black);
        }

        public override void update() {
            base.update();

            if (input.wasLeftButtonClicked()) {
                if (input.rectangle.Intersects(exitRec)) {
                    ModalUtil.removeModal(id);
                } else if (input.rectangle.Intersects(setDestinationRectangle)) {
                    setDestinationAction();
                }
            } else if (input.wasLeftButtonClickedAndHeld() && input.rectangle.Intersects(buyFuelRectangle)) {
                buyFuelAction();
            }

            if (input.wasLeftButtonClickedAndHeld() && input.rectangle.Intersects(rect) && input.didMouseMove) {
                exitRec.Location = rect.Location + exitRecOffset;
                setDestinationRectangle.Location = rect.Location + setDestRecOffset;
                buyFuelRectangle.Location = rect.Location + buyFuelRecOffset;
            }
        }
    }
}