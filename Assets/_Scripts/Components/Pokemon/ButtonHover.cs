using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace _Scripts.Pokemon {
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