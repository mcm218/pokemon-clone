using Sirenix.OdinInspector;
using System;

namespace _Scripts.Pokemon {
    [Serializable]
    public abstract class BaseMove : SerializedScriptableObject
    {
        public     int          id;
        public new string       name;
        public     int          power;
        public     int          accuracy;
        public     int          pp;
        public     int          priority;
        public     MoveCategory category;
        public     PokemonType  type;
        protected  MoveType     moveType;
    }
}