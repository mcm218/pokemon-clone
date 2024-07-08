using System.Collections.Generic;
using System.Linq;

namespace _Scripts.Pokemon {
    public static class PokemonExtensions
    {
        public static float GetModifier(this Pokemon source, Pokemon target, PokemonType moveType)
        {
            float stab              = source.Type == moveType ? 1.5f : 1;
            float typeEffectiveness = moveType.GetEffectiveness(target.Type);
            float critical          = UnityEngine.Random.Range(0, 100) < 6 ? 2 : 1;
            float random            = UnityEngine.Random.Range(85, 100) / 100f;
            return 1 * stab * typeEffectiveness * critical * random;
        }

        public static int GetMoveDamage(this Pokemon pokemon, int level, int power, int attack, int defense, float modifier)
        {
            return (int)((2 * level * power * attack / (250 * defense) + 2) * modifier);
        }
        
        public static GameManager SafeGetManager() {
            GameManager instance = GameManager.Instance;
            if (!instance) { throw new System.Exception("GameManager instance not found"); }
            return instance;
        }
        
        /// <summary>
        /// Selects the move with the highest priority and then the highest speed stat
        /// </summary>
        /// <param name="pokemonList"></param>
        /// <returns></returns>
        public static SelectedMove SelectBySpeed(this IList<SelectedMove> pokemonList) {
            return pokemonList
                   .OrderByDescending(x => x.move.priority)
                   .ThenByDescending(p => p.source.pokemon.Speed)
                   .First();
        }
    }
}