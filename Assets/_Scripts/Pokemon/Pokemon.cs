using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Pokemon {
    [Serializable]
    public class Pokemon
    {
        [SerializeField]
        public PokemonData data;

        public int level = 1;

        public int hp = 1;

        public Stats IVs = new Stats();

        public Stats EVs = new Stats();

        [Range(-6, 6)] public int attackBuff  = 0;
        [Range(-6, 6)] public int defenseBuff = 0;
        [Range(-6, 6)] public int speedBuff   = 0;
        [Range(-6, 6)] public int specialBuff = 0;

        public List<IMove>     moves           = new List<IMove>();
        public StatusCondition statusCondition = StatusCondition.None;

        public int Level => level;

        public PokemonType Type => data.type;


        public int Attack  => GetStatWithBuff(StatType.Attack,  attackBuff);
        public int Defense => GetStatWithBuff(StatType.Defense, defenseBuff);
        public int Speed   => GetStatWithBuff(StatType.Speed,   speedBuff);
        public int Special => GetStatWithBuff(StatType.Special, specialBuff);


        private int GetStatWithBuff(StatType stat, int buff)
        {
            if (stat == StatType.HP)
            {
                return (2 * data.stats.hp + IVs.hp + EVs.hp / 4) * level / 100 + level + 10;
            }

            int statValue = 0;
            int ivValue   = 0;
            int evValue   = 0;
            switch (stat)
            {
                case StatType.Attack:
                    statValue = data.stats.attack;
                    ivValue   = IVs.attack;
                    evValue   = EVs.attack;
                    break;
                case StatType.Defense:
                    statValue = data.stats.defense;
                    ivValue   = IVs.defense;
                    evValue   = EVs.defense;
                    break;
                case StatType.Speed:
                    statValue = data.stats.speed;
                    ivValue   = IVs.speed;
                    evValue   = EVs.speed;
                    break;
                case StatType.Special:
                    statValue = data.stats.special;
                    ivValue   = IVs.special;
                    evValue   = EVs.special;
                    break;
            }
            return (int)(((2 * statValue + ivValue + evValue / 4) * level / 100 + 5) * BuffToMultiplier(buff));
        }

        public void UseMove(int moveIndex, Pokemon target)
        {
            moves[moveIndex].Use(this, target);
        }


        public void TakeDamage(int damage)
        {
            hp -= damage;
        }

        public float BuffToMultiplier(int stage)
        {
            if (stage < 0)
            {
                return 2 / (-stage - 2);
            }
            return (stage + 2) / 2;
        }
    }
}