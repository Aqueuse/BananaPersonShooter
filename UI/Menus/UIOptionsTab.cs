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
        
        private Color activatedTabColor;
        private Color unactivatedTabColor;
        
        private UIOptionsMenu uiOptionsMenu;
        
        private void Start() {
            uiOptionsMenu = ObjectsReference.Instance.uiOptionsMenu;
            activatedTabColor = uiOptionsMenu.tabButtonActivatedColor;
            unactivatedTabColor = uiOptionsMenu.tabButtonUnactivatedColor;
        }

        public void Enable() {
            ObjectsReference.Instance.uiManager.SetActive(_canvasGroupType, true);
            tabButtonImage.color = activatedTabColor;
            tabButtonImage.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
            
            SelectFirstElement();
        }

        public void Disable() {
            ObjectsReference.Instance.uiManager.SetActive(_canvasGroupType, false);
            tabButtonImage.color = unactivatedTabColor;
            tabButtonImage.GetComponentInChildren<TextMeshProUGUI>().color = activatedTabColor;
        }
        
        private void SelectFirstElement() {
            EventSystem.current.SetSelectedGameObject(firstElement);
            firstElement.GetComponent<UIActionButton>().Select();
        }
    }
}
