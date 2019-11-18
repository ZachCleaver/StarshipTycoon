using Microsoft.Xna.Framework.Graphics;
using StarshipTycoon.InfoMenus;
using System;
using System.Collections.Generic;

namespace StarshipTycoon.Utils {
    static class ModalUtil {
        public static Dictionary<string, BaseInfo> modals { get; set; }

        private static List<string> modalsToRemove { get; set; }

        public static void init() {
            modals = new Dictionary<string, BaseInfo>();
            modalsToRemove = new List<string>();
        }

        public static void addModal(BaseInfo modal) {
            if (new List<string>(modals.Keys).Contains(modal.id)) {
                Console.Out.WriteLine("WARN: modal already exists for ID " + modal.id + ".");
            } else {
                modals.Add(modal.id, modal);
            }
        }

        public static void removeModal(string identifier) {
            modalsToRemove.Add(identifier);
        }

        public static void drawModals(SpriteBatch spriteBatch) {
            foreach (KeyValuePair<string, BaseInfo> entry in modals) {
                entry.Value.draw(spriteBatch);
            }

            foreach (string id in modalsToRemove) {
                modals.Remove(id);
            }
            modalsToRemove.Clear();
        }

        public static void updateModals() {
            foreach (BaseInfo modal in modals.Values) {
                modal.update();
            }
        }
    }
}
