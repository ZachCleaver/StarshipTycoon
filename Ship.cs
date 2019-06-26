using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StarshipTycoon {
    class Ship {
        InputHandler inputHander = InputHandler.Instance;
        private Texture2D texture;
        public String name { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public double speed { get; set; }
        public double angle { get; set; }
        public double fuelCapacity { get; set; }
        public double fuelRemaining { get; set; }
        public Planet dest { get; set; }
        public bool needNewDest { get; set; }
        public Rectangle rect { get; set; }
        public bool isDocked { get; set; }
        private Color color;

        //TODO: Make this base class so we don't have to pass in specifics for each ship type
        public Ship(String name, Texture2D texture, int x, int y, int width, int height, double speed, int fuelCapacity, Color color) {
            this.name = name;
            this.texture = texture;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.speed = speed;
            this.color = color;

            this.fuelCapacity = fuelCapacity;
            this.fuelRemaining = fuelCapacity;
        }

        public void draw(SpriteBatch sb) {
            sb.Draw(texture, new Rectangle(rect.X + width / 2, rect.Y + height / 2, width, height), 
                null, color, (float)angle, new Vector2(texture.Width / 2, texture.Height / 2),
                SpriteEffects.None, 0f);

            if (!isDocked) {
                drawLine(sb);
            }
        }

        //TODO: Uhhh, does this belong here?
        //http://gamedev.stackexchange.com/questions/44015/how-can-i-draw-a-simple-2d-line-in-xna-without-using-3d-primitives-and-shders
        void drawLine(SpriteBatch sb) {
            int centerX = rect.Center.X;
            int centerY = rect.Center.Y;

            Vector2 edge = dest.rectangle.Center.ToVector2() - new Vector2(centerX, centerY);
            // calculate angle to rotate line

            sb.Draw(texture,// rectangle defines shape of line and position of start of line
                new Rectangle(centerX, centerY, (int)edge.Length(), //sb will stretch the texture to fill this rectangle
                    1), //width of line, change this to make thicker line
                null,
                Color.White, //colour of line
                (float) (angle + Math.PI / 2),     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);
        }

        public void update() {
            if (needNewDest) {
                dest = PlanetUtil.getDestination(this);

                if (dest != null) {
                    isDocked = false;
                    updateAngle();
                }
            }

            if (!isDocked) {
                move();
            }
        }

        //TODO: Flesh this out
        public int sellCargo() {
            int moneyMade = new Random().Next(10);
            Console.Out.WriteLine(this.name + " made " + moneyMade + " creds!");
            return moneyMade;
        }

        public void updateAngle() {
            angle = Math.Atan2(y - dest.rectangle.Center.Y, x - dest.rectangle.Center.X) + Math.PI / 2;
        }

        private void move() {
            bool xArrived = false;
            bool yArrived = false;

            //TODO: Set once
            int destX = dest.rectangle.Center.X;
            int destY = dest.rectangle.Center.Y;
            rect = new Rectangle((int)x, (int)y, width, height);

            int centerX = rect.Center.X;
            int centerY = rect.Center.Y;

            if (centerX != destX) {
                double normalSpeed = speed * Math.Sin(angle);
                double leftoverDistance = destX - centerX;

                if (Math.Abs(normalSpeed) < Math.Abs(leftoverDistance)) {
                    x -= normalSpeed;
                } else {
                    x += leftoverDistance;
                }
            } else {
                xArrived = true;
            }

            if (centerY != destY) {
                double normalSpeedY = speed * Math.Cos(angle);
                double leftoverDistanceY = destY - centerY;

                if (Math.Abs(normalSpeedY) < Math.Abs(leftoverDistanceY)) {
                    y += normalSpeedY;
                } else {
                    y += leftoverDistanceY;
                }
            } else {
                yArrived = true;
            }

            //TODO: Make this take actual x and y values away so it's more accurate
            fuelRemaining -= speed;

            //NOTE: Enable to auto-get new destination
            needNewDest = xArrived && yArrived;

            if (xArrived && yArrived && !isDocked) {
                isDocked = true;
                dest.timesVisited++;
                angle = 0;
            }
        }
    }
}