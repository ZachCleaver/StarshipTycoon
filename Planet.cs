using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarshipTycoon {
    class Planet {
        private Texture2D texture;
        public string name { get; set; }
        private int x, y, height, width;
        private Color color;
        public Rectangle rectangle { get; set; }
        public int timesVisited = 0;

        public Planet(Texture2D texture, int x, int y, int height, int width, Color color, string name) {
            this.texture = texture;
            this.x = x;
            this.y = y;
            this.height = height;
            this.width = width;
            this.rectangle = new Rectangle(x, y, width, height);
            this.color = color;
            this.name = name;
        }

        public void Draw(SpriteBatch sb) {
            sb.Draw(texture, new Rectangle(x, y, width, height), color);
        }
    }
}