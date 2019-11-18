using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarshipTycoon.InfoMenus {
    abstract class BaseInfo {
        public string id { get; private set; }

        protected static InputHandler input;

        protected static Texture2D texture;
        protected static SpriteFont font;
        protected Rectangle rect;

        private Point mouseOffset;

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

        //TODO: Bounding to window. You can drag outside of screen and lose modal.
        //TODO: Only move when clicking on title bar of modal?
        //TODO: Only move modal that has focus. This will currently move all that the mouse touches
        public virtual void update() {
            //The first time we click on the modal, register how far away the top left of
            //the modal is from the mouse's location
            if (input.wasLeftButtonClickedAndHeld() && input.rectangle.Intersects(rect)) {
                if (mouseOffset == new Point()) {
                    mouseOffset = input.pos.ToPoint() - rect.Location;
                }
            } else if (!input.wasLeftButtonClickedAndHeld()) {
                mouseOffset = new Point();
            }

            //If the offset has been registered, make sure the modal location is always
            //the same distance away from the mouse for as long as it is held down
            if (mouseOffset != new Point()) {
                rect.Location = input.pos.ToPoint() - mouseOffset;
            }
        }
    }
}
