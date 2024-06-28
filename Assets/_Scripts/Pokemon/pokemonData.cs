using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;


// MVC =
// View -> GameObject
// Controller -> MonoBehaviour -> PokemonController
// Model -> ScriptableObject -> Pokemon

namespace _Scripts.Pokemon
{

    public interface ISaveable<T>
    {
        void Save();
        static T Load()
        {
            return default;
        }
    }

    public class GameManager : PersistentSingleton<GameManager>
    {
        public GameData gameData;

        public void Load()
        {
            gameData = GameData.Load();
            // TODO: Recreate scene from gameData
        }

        public void Save()
        {
            gameData.Save();
        }

        protected override void Awake()
        {
            base.Awake();
        }

        [Button]
        private void BuildScene() {
            GameObject playerPokemonParent = new GameObject();
            GameObject enemyPokemonParent = new GameObject();


            Instantiate(playerPokemonParent);
            Instantiate(enemyPokemonParent);
        }
    }

    [Serializable, CreateAssetMenu(fileName = "NewGameData", menuName = "Game Data")]
    public class GameData : SerializedScriptableObject, ISaveable<GameData>
    {
        public List<Pokemon> playerPokemon = new List<Pokemon>();
        public List<Pokemon> enemyPokemon = new List<Pokemon>();

        public List<Pokemon> playerSelectedPokemon = new List<Pokemon>();
        public List<Pokemon> enemySelectedPokemon = new List<Pokemon>();

        public void Save()
        {
            byte[] bytes = SerializationUtility.SerializeValue(this, DataFormat.JSON);
            File.WriteAllBytes(Application.persistentDataPath + "/gameData.json", bytes);
        }

        public static GameData Load()
        {
            if (File.Exists(Application.persistentDataPath + "/gameData.json"))
            {
                byte[] bytes = File.ReadAllBytes(Application.persistentDataPath + "/gameData.json");
                return SerializationUtility.DeserializeValue<GameData>(bytes, DataFormat.JSON);
            }
            return new GameData();
        }
    }

    [Serializable]
    public class Stats
    {
        public int hp = 1;
        public int attack = 1;
        public int defense = 1;
        public int speed = 1;
        public int special = 1;

        public static Stats RandomIVs()
        {
            return new Stats
            {
                hp = UnityEngine.Random.Range(0, 32),
                attack = UnityEngine.Random.Range(0, 32),
                defense = UnityEngine.Random.Range(0, 32),
                speed = UnityEngine.Random.Range(0, 32),
                special = UnityEngine.Random.Range(0, 32)
            };
        }

        public Stats EVsAdd(Stats stats)
        {
            int max = 255;
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

    public enum StatType
    {
        HP,
        Attack,
        Defense,
        Speed,
        Special
    }

    public class PokemonController : SerializedMonoBehaviour
    {
        public Pokemon pokemon;

        public List<IMove> Moves => pokemon.moves;
    }


    [Serializable, CreateAssetMenu(fileName = "NewPokemonData", menuName = "Pokemon Data")]
    public class PokemonData : SerializedScriptableObject
    {
        public int id = 1;
        public new string name = "Pokemon";
        public string nickname = "Pokemon";
        public Stats stats = new Stats();
        public PokemonType type = PokemonType.Normal;

        public Stats EVsDrop = new Stats();

        public List<IMove> learnableMoves = new List<IMove>();
    }

    [Serializable]
    public class Pokemon
    {
        [SerializeField]
        private PokemonData data;

        public int level = 1;

        public int hp = 1;

        public Stats IVs = Stats.RandomIVs();

        public Stats EVs = new Stats();

        [Range(-6, 6)] public int attackBuff = 0;
        [Range(-6, 6)] public int defenseBuff = 0;
        [Range(-6, 6)] public int speedBuff = 0;
        [Range(-6, 6)] public int specialBuff = 0;

        public List<IMove> moves = new List<IMove>();
        public StatusCondition statusCondition = StatusCondition.None;

        public int Level => level;

        public PokemonType Type => data.type;


        public int Attack => GetStatWithBuff(StatType.Attack, attackBuff);
        public int Defense => GetStatWithBuff(StatType.Defense, defenseBuff);
        public int Speed => GetStatWithBuff(StatType.Speed, speedBuff);
        public int Special => GetStatWithBuff(StatType.Special, specialBuff);


        private int GetStatWithBuff(StatType stat, int buff)
        {
            if (stat == StatType.HP)
            {
                return (2 * data.stats.hp + IVs.hp + EVs.hp / 4) * level / 100 + level + 10;
            }

            int statValue = 0;
            int ivValue = 0;
            int evValue = 0;
            switch (stat)
            {
                case StatType.Attack:
                    statValue = data.stats.attack;
                    ivValue = IVs.attack;
                    evValue = EVs.attack;
                    break;
                case StatType.Defense:
                    statValue = data.stats.defense;
                    ivValue = IVs.defense;
                    evValue = EVs.defense;
                    break;
                case StatType.Speed:
                    statValue = data.stats.speed;
                    ivValue = IVs.speed;
                    evValue = EVs.speed;
                    break;
                case StatType.Special:
                    statValue = data.stats.special;
                    ivValue = IVs.special;
                    evValue = EVs.special;
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


    [Serializable]
    public abstract class BaseMove : SerializedScriptableObject
    {
        public int id;
        public new string name;
        public int power;
        public int accuracy;
        public int pp;
        public int priority;
        public MoveCategory category;
        public PokemonType type;
        protected MoveType moveType;
    }

    [Serializable, CreateAssetMenu(fileName = "NewStandardDamageMove", menuName = "Moves/Standard Damage Move")]
    public class StandardDamageMove : BaseMove, IMove
    {
        protected new MoveType moveType = MoveType.StandardDamage;

        public void Use(Pokemon source, Pokemon target)
        {
            MoveCategory category = type.GetMoveCategory();
            int attack = category == MoveCategory.Physical ? source.Attack : source.Special;
            int defense = category == MoveCategory.Physical ? target.Defense : target.Special;

            float modifier = source.GetModifier(target, source.Type);
            int damage = source.GetMoveDamage(source.Level, power, attack, defense, modifier);

            target.TakeDamage(damage);
        }
    }


    [Serializable, CreateAssetMenu(fileName = "NewFixedDamageMove", menuName = "Moves/Fixed Damage Move")]
    public class FixedDamageMove : BaseMove, IMove
    {
        protected new MoveType moveType = MoveType.FixedDamage;

        public void Use(Pokemon source, Pokemon target)
        {
            target.TakeDamage(power);
        }
    }

    public interface IMove
    {
        void Use(Pokemon source, Pokemon target);

    }

    public static class PokemonExtensions
    {
        public static float GetModifier(this Pokemon source, Pokemon target, PokemonType moveType)
        {
            float stab = source.Type == moveType ? 1.5f : 1;
            float typeEffectiveness = moveType.GetEffectiveness(target.Type);
            float critical = UnityEngine.Random.Range(0, 100) < 6 ? 2 : 1;
            float random = UnityEngine.Random.Range(85, 100) / 100f;
            return 1 * stab * typeEffectiveness * critical * random;
        }

        public static int GetMoveDamage(this Pokemon pokemon, int level, int power, int attack, int defense, float modifier)
        {
            return (int)((2 * level * power * attack / (250 * defense) + 2) * modifier);
        }
    }


    public enum PokemonType
    {
        Normal,
        Fire,
        Water,
        Electric,
        Grass,
        Ice,
        Fighting,
        Poison,
        Ground,
        Flying,
        Psychic,
        Bug,
        Rock,
        Ghost,
        Dragon
    }

    public static class PokemonTypeExtensions
    {
        public static float GetEffectiveness(this PokemonType source, PokemonType target)
        {
            switch (source)
            {
                case PokemonType.Normal:
                    switch (target)
                    {
                        case PokemonType.Rock:
                            return 0.5f;
                        case PokemonType.Ghost:
                            return 0;
                        default:
                            return 1;
                    }
                case PokemonType.Fire:
                    switch (target)
                    {
                        case PokemonType.Fire:
                        case PokemonType.Water:
                        case PokemonType.Rock:
                        case PokemonType.Dragon:
                            return 0.5f;
                        case PokemonType.Grass:
                        case PokemonType.Ice:
                        case PokemonType.Bug:
                            return 2;
                        default:
                            return 1;
                    }
                case PokemonType.Water:
                    switch (target)
                    {
                        case PokemonType.Water:
                        case PokemonType.Grass:
                        case PokemonType.Dragon:
                            return 0.5f;
                        case PokemonType.Fire:
                        case PokemonType.Ground:
                        case PokemonType.Rock:
                            return 2;
                        default:
                            return 1;
                    }
                case PokemonType.Electric:
                    switch (target)
                    {
                        case PokemonType.Electric:
                        case PokemonType.Grass:
                        case PokemonType.Dragon:
                            return 0.5f;
                        case PokemonType.Water:
                        case PokemonType.Flying:
                            return 2;
                        case PokemonType.Ground:
                            return 0;
                        default:
                            return 1;
                    }
                case PokemonType.Grass:
                    switch (target)
                    {
                        case PokemonType.Fire:
                        case PokemonType.Grass:
                        case PokemonType.Poison:
                        case PokemonType.Flying:
                        case PokemonType.Bug:
                        case PokemonType.Dragon:
                            return 0.5f;
                        case PokemonType.Water:
                        case PokemonType.Ground:
                        case PokemonType.Rock:
                            return 2;
                        default:
                            return 1;
                    }
                case PokemonType.Ice:
                    switch (target)
                    {
                        case PokemonType.Fire:
                        case PokemonType.Water:
                        case PokemonType.Ice:
                            return 0.5f;
                        case PokemonType.Grass:
                        case PokemonType.Ground:
                        case PokemonType.Flying:
                        case PokemonType.Dragon:
                            return 2;
                        default:
                            return 1;
                    }
                case PokemonType.Fighting:
                    switch (target)
                    {
                        case PokemonType.Poison:
                        case PokemonType.Flying:
                        case PokemonType.Psychic:
                        case PokemonType.Bug:
                            return 0.5f;
                        case PokemonType.Normal:
                        case PokemonType.Ice:
                        case PokemonType.Rock:
                            return 2;
                        case PokemonType.Ghost:
                            return 0;
                        default:
                            return 1;
                    }
                case PokemonType.Poison:
                    switch (target)
                    {
                        case PokemonType.Poison:
                        case PokemonType.Ground:
                        case PokemonType.Rock:
                        case PokemonType.Ghost:
                            return 0.5f;
                        case PokemonType.Grass:
                            return 2;
                        default:
                            return 1;
                    }
                case PokemonType.Ground:
                    switch (target)
                    {
                        case PokemonType.Grass:
                        case PokemonType.Bug:
                            return 0.5f;
                        case PokemonType.Fire:
                        case PokemonType.Electric:
                        case PokemonType.Poison:
                        case PokemonType.Rock:
                            return 2;
                        case PokemonType.Flying:
                            return 0;
                        default:
                            return 1;
                    }
                case PokemonType.Flying:
                    switch (target)
                    {
                        case PokemonType.Electric:
                        case PokemonType.Rock:
                            return 0.5f;
                        case PokemonType.Grass:
                        case PokemonType.Fighting:
                        case PokemonType.Bug:
                            return 2;
                        default:
                            return 1;
                    }
                case PokemonType.Psychic:
                    switch (target)
                    {
                        case PokemonType.Psychic:
                            return 0.5f;
                        case PokemonType.Fighting:
                        case PokemonType.Poison:
                            return 2;
                        default:
                            return 1;
                    }
                case PokemonType.Bug:
                    switch (target)
                    {
                        case PokemonType.Fire:
                        case PokemonType.Fighting:
                        case PokemonType.Poison:
                        case PokemonType.Flying:
                        case PokemonType.Ghost:
                            return 0.5f;
                        case PokemonType.Grass:
                        case PokemonType.Psychic:
                            return 2;
                        default:
                            return 1;
                    }
                case PokemonType.Rock:
                    switch (target)
                    {
                        case PokemonType.Fighting:
                        case PokemonType.Ground:
                            return 0.5f;
                        case PokemonType.Fire:
                        case PokemonType.Ice:
                        case PokemonType.Flying:
                        case PokemonType.Bug:
                            return 2;
                        default:
                            return 1;
                    }
                case PokemonType.Ghost:
                    switch (target)
                    {
                        case PokemonType.Normal:
                        case PokemonType.Fighting:
                            return 0;
                        case PokemonType.Psychic:
                        case PokemonType.Ghost:
                            return 2;
                        default:
                            return 1;
                    }
                case PokemonType.Dragon:
                    switch (target)
                    {
                        case PokemonType.Dragon:
                            return 2;
                        default:
                            return 1;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(source), source, null);
            }
        }
    }

    public enum MoveCategory
    {
        Physical,
        Special,
        Status
    }

    public enum MoveType
    {
        // Simple
        StandardDamage,
        FixedDamage,
        // Buff,
        // Debuff,
        // TODO: Implement these
        // -- Moderate --
        // Status,
        // Healing,
        // MultiHit,
        // Recoil,
        // Flinch,
        // HighCritRatio,
        // Priority,

        // -- Complex --
        // MultiTurn,
        // StatusRecovery,
        // Trapping,
        // Charging

        // -- Unique --
        // SelfDestruct,
        // LeechSeed,
        // Substitute,
        // Transform,
        // Mirror
    }

    public enum StatusCondition
    {
        None,
        Poison,
        Burn,
        Paralyze,
        Sleep,
        Confusion
    }

    public static class PokemonTypeExtension
    {
        public static MoveCategory GetMoveCategory(this PokemonType type)
        {
            switch (type)
            {
                case PokemonType.Normal:
                case PokemonType.Ground:
                case PokemonType.Rock:
                case PokemonType.Flying:
                case PokemonType.Bug:
                case PokemonType.Fighting:
                case PokemonType.Poison:
                case PokemonType.Ghost:
                    return MoveCategory.Physical;
                case PokemonType.Fire:
                case PokemonType.Water:
                case PokemonType.Grass:
                case PokemonType.Electric:
                case PokemonType.Dragon:
                case PokemonType.Psychic:
                case PokemonType.Ice:
                    return MoveCategory.Special;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}