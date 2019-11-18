using Microsoft.Xna.Framework;

namespace StarshipTycoon.InfoMenus {
    abstract class IShipInfo : BaseInfo {
        private static readonly int WIDTH = 180;
        private static readonly int HEIGHT = 100;

        public IShipInfo(string uniqueIdentifier, int shipX, int shipY) : 
            base(new Rectangle(shipX, shipY, WIDTH, HEIGHT), uniqueIdentifier) {
        }
    }
}