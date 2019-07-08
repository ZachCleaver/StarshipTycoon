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
            //int x = ship.getCollisionRectangle().X;
            //int y = ship.getCollisionRectangle().Y;
            int width = 180;
            int height = 100;
            int x = Globals.screenWidth - width;
            int y = 0;



            sb.Draw(texture, new Rectangle(x, y, width, height), Color.White * 0.5f);
            sb.DrawString(font, "Name: " + ship.name, new Vector2(x + 3, y + 3), Color.Black);
            sb.DrawString(font, "Fuel: " + (int)ship.fuelRemaining + "/" + ship.fuelCapacity, new Vector2(x + 3, y + 20), Color.Black);

            //TODO: Give this its own texture so it looks like an 'X'
            Rectangle exitRec = new Rectangle(x + 150 - 8, y, 8, 8);
            sb.Draw(texture, exitRec, Color.White);

            Rectangle setDestinationRectangle = new Rectangle(x + 3, y + 40, 140, 20);
            Color setDestinationColor = Color.AliceBlue;

            if (input.rectangle.Intersects(setDestinationRectangle)) {
                setDestinationColor = Color.DarkSeaGreen;
            }
            sb.Draw(texture, setDestinationRectangle, setDestinationColor);
            sb.DrawString(font, "Select Destination", new Vector2(x + 5, y + 43), Color.Black);

            //TODO: Should this go on planet menu? Yeah, it should.....
            //TODO: Make button class since we're doing that with buyFuel and setDestination???
            Rectangle buyFuelRectangle = new Rectangle(x + 3, y + 65, 160, 20);
            Color buyFuelColor = Color.AliceBlue;

            if (input.rectangle.Intersects(buyFuelRectangle)) {
                buyFuelColor = Color.DarkSeaGreen;
            }
            sb.Draw(texture, buyFuelRectangle, buyFuelColor);
            sb.DrawString(font, "Buy Fuel for " + ship.dest.fuelCost + " per unit.", new Vector2(x + 5, y + 68), Color.Black);

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
