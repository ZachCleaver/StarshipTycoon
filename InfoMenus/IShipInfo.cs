using Microsoft.Xna.Framework;

namespace StarshipTycoon.InfoMenus {
    abstract class IShipInfo : BaseInfo {
        private static readonly int WIDTH = 180;
        private static readonly int HEIGHT = 100;
        private static readonly int X = Globals.screenWidth - WIDTH;
        private static readonly int Y = 30;

        public IShipInfo(string uniqueIdentifier) : base(new Rectangle(X, Y, WIDTH, HEIGHT), uniqueIdentifier) {

        }
    }
}
