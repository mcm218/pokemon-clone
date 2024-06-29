using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace _Scripts.Pokemon {
    public class PokemonController : SerializedMonoBehaviour
    {
        public Pokemon pokemon;

        public List<IMove> Moves => pokemon.moves;
    }
}