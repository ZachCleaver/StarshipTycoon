using Microsoft.Xna.Framework;

namespace StarshipTycoon.Utils {
    class RectangleExtension {
        private Rectangle rectangle;

        public RectangleExtension(int x, int y, int width, int height) {
            this.rectangle = new Rectangle(x, y, width, height);
        }

        /// <summary>
        /// Returns the unaltered rectangle used for drawing. The Globals.Camera will
        /// handle resizing, repositioning, etc. via SpriteBatch.begin() in Main.cs.
        /// </summary>
        /// <returns></returns>
        public Rectangle getDrawingRectangle() {
            return rectangle;
        }

        public Rectangle getCollisionRectangle() {
            Vector2 pos = getCollisionVector();
            Vector2 size = rectangle.Size.ToVector2() * Globals.camera.Zoom;
            return new Rectangle(pos.ToPoint(), size.ToPoint());
        }

        private Vector2 getCollisionVector() {
            return Vector2.Transform(rectangle.Location.ToVector2(), Globals.camera.TranslationMatrix);
        }
    }
}