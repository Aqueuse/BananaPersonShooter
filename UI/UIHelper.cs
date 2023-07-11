using UnityEngine;

namespace UI {
    public class UIHelper : MonoBehaviour {
        [SerializeField] private CanvasGroup bananasHelper;
        [SerializeField] private CanvasGroup modeConstructionHelper;
        [SerializeField] private CanvasGroup retrieveConfirmationHelper;

        [SerializeField] private CanvasGroup buildHelper;
        [SerializeField] private CanvasGroup buildHelper_Y_axis;
        [SerializeField] private CanvasGroup buildHelper_Z_axis;
        
        public void show_default_helper() {
            retrieveConfirmationHelper.alpha = 0;
            buildHelper.alpha = 0;
            bananasHelper.alpha = 0;
            modeConstructionHelper.alpha = 1;
        }

        public void show_build_helper() {
            modeConstructionHelper.alpha = 0;
            bananasHelper.alpha = 0;
            if (ObjectsReference.Instance.bananaMan.activeItemCategory == ItemCategory.BUILDABLE) {
                buildHelper.alpha = 1;
            }
        }

        public void show_banana_helper() {
            retrieveConfirmationHelper.alpha = 0;
            buildHelper.alpha = 0;
            bananasHelper.alpha = 1;
            modeConstructionHelper.alpha = 0;
        }
        
        public void Show_Y_Axis() {
            buildHelper_Y_axis.alpha = 1;
            buildHelper_Z_axis.alpha = 0;
        }
        
        public void Show_Z_Axis() {
            buildHelper_Y_axis.alpha = 0;
            buildHelper_Z_axis.alpha = 1;
        }
        
        public void Show_retrieve_confirmation() {
            retrieveConfirmationHelper.alpha = 1;
        }

        public void Hide_retrieve_confirmation() {
            retrieveConfirmationHelper.alpha = 0;
        }
    }
}
