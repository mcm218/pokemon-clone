using System;

namespace _Scripts.Pokemon {
    [Serializable]
    public abstract class BaseMove : IMove {
        private         MoveData     data;
        public          int          id => data.id;
        public          string       name => data.name;
        public          int          power => data.power;
        public          int          accuracy => data.accuracy;
        public          int          pp;
        public          int          maxPP => data.pp;
        public          int          priority => data.priority;
        public          MoveCategory category => data.category;
        public          PokemonType  type => data.type;
        protected       MoveType     moveType => data.moveType;
        public abstract void         Use(Pokemon source, Pokemon target);


        public BaseMove() {
            
        }
        
        public BaseMove(MoveData data) {
            this.data = data;
            pp = maxPP;
        }
    }

}