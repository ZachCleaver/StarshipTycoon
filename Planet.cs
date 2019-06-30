using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarshipTycoon.Utils;

namespace StarshipTycoon {
    class Planet {
        private Texture2D texture;
        public MarketPlace market;
        public string name { get; set; }
        private int x, y, height, width;
        private Color color;
        public Rectangle rectangle { get; set; }
        public int timesVisited = 0;
        public int fuelCost { get; set; }

        public Planet(Texture2D texture, int x, int y, int height, int width, Color color, string name) {
            this.texture = texture;
            this.x = x;
            this.y = y;
            this.height = height;
            this.width = width;
            this.rectangle = new Rectangle(x, y, width, height);
            this.color = color;
            this.name = name;

            this.market = new MarketPlace();
            this.fuelCost = RandomHelper.Instance.Next(1, 3);
        }

        public void Draw(SpriteBatch sb) {
            sb.Draw(texture, rectangle, color);
        }
    }
}