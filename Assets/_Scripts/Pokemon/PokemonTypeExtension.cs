using System;

namespace _Scripts.Pokemon {
    public static class PokemonTypeExtension
    {
        public static MoveCategory GetMoveCategory(this PokemonType type)
        {
            switch (type)
            {
                case PokemonType.Normal:
                case PokemonType.Ground:
                case PokemonType.Rock:
                case PokemonType.Flying:
                case PokemonType.Bug:
                case PokemonType.Fighting:
                case PokemonType.Poison:
                case PokemonType.Ghost:
                    return MoveCategory.Physical;
                case PokemonType.Fire:
                case PokemonType.Water:
                case PokemonType.Grass:
                case PokemonType.Electric:
                case PokemonType.Dragon:
                case PokemonType.Psychic:
                case PokemonType.Ice:
                    return MoveCategory.Special;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}