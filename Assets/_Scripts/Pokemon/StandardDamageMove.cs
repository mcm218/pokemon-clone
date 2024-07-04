using System;
using UnityEngine;

namespace _Scripts.Pokemon {
    [Serializable, CreateAssetMenu(fileName = "NewStandardDamageMove", menuName = "Moves/Standard Damage Move")]
    public class StandardDamageMove : BaseMove
    {
        protected new MoveType moveType = MoveType.StandardDamage;

        public override void Use(Pokemon source, Pokemon target)
        {
            MoveCategory category = type.GetMoveCategory();
            int          attack   = category == MoveCategory.Physical ? source.Attack : source.Special;
            int          defense  = category == MoveCategory.Physical ? target.Defense : target.Special;

            float modifier = source.GetModifier(target, source.Type);
            int   damage   = source.GetMoveDamage(source.Level, power, attack, defense, modifier);

            target.TakeDamage(damage);
        }
    }
}