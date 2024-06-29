using System;
using UnityEngine;

namespace _Scripts.Pokemon {
    [Serializable]
    public class Stats
    {
        public int hp      = 1;
        public int attack  = 1;
        public int defense = 1;
        public int speed   = 1;
        public int special = 1;

        public static Stats RandomIVs()
        {
            return new Stats
            {
                hp      = UnityEngine.Random.Range(0, 32),
                attack  = UnityEngine.Random.Range(0, 32),
                defense = UnityEngine.Random.Range(0, 32),
                speed   = UnityEngine.Random.Range(0, 32),
                special = UnityEngine.Random.Range(0, 32)
            };
        }

        public Stats EVsAdd(Stats stats)
        {
            int max      = 255;
            int totalMax = 512;
            hp = Mathf.Clamp(hp + stats.hp, 0, max);
            if (Total() > totalMax)
            {
                hp = Mathf.Clamp(hp - (Total() - totalMax), 0, max);
            }

            attack = Mathf.Clamp(attack + stats.attack, 0, max);
            if (Total() > totalMax)
            {
                attack = Mathf.Clamp(attack - (Total() - totalMax), 0, max);
            }

            defense = Mathf.Clamp(defense + stats.defense, 0, max);
            if (Total() > totalMax)
            {
                defense = Mathf.Clamp(defense - (Total() - totalMax), 0, max);
            }

            speed = Mathf.Clamp(speed + stats.speed, 0, max);
            if (Total() > totalMax)
            {
                speed = Mathf.Clamp(speed - (Total() - totalMax), 0, max);
            }

            special = Mathf.Clamp(special + stats.special, 0, max);
            if (Total() > totalMax)
            {
                special = Mathf.Clamp(special - (Total() - totalMax), 0, max);
            }

            return this;
        }

        public int Total()
        {
            return hp + attack + defense + speed + special;
        }
    }
}