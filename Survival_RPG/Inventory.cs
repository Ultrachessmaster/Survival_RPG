using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Engine;
using Microsoft.Xna.Framework;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Survival {
    class Inventory {
        IWeapon[] weapons = new IWeapon[5];
        Food[] foods = new Food[5];

        int weaponslot = 0;
        int foodslot = 0;

        public void AddWeapon(IWeapon weapon) {
            if (weaponslot < weapons.Length) {
                weapons[weaponslot] = weapon;
                weaponslot++;
            }
        }

        public void AddFood(Food food) {
            if (foodslot < foods.Length) {
                foods[foodslot] = food;
                foodslot++;
            }
        }

        public void Draw(SpriteBatch sb) {
            Text itemtext = new Text(Vector2.Zero, "", SRPG.Arial, Color.Blue);
            StringBuilder strb = new StringBuilder("WEAPONS:\n");
            foreach(IWeapon weapon in weapons) {
                if (weapon == null)
                    continue;
                strb.AppendLine(weapon.Name + " - Attack: " + weapon.Attack);
            }

            strb.AppendLine("=============");
            strb.AppendLine("FOODS:");
            foreach (Food food in foods) {
                if (food == null)
                    continue;
                strb.AppendLine(food.Name + " - Satiation: " + food.hunger_restore);
            }

            itemtext.text = strb.ToString();
            itemtext.Draw(sb);
        }

        public void Update() {
            if(Input.IsKeyPressed(Keys.LeftControl) || Input.IsKeyPressed(Keys.RightControl)) {
                SRPG.GameSt = GameState.Normal;
            }
        }
    }
}
