using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.Pokemon {
    [CreateAssetMenu(menuName = "Create MoveData", fileName = "MoveData", order = 0)]
    public class MoveData : SerializedScriptableObject {
        public     int          id;
        public new string       name;
        public     int          power;
        public     int          accuracy;
        public     int          pp;
        public     int          priority;
        public     MoveCategory category;
        public     PokemonType  type;
        public     MoveType     moveType;
    }
}