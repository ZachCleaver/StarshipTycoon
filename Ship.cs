using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarshipTycoon.Utils;
using System;
using System.Collections.Generic;

namespace StarshipTycoon {
    class Ship {
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
        //TODO: Should this be a vector? We only care about center and timesVisited
        public Planet dest { get; set; }
        public bool needNewDest { get; set; }
        private RectangleExtension rect;
        public bool isDocked { get; set; }
        private Color color;
        private List<Item> cargo;
        public bool marketStepComplete { get; set; }

        //TODO: Make this base class so we don't have to pass in specifics for each ship type
        public Ship(String name, Texture2D texture, Planet startingPlanet, int width, int height, double speed, int fuelCapacity, Color color) {
            this.name = name;
            this.texture = texture;
            this.dest = startingPlanet;

            this.x = startingPlanet.getDrawingRectangle().Center.X;
            this.y = startingPlanet.getDrawingRectangle().Center.Y;
            this.width = width;
            this.height = height;
            this.rect = new RectangleExtension((int)x, (int)y, width, height);

            this.speed = speed;
            this.color = color;

            this.fuelCapacity = fuelCapacity;
            this.fuelRemaining = fuelCapacity;
            cargo = new List<Item>();
            this.marketStepComplete = true;
            this.isDocked = true;
        }

        public void setNewDestination(Planet selectedPlanet) {
            this.dest = selectedPlanet;
            this.needNewDest = false;
            this.isDocked = false;
            this.updateAngle();
        }

        public void draw(SpriteBatch sb) {
            if (!isDocked) {
                //TODO: C'mon, you know this is janky
                sb.End();
                sb.Begin();
                DrawUtil.drawLine(sb, rect.getCollisionRectangle().Center.ToVector2(), dest.getCollisionRectangle().Center.ToVector2(), angle + Math.PI / 2);
                sb.End();
                sb.Begin(SpriteSortMode.Deferred, null,
                null, null, null, null, Globals.camera.TranslationMatrix);
            }

            sb.Draw(texture, new Rectangle(rect.getDrawingRectangle().X + width / 2, rect.getDrawingRectangle().Y + height / 2, width, height), 
                null, color, (float)angle, new Vector2(texture.Width / 2, texture.Height / 2),
                SpriteEffects.None, 0f);
        }

        public void update() {
            if (needNewDest) {
                Planet newDest = PlanetUtil.getRandomValidDestination(this);

                //We'll get the same destination back only if we don't have enough fuel to travel
                if (dest != newDest) {
                    isDocked = false;
                    dest = newDest;
                    updateAngle();
                }
            }

            if (!isDocked && fuelRemaining > 0) {
                move();
            }
        }

        //TODO: Flesh this out
        public int sellCargo() {
            int moneyMade = 0;
            int itemsSold = 0;
            List<String> itemsToRemove = new List<String>();

            cargo.ForEach(item => {
                Item itemOnPlanet = dest.market.items.Find(i => i.name.Equals(item.name));
                if (itemOnPlanet != null) {
                    moneyMade += itemOnPlanet.cost;
                    itemsSold++;
                    itemsToRemove.Add(itemOnPlanet.name);
                }
            });
            Console.Out.WriteLine(this.name + " sold " + itemsSold + " items for " + moneyMade + " creds!");
            cargo.RemoveAll(item => {
                return itemsToRemove.Contains(item.name);
            });
            Console.Out.WriteLine(this.name + " still has " + cargo.Count + " items.");
            return moneyMade;
        }

        public int buyCargo(int money) {
            if (dest.market.items.Count == 0) {
                Console.Out.WriteLine("Planet has no items!");
            }
            dest.market.items.ForEach(item => {
                if (money > item.cost) {
                    Console.Out.WriteLine("Bought " + item.name + " for " + item.cost);
                    money -= item.cost;
                    cargo.Add(item);
                } else {
                    Console.Out.WriteLine("Could not afford " + item.name + " for " + item.cost + "!");
                }
            });

            return money;
        }

        public void updateAngle() {
            //This HAS to match the values used below in move()
            angle = Math.Atan2(rect.getDrawingRectangle().Center.Y - dest.getDrawingRectangle().Center.Y, 
                rect.getDrawingRectangle().Center.X - dest.getDrawingRectangle().Center.X) + Math.PI / 2;
        }

        public void move() {
            bool xArrived = false;
            bool yArrived = false;

            rect = new RectangleExtension((int)x, (int)y, width, height);
            //TODO: Set once
            double destX = dest.getDrawingRectangle().Center.X;
            double destY = dest.getDrawingRectangle().Center.Y;

            double centerX = rect.getDrawingRectangle().Center.X;
            double centerY = rect.getDrawingRectangle().Center.Y;

            double xTraveled= 0, yTraveled = 0;

            if (centerX != destX) {
                double normalSpeed = speed * Math.Sin(angle);
                double leftoverDistance = (destX - centerX);

                if (Math.Abs(normalSpeed) < Math.Abs(leftoverDistance)) {
                    x -= normalSpeed;
                    xTraveled = normalSpeed;
                } else {
                    x += leftoverDistance;
                    xTraveled = leftoverDistance;
                }
            } else {
                xArrived = true;
            }

            if (centerY != destY) {
                double normalSpeedY = speed * Math.Cos(angle);
                double leftoverDistanceY = (destY - centerY);

                if (Math.Abs(normalSpeedY) < Math.Abs(leftoverDistanceY)) {
                    y += normalSpeedY;
                    yTraveled = normalSpeedY;
                } else {
                    y += leftoverDistanceY;
                    yTraveled = leftoverDistanceY;
                }
            } else {
                yArrived = true;
            }

            //General assumption that could be slightly off, but nothing to be overly concerned about
            double fuelSpent = Math.Sqrt(Math.Pow(xTraveled, 2) + Math.Pow(yTraveled, 2));
            fuelRemaining -= fuelSpent;

            //NOTE: Enable to auto-get new destination
            needNewDest = xArrived && yArrived;

            if (xArrived && yArrived && !isDocked) {
                isDocked = true;
                marketStepComplete = false;
                dest.timesVisited++;
                angle = 0;
            }
        }

        public Rectangle getCollisionRectangle() {
            return rect.getCollisionRectangle();
        }

        public Rectangle getDrawingRectangle() {
            return rect.getDrawingRectangle();
        }
    }
}