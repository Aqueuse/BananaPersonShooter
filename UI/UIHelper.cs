using UnityEngine;

namespace UI {
    public class UIHelper : MonoBehaviour {
        [SerializeField] private CanvasGroup placeHelper;
        [SerializeField] private CanvasGroup buildHelper;

        [SerializeField] private CanvasGroup one_axe_buildHelper;

        [SerializeField] private CanvasGroup notEnoughMaterialHelper;

        public void ShowHelper() {
            GetComponent<CanvasGroup>().alpha = 1;
        }

        public void HideHelper() {
            GetComponent<CanvasGroup>().alpha = 0;
        }
        
        public void ShowDefaultHelper() {
            buildHelper.alpha = 0;
        }

        public void ShowBuildHelper() {
            buildHelper.alpha = 1;
            one_axe_buildHelper.alpha = 1;
        }
        
        public void ShowNormalPlaceHelper() {
            placeHelper.alpha = 1;
            notEnoughMaterialHelper.alpha = 0;
        }
    }
}
