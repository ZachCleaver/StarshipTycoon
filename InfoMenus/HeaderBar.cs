using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarshipTycoon.InfoMenus {
    class HeaderBar : BaseInfo {
        private static HeaderBar instance;

        private static Rectangle rect;
        private static Player player;

        private HeaderBar() {}

        public static void init(Player player) {
            rect = new Rectangle(0, 0, Globals.screenWidth, 30);
            HeaderBar.player = player;
        }

        public static HeaderBar Instance {
            get {
                if (instance == null) {
                    instance = new HeaderBar();
                }
                return instance;
            }
        }

        public override void draw(SpriteBatch sb) {
            sb.Draw(texture, rect, Color.ForestGreen);
            sb.DrawString(font, "'Sup widdit, playa?", new Vector2(5, 7), Color.Black);
            sb.DrawString(font, "Credtis: " + player.money, new Vector2(200, 7), Color.Black);
        }
    }
}
