using System;
using UnityEngine;

namespace _Scripts.Pokemon {
    [Serializable, CreateAssetMenu(fileName = "NewFixedDamageMove", menuName = "Moves/Fixed Damage Move")]
    public class FixedDamageMove : BaseMove
    {
        protected new MoveType moveType = MoveType.FixedDamage;

        public override void Use(Pokemon source, Pokemon target)
        {
            target.TakeDamage(power);
        }
    }
}