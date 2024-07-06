using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Scripts.Pokemon {
    public class PokemonView : SerializedMonoBehaviour {
        [FormerlySerializedAs("pokemonController")] public PokemonController controller;
        
        public List<MoveButtonUI> moveButtons = new List<MoveButtonUI>();

        private void Awake() {
            controller = GetComponent<PokemonController>();
        }

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
    
    public class MoveButtonUI : SerializedMonoBehaviour {
        private BaseMove move;
        
        public Button button;
        private ButtonHover buttonHover;

        private void Awake() {
            button = GetComponent<Button>();
            buttonHover = GetComponent<ButtonHover>();
            if (buttonHover == null) {
                buttonHover = gameObject.AddComponent<ButtonHover>();
            }
        }
        
        public void SetMove(BaseMove move, UnityAction onMoveSelect,UnityAction<PointerEventData> onPointerEnter, UnityAction<PointerEventData> onPointerExit) {
            this.move = move;
            button.onClick.AddListener(onMoveSelect);
            buttonHover.onPointerEnter = onPointerEnter;
            buttonHover.onPointerExit  = onPointerExit;
        }
    }
    
    public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        public UnityAction<PointerEventData> onPointerEnter;
        public UnityAction<PointerEventData> onPointerExit;
        
        public void OnPointerEnter(PointerEventData eventData) {
            onPointerEnter.Invoke(eventData);
        }

        public void OnPointerExit(PointerEventData eventData) {
            onPointerExit.Invoke(eventData);
        }
    }
}