using System;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Pokemon {
    public class PokemonController : SerializedMonoBehaviour
    {
        public Pokemon pokemon;

        public List<BaseMove> Moves => pokemon.moves;

        public async Awaitable OnMoveSelect(BaseMove selectedmove) {
            var gameManager = GameManager.Instance;
            if (!gameManager.OrNull()) throw new Exception("GameManager is null");
            
            var target = await GameManager.Instance.FindTarget(this)!;
            if (!target.OrNull()) {
                throw new Exception("Target not found");
            }
            gameManager.RegisterMove(this, selectedmove, target);

        }
    }
}