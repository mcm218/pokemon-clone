using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


// MVC =
// View -> GameObject
// Controller -> MonoBehaviour -> PokemonController
// Model -> ScriptableObject -> Pokémon

namespace _Scripts.Pokemon
{
    [Serializable, CreateAssetMenu(fileName = "NewPokemonData", menuName = "Pokemon Data")]
    public class PokemonData : SerializedScriptableObject
    {
        public     int         id       = 1;
        public new string      name     = "Pokemon";
        public     string      nickname = "Pokemon";
        public     Stats       stats    = new Stats();
        public     Sprite      sprite;
        public     PokemonType type     = PokemonType.Normal;

        public Stats EVsDrop = new Stats();

        public List<BaseMove> learnableMoves = new List<BaseMove>();
    }
}