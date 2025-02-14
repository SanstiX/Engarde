using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Xsl;

namespace Engarde
{
    public class Character
    {
        public string Name { get; }
        public int HP { get; protected set; } = 100;
        public int MaxHP = 100;
        public string buffEffect;
        public int Armor { get; protected set; } = 0;
        private bool defending;
        public bool countered;
        private bool hasJustBuffed = false;
        public bool armorJustBroken = false; //geen comments?
        public bool gotCrit;

        public bool IsDefeated => HP <= 0;

        public Character(string name, int hp, int armor)
        {
            Name = name;
            HP = hp;
            Armor = armor;
        }

        public void Attack(Character opponent)
        {
            int damage = 10;
            Random rnd = new Random();
            if (rnd.Next(0, 10) == 0)
            {
                damage *= 2;
                gotCrit = true;
            }
            if (opponent.defending)
            {
                int counternum = rnd.Next(0, 5);
                if (counternum == 0 && !gotCrit)
                    opponent.countered = true;
                opponent.defending = false;
            }
            if (opponent.countered)
            {
                HP -= damage;  
            }
            else
            {
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
            }
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
            int buffType = rnd.Next(0, 4);
            if (buffType == 0)
            {
                HP = Math.Min(100, HP + 15);
                buffEffect = "heal";
            }
            else if (buffType == 1)
            { 
                Armor += 15;
                buffEffect = "armor";
            }
            else
            {
                HP = HP;
                buffEffect = "nothing";
            }


            hasJustBuffed = true;
            return true;
        }
    }
}
