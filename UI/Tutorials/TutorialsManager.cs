using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace UI.Tutorials {
    public class TutorialsManager : MonoBehaviour {
        [SerializeField] private List<UiTutorialButton> tutorialsButtons;

        public void Show_Help() {
            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.TUTORIALS, true);
        }

        public void Hide_Help() {
            ObjectsReference.Instance.gameManager.PauseGame(false);
            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.TUTORIALS, false);
        }
        
        public void HideAllTutorials() {
            foreach (var tutorial in tutorialsButtons) {
                tutorial.HideTutorial();
            }
        }
    }
}
