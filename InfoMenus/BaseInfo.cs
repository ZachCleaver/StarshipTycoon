using Microsoft.Xna.Framework.Graphics;

namespace StarshipTycoon.InfoMenus {
    abstract class BaseInfo {
        protected static InputHandler input;
        protected static Texture2D texture;
        protected static SpriteFont font;

        public static void init(Texture2D texture, SpriteFont font) {
            BaseInfo.texture = texture;
            BaseInfo.font = font;
            input = InputHandler.Instance;
        }

        public abstract void draw(SpriteBatch sb);
    }
}
