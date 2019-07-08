using System;

namespace StarshipTycoon.Utils {
    class RandomHelper : Random {
        static RandomHelper _Instance;
        public static RandomHelper Instance {
            get {
                if (_Instance == null) {
                    _Instance = new RandomHelper();
                }
                return _Instance;
            }
        }

        private RandomHelper() { }
    }
}