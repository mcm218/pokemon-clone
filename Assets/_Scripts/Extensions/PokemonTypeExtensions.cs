using System;

namespace _Scripts.Pokemon {
    public static class PokemonTypeExtensions
    {
        public static float GetEffectiveness(this PokemonType source, PokemonType target)
        {
            switch (source)
            {
                case PokemonType.Normal:
                    switch (target)
                    {
                        case PokemonType.Rock:
                            return 0.5f;
                        case PokemonType.Ghost:
                            return 0;
                        default:
                            return 1;
                    }
                case PokemonType.Fire:
                    switch (target)
                    {
                        case PokemonType.Fire:
                        case PokemonType.Water:
                        case PokemonType.Rock:
                        case PokemonType.Dragon:
                            return 0.5f;
                        case PokemonType.Grass:
                        case PokemonType.Ice:
                        case PokemonType.Bug:
                            return 2;
                        default:
                            return 1;
                    }
                case PokemonType.Water:
                    switch (target)
                    {
                        case PokemonType.Water:
                        case PokemonType.Grass:
                        case PokemonType.Dragon:
                            return 0.5f;
                        case PokemonType.Fire:
                        case PokemonType.Ground:
                        case PokemonType.Rock:
                            return 2;
                        default:
                            return 1;
                    }
                case PokemonType.Electric:
                    switch (target)
                    {
                        case PokemonType.Electric:
                        case PokemonType.Grass:
                        case PokemonType.Dragon:
                            return 0.5f;
                        case PokemonType.Water:
                        case PokemonType.Flying:
                            return 2;
                        case PokemonType.Ground:
                            return 0;
                        default:
                            return 1;
                    }
                case PokemonType.Grass:
                    switch (target)
                    {
                        case PokemonType.Fire:
                        case PokemonType.Grass:
                        case PokemonType.Poison:
                        case PokemonType.Flying:
                        case PokemonType.Bug:
                        case PokemonType.Dragon:
                            return 0.5f;
                        case PokemonType.Water:
                        case PokemonType.Ground:
                        case PokemonType.Rock:
                            return 2;
                        default:
                            return 1;
                    }
                case PokemonType.Ice:
                    switch (target)
                    {
                        case PokemonType.Fire:
                        case PokemonType.Water:
                        case PokemonType.Ice:
                            return 0.5f;
                        case PokemonType.Grass:
                        case PokemonType.Ground:
                        case PokemonType.Flying:
                        case PokemonType.Dragon:
                            return 2;
                        default:
                            return 1;
                    }
                case PokemonType.Fighting:
                    switch (target)
                    {
                        case PokemonType.Poison:
                        case PokemonType.Flying:
                        case PokemonType.Psychic:
                        case PokemonType.Bug:
                            return 0.5f;
                        case PokemonType.Normal:
                        case PokemonType.Ice:
                        case PokemonType.Rock:
                            return 2;
                        case PokemonType.Ghost:
                            return 0;
                        default:
                            return 1;
                    }
                case PokemonType.Poison:
                    switch (target)
                    {
                        case PokemonType.Poison:
                        case PokemonType.Ground:
                        case PokemonType.Rock:
                        case PokemonType.Ghost:
                            return 0.5f;
                        case PokemonType.Grass:
                            return 2;
                        default:
                            return 1;
                    }
                case PokemonType.Ground:
                    switch (target)
                    {
                        case PokemonType.Grass:
                        case PokemonType.Bug:
                            return 0.5f;
                        case PokemonType.Fire:
                        case PokemonType.Electric:
                        case PokemonType.Poison:
                        case PokemonType.Rock:
                            return 2;
                        case PokemonType.Flying:
                            return 0;
                        default:
                            return 1;
                    }
                case PokemonType.Flying:
                    switch (target)
                    {
                        case PokemonType.Electric:
                        case PokemonType.Rock:
                            return 0.5f;
                        case PokemonType.Grass:
                        case PokemonType.Fighting:
                        case PokemonType.Bug:
                            return 2;
                        default:
                            return 1;
                    }
                case PokemonType.Psychic:
                    switch (target)
                    {
                        case PokemonType.Psychic:
                            return 0.5f;
                        case PokemonType.Fighting:
                        case PokemonType.Poison:
                            return 2;
                        default:
                            return 1;
                    }
                case PokemonType.Bug:
                    switch (target)
                    {
                        case PokemonType.Fire:
                        case PokemonType.Fighting:
                        case PokemonType.Poison:
                        case PokemonType.Flying:
                        case PokemonType.Ghost:
                            return 0.5f;
                        case PokemonType.Grass:
                        case PokemonType.Psychic:
                            return 2;
                        default:
                            return 1;
                    }
                case PokemonType.Rock:
                    switch (target)
                    {
                        case PokemonType.Fighting:
                        case PokemonType.Ground:
                            return 0.5f;
                        case PokemonType.Fire:
                        case PokemonType.Ice:
                        case PokemonType.Flying:
                        case PokemonType.Bug:
                            return 2;
                        default:
                            return 1;
                    }
                case PokemonType.Ghost:
                    switch (target)
                    {
                        case PokemonType.Normal:
                        case PokemonType.Fighting:
                            return 0;
                        case PokemonType.Psychic:
                        case PokemonType.Ghost:
                            return 2;
                        default:
                            return 1;
                    }
                case PokemonType.Dragon:
                    switch (target)
                    {
                        case PokemonType.Dragon:
                            return 2;
                        default:
                            return 1;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(source), source, null);
            }
        }
    }
}