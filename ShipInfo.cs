using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StarshipTycoon {
    static class ShipInfo {
        private static InputHandler input;
        private static Texture2D texture;
        private static SpriteFont font;

        public static void setTexture(Texture2D texture, SpriteFont font) {
            ShipInfo.texture = texture;
            ShipInfo.font = font;
            input = InputHandler.Instance;
        }

        //TODO: Draw in different position if off screen
        //TODO: Don't make these values in rectangles so hardcoded
        public static void draw(SpriteBatch sb, Ship ship, Action exitAction, Action setDestinationAction, Action buyFuelAction) {
            sb.Draw(texture, new Rectangle(ship.rect.X, ship.rect.Y, 180, 100), Color.White * 0.5f);
            sb.DrawString(font, "Name: " + ship.name, new Vector2(ship.rect.X + 3, ship.rect.Y + 3), Color.Black);
            sb.DrawString(font, "Fuel: " + (int)ship.fuelRemaining + "/" + ship.fuelCapacity, new Vector2(ship.rect.X + 3, ship.rect.Y + 20), Color.Black);

            //TODO: Give this its own texture so it looks like an 'X'
            Rectangle exitRec = new Rectangle(ship.rect.X + 150 - 8, ship.rect.Y, 8, 8);
            sb.Draw(texture, exitRec, Color.White);

            Rectangle setDestinationRectangle = new Rectangle(ship.rect.X + 3, ship.rect.Y + 40, 140, 20);
            Color setDestinationColor = Color.AliceBlue;

            if (input.rectangle.Intersects(setDestinationRectangle)) {
                setDestinationColor = Color.DarkSeaGreen;
            }
            sb.Draw(texture, setDestinationRectangle, setDestinationColor);
            sb.DrawString(font, "Select Destination", new Vector2(ship.rect.X + 5, ship.rect.Y + 43), Color.Black);

            //TODO: Should this go on planet menu? Yeah, it should.....
            //TODO: Make button class since we're doing that with buyFuel and setDestination???
            Rectangle buyFuelRectangle = new Rectangle(ship.rect.X + 3, ship.rect.Y + 65, 160, 20);
            Color buyFuelColor = Color.AliceBlue;

            if (input.rectangle.Intersects(buyFuelRectangle)) {
                buyFuelColor = Color.DarkSeaGreen;
            }
            sb.Draw(texture, buyFuelRectangle, buyFuelColor);
            sb.DrawString(font, "Buy Fuel for " + ship.dest.fuelCost + " per unit.", new Vector2(ship.rect.X + 5, ship.rect.Y + 68), Color.Black);

            if (input.wasLeftButtonClicked()) {
                if (input.rectangle.Intersects(exitRec)) {
                    exitAction();
                } else if (input.rectangle.Intersects(setDestinationRectangle)) {
                    setDestinationAction();
                }
            } else if (input.wasLeftButtonClickedAndHeld() && input.rectangle.Intersects(buyFuelRectangle)) {
                buyFuelAction();
            }
        }
    }
}
