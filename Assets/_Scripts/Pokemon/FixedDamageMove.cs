using System;
using UnityEngine;

namespace _Scripts.Pokemon {
    [Serializable, CreateAssetMenu(fileName = "NewFixedDamageMove", menuName = "Moves/Fixed Damage Move")]
    public class FixedDamageMove : BaseMove, IMove
    {
        protected new MoveType moveType = MoveType.FixedDamage;

        public void Use(Pokemon source, Pokemon target)
        {
            target.TakeDamage(power);
        }
    }
}