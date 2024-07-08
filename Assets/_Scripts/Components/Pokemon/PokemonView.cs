using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Pokemon {
    public class PokemonView : MonoBehaviour {
        [FormerlySerializedAs("pokemonController")] public PokemonController controller;
        
        public GameObject movePanel;
        
        public List<MoveButtonUI> moveButtons = new List<MoveButtonUI>();

        public void Show() {
            movePanel.SetActive(true);
        }
        
        public void Hide() {
            movePanel.SetActive(false);
        }

        private void Awake() {
            Hide();
            controller = GetComponent<PokemonController>();
        }

        // TODO: Should this be move to the controller class?
        private void Start() {
            for (int index = 0; index < controller.Moves.Count; index++) {
                var move = controller.Moves[index];
                var moveButton = moveButtons[index];
                if (!moveButton.OrNull()) {
                    throw new Exception("MoveButton is null");
                }
                moveButton.SetMove(move, () => OnMoveSelect(move), (_) => { }, (_) => { });
            }
        }

        public void OnMoveSelect(BaseMove selectedMove) {
            controller.OnMoveSelect(selectedMove);
        }
    }

}