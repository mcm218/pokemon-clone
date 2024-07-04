using Sirenix.OdinInspector;
using System;

namespace _Scripts.Pokemon {
    [Serializable]
    public abstract class BaseMove : SerializedScriptableObject, IMove
    {
        public          int          id;
        public new      string       name;
        public          int          power;
        public          int          accuracy;
        public          int          pp;
        public          int          priority;
        public          MoveCategory category;
        public          PokemonType  type;
        protected       MoveType     moveType;
        public abstract void         Use(Pokemon source, Pokemon target);
    }
}