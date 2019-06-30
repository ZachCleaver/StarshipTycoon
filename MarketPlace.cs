using StarshipTycoon.Utils;
using System.Collections.Generic;

namespace StarshipTycoon {
    class MarketPlace {
        public List<Item> items { get; set; }

        public MarketPlace() {
            items = new List<Item>();

            int itemsToCreate = RandomHelper.Instance.Next(ItemUtils.allItems.Count);
            for (int i = 0; i < itemsToCreate; i++) {
                Item item = ItemUtils.allItems[i];
                items.Add(new Item(item.name, item.cost + RandomHelper.Instance.Next(-item.cost / 2, item.cost / 2)));
            }
        }
    }
}