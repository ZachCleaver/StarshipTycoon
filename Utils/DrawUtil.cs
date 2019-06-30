using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StarshipTycoon.Utils {
    class DrawUtil {
        private static Texture2D texture;

        public static void init(Texture2D texture) {
            DrawUtil.texture = texture;
        }

        //http://gamedev.stackexchange.com/questions/44015/how-can-i-draw-a-simple-2d-line-in-xna-without-using-3d-primitives-and-shders
        public static void drawLine(SpriteBatch sb, Vector2 start, Vector2 end, double angle, Color color) {
            Vector2 edge = end - start;

            sb.Draw(texture,// rectangle defines shape of line and position of start of line
                new Rectangle((int)start.X, (int)start.Y, (int)edge.Length(), //sb will stretch the texture to fill this rectangle
                    1), //width of line, change this to make thicker line
                null,
                color, //color of line
                (float)angle, //angle of line
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);
        }

        public static void drawLine(SpriteBatch sb, Vector2 start, Vector2 end, double angle) {
            drawLine(sb, start, end, angle, Color.White);
        }

        public static void drawLine(SpriteBatch sb, Vector2 start, Vector2 end, Color color) {
            Vector2 edge = end - start;
            // calculate angle to rotate line
            float angle = (float)Math.Atan2(edge.Y, edge.X);
            drawLine(sb, start, end, angle, color);
        }
    }
}