using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Scripts.Pokemon {
    [RequireComponent(typeof(Button)), RequireComponent(typeof(ButtonHover))]
    public class MoveButtonUI : MonoBehaviour {
        private BaseMove move;
        
        public  Button      button;
        public  TextMeshProUGUI        moveLabel;
        private ButtonHover buttonHover;

        private void Awake() {
            button      = GetComponent<Button>();
            buttonHover = GetComponent<ButtonHover>();
            if (buttonHover == null) {
                buttonHover = this.gameObject.AddComponent<ButtonHover>();
            }
            
            moveLabel = button.GetComponentInChildren<TextMeshProUGUI>(); 
            if (moveLabel == null) {
                GameObject label = new GameObject("Label");
                label.transform.SetParent(button.transform);
                
                moveLabel                         = label.AddComponent<TextMeshProUGUI>();
                moveLabel.alignment               = TextAlignmentOptions.Midline;
                moveLabel.font                    = Resources.GetBuiltinResource<TMP_FontAsset>("Arial.ttf");
                moveLabel.fontSize                = 14;
                moveLabel.color                   = Color.white;
                moveLabel.rectTransform.sizeDelta = new Vector2(200, 50);
            }
        }
        
        public void SetMove(BaseMove move, UnityAction onMoveSelect,UnityAction<PointerEventData> onPointerEnter, UnityAction<PointerEventData> onPointerExit) {
            this.move = move;
            button.onClick.AddListener(onMoveSelect);
            buttonHover.onPointerEnter = onPointerEnter;
            buttonHover.onPointerExit  = onPointerExit;
            SetLabel(move.pp, move.maxPP);
        }
        

        private void SetLabel(int pp, int maxPP) {
            moveLabel.text             = $"{move.name} - {move.pp}/{move.maxPP}";
            if (pp == 0) {
                moveLabel.color = Color.red;
            }
            else if (pp <= maxPP / 2) {
                moveLabel.color = Color.yellow;
            }
            else {
                moveLabel.color = Color.white;
            }
        }
    }
}