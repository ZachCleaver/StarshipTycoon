using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace StarshipTycoon.Players {
    abstract class BasePlayer {
        public List<Ship> ships { get; set; }
        public int money { get; set; }

        protected BasePlayer() {
            ships = new List<Ship>();
            money = 1000;
        }

        public abstract void update();

        public abstract void draw(SpriteBatch spriteBatch);
    }
}