using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Menus {
    public class UIOptionsTab : MonoBehaviour {
        [SerializeField] private GameObject firstElement;

        [SerializeField] private UICanvasGroupType _canvasGroupType;
        [SerializeField] private Image tabButtonImage;
        
        private Color activatedColor;
        private Color unactivatedColor;
        
        private void Start() {
            activatedColor = ObjectsReference.Instance.uiOptionsMenu.tabButtonActivatedColor;
            unactivatedColor = ObjectsReference.Instance.uiOptionsMenu.tabButtonUnactivatedColor;
        }

        public void Enable() {
            ObjectsReference.Instance.uiManager.Set_active(_canvasGroupType, true);
            tabButtonImage.color = activatedColor;
            tabButtonImage.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
            
            SelectFirstElement();
        }

        public void Disable() {
            ObjectsReference.Instance.uiManager.Set_active(_canvasGroupType, false);
            tabButtonImage.color = unactivatedColor;
            tabButtonImage.GetComponentInChildren<TextMeshProUGUI>().color = activatedColor;
        }
        
        private void SelectFirstElement() {
            EventSystem.current.SetSelectedGameObject(firstElement);
            firstElement.GetComponent<UIActionButton>().Select();
        }
    }
}
