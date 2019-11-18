using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StarshipTycoon.InfoMenus {
    class HeaderBar {
        private static HeaderBar instance;

        private static Player player;
        private static Rectangle rect;
        private static Texture2D texture;
        private static SpriteFont font { get; set; }

        public static void init(Player player, Texture2D texture, SpriteFont font) {
            instance = new HeaderBar(player, texture, font);
        }

        private HeaderBar(Player player, Texture2D texture, SpriteFont font) {
            HeaderBar.texture = texture;
            HeaderBar.font = font;
            HeaderBar.player = player;

            rect = new Rectangle(0, 0, Globals.screenWidth, 30);
        }

        public static HeaderBar Instance {
            get {
                if (instance == null) {
                    throw new NotImplementedException();
                }
                return instance;
            }
        }

        public void draw(SpriteBatch sb) {
            sb.Draw(texture, rect, Color.ForestGreen);
            sb.DrawString(font, "'Sup widdit, playa?", new Vector2(5, 7), Color.Black);
            sb.DrawString(font, "Credtis: " + player.money, new Vector2(200, 7), Color.Black);
        }
    }
}
