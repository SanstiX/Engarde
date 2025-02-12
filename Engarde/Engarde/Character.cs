using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engarde
{
    public class Character
    {
        public string Name { get; }
        public int HP { get; private set; } = 100;
        public int Armor { get; private set; } = 0;
        private bool defending;
        private bool hasJustBuffed = false;
        public bool armorJustBroken = false;

        public bool IsDefeated => HP <= 0;

        public Character(string name, int hp)
        {
            Name = name;
            HP = hp;
        }

        public void Attack(Character opponent)
        {
            int damage = 10;
            if (opponent.defending)
            {
                damage /= 2;
                opponent.defending = false;
            }
            if (opponent.Armor > 0)
            {
                if ((opponent.Armor - damage) <= 0)
                {
                    opponent.HP = Math.Max(0, opponent.HP - (damage - opponent.Armor));
                    opponent.Armor = 0;
                    opponent.armorJustBroken = true;
                }
                else
                    opponent.Armor -= damage;
            }
            else
                opponent.HP = Math.Max(0, opponent.HP - damage);
            hasJustBuffed = false;
        }

        public void Defend()
        {
            defending = true;
            hasJustBuffed = false;
        }

        public bool TryBuff()
        {
            if (hasJustBuffed)
                return false;

            Random rnd = new Random();
            int buffType = rnd.Next(0, 3);
            if (buffType == 0)
                HP = Math.Min(100, HP + 15);
            else if (buffType == 1)
                Armor += 30;

            hasJustBuffed = true;
            return true;
        }
    }
}
