using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Pokemon {
    [Serializable]
    public class Pokemon
    {
        [FormerlySerializedAs("data2"),SerializeField]
        public PokemonData data;

        public int level = 1;

        [ReadOnly]
        public int baseHP = -1;

        public int currentHP = -1;

        public Stats IVs = new Stats();

        public Stats EVs = new Stats();

        [Range(-6, 6)] public int attackBuff  = 0;
        [Range(-6, 6)] public int defenseBuff = 0;
        [Range(-6, 6)] public int speedBuff   = 0;
        [Range(-6, 6)] public int specialBuff = 0;

        [SerializeField, ReadOnly]
        public List<BaseMove>     moves           = new List<BaseMove>();
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

        [Button]
        public void ResetHP() {
            baseHP = (2 * data.stats.hp + IVs.hp + EVs.hp / 4) * level / 100 + level + 10;
            currentHP = baseHP;
        }

        [Button]
        public void ResetMoves() {
            moves = new List<BaseMove>();
            for (int i = 0; i < 4; i++) {
                if (data.learnableMoves.Count > i) {
                    moves.Add(data.learnableMoves[i]);
                }
            }
        }

        public void UseMove(int moveIndex, Pokemon target)
        {
            moves[moveIndex].Use(this, target);
        }


        public void TakeDamage(int damage)
        {
            currentHP -= damage;
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