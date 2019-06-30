using System;

namespace StarshipTycoon {
    class Item {
        public String name { get; set; }
        public int cost { get; set; }

        public Item(String name, int cost) {
            this.name = name;
            this.cost = cost;
        }
    }
}