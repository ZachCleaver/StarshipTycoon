using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarshipTycoon.InfoMenus {
    abstract class BaseInfo {
        public string id { get; private set; }

        protected static InputHandler input;

        protected static Texture2D texture;
        protected static SpriteFont font;
        protected Rectangle rect { get; private set; }

        //NOTE: This works if we want all menus to have the same background and font
        //We'll need to move this out to each Info's Interface class if we want them to be different
        public static void init(Texture2D texture, SpriteFont font) {
            BaseInfo.texture = texture;
            BaseInfo.font = font;

            input = InputHandler.Instance;
            
        }

        public BaseInfo(Rectangle rect, string uniqueIdentifier) {
            this.rect = rect;
            this.id = uniqueIdentifier;
        }

        public abstract void draw(SpriteBatch sb);
    }
}
