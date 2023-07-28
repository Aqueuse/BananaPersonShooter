using Enums;
using UnityEngine;

namespace UI {
    public class UIHelper : MonoBehaviour {
        [SerializeField] private CanvasGroup bananasHelper;
        [SerializeField] private CanvasGroup modeConstructionHelper;
        [SerializeField] private CanvasGroup retrieveConfirmationHelper;
        [SerializeField] private CanvasGroup placeHelper;
        [SerializeField] private CanvasGroup buildHelper;
        
        [SerializeField] private CanvasGroup complete_buildHelper;
        [SerializeField] private CanvasGroup one_axe_buildHelper;

        [SerializeField] private CanvasGroup notEnoughMaterialHelper;

        public void show_default_helper() {
            modeConstructionHelper.alpha = 1;
            bananasHelper.alpha = 0;
            buildHelper.alpha = 0;
            retrieveConfirmationHelper.alpha = 0;
        }

        public void show_build_helper() {
            modeConstructionHelper.alpha = 0;
            bananasHelper.alpha = 0;
            buildHelper.alpha = 1;

            if (ObjectsReference.Instance.bananaMan.activeBuildableType == BuildableType.PLATEFORM) {
                complete_buildHelper.alpha = 1;
                one_axe_buildHelper.alpha = 0;
            }

            else {
                complete_buildHelper.alpha = 0;
                one_axe_buildHelper.alpha = 1;
            }
        }

        public void show_banana_helper() {
            retrieveConfirmationHelper.alpha = 0;
            complete_buildHelper.alpha = 0;
            bananasHelper.alpha = 1;
            modeConstructionHelper.alpha = 0;
            buildHelper.alpha = 0;
        }

        public void Show_retrieve_confirmation() {
            retrieveConfirmationHelper.alpha = 1;
        }

        public void Hide_retrieve_confirmation() {
            retrieveConfirmationHelper.alpha = 0;
        }

        public void ShowNetEnoughMaterialsHelper() {
            placeHelper.alpha = 0;
            notEnoughMaterialHelper.alpha = 1;
        }

        public void ShowNormalPlaceHelper() {
            placeHelper.alpha = 1;
            notEnoughMaterialHelper.alpha = 0;
        }
    }
}
