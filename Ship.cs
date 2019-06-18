using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StarshipTycoon {
    class Ship {
        private Texture2D texture;
        public double x { get; set; }
        public double y { get; set; }
        private int width, height;
        private double speed;
        public double angle { get; set; }
        public double fuelCapacity { get; set; }
        public double fuelRemaining { get; set; }
        public Planet dest { get; set; }
        public bool needNewDest { get; set; }

        public Ship(Texture2D texture, int x, int y, int width, int height, double speed) {
            this.texture = texture;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.speed = speed;
        }


        public void Draw(SpriteBatch sb) {
            //sb.Draw(texture, new Rectangle((int)x, (int)y, width, height), Color.White);
            sb.Draw(texture, new Rectangle((int)x, (int)y, width, height), null, Color.White, (float)angle, new Vector2(0, 0), SpriteEffects.None, 0f);
        }

        public void Update() {
            bool xArrived = false;
            bool yArrived = false;

            if ((int)x != dest.rectangle.X) {
                double normalSpeed = speed * Math.Sin(angle);
                double leftoverDistance = dest.rectangle.X - x;

                if (Math.Abs(normalSpeed) < Math.Abs(leftoverDistance)) {
                    x -= normalSpeed;
                } else {
                    x += leftoverDistance;
                }
            } else {
                xArrived = true;
            }

            if ((int)y != dest.rectangle.Y) {
                double normalSpeedY = speed * Math.Cos(angle);
                double leftoverDistanceY = dest.rectangle.Y - y;

                if (Math.Abs(normalSpeedY) < Math.Abs(leftoverDistanceY)) {
                    y += normalSpeedY;
                } else {
                    y += leftoverDistanceY;
                }
            } else {
                yArrived = true;
            }

            needNewDest = xArrived && yArrived;
        }
    }
}