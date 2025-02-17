using System;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Pokemon {
    public class PokemonController : MonoBehaviour
    {
        public Pokemon pokemon;

        public ObservableList<BaseMove> Moves = new ObservableList<BaseMove>();


        private void Awake() {
            if (pokemon == null) {
                throw new Exception("Pokemon is null");
            }
            Moves.AddRange(pokemon.moves);
        }
        
        public void OnTurnStart() {
            var view = GetComponent<PokemonView>();
            if (!view.OrNull()) {
                throw new Exception("PokemonView is null");
            }
            view.Show();
        }

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