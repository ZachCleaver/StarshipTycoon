using System.Collections.Generic;

namespace StarshipTycoon.Utils {
    class ItemUtils {
        public static List<Item> allItems = new List<Item> {
            new Item("Beer", 25),
            new Item("Wine", 40),
            new Item("Stout", 14),
            new Item("Growler", 10),
            new Item("Pint", 7)
        };
    }
}