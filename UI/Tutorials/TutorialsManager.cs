using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace UI.Tutorials {
    public class TutorialsManager : MonoBehaviour {
        [SerializeField] private List<UiTutorialButton> tutorialsButtons;

        public static void Show_Help() {
            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.ADVANCEMENTS, true);
        }

        public void Hide_Help() {
            ObjectsReference.Instance.gameManager.PauseGame(false);
            ObjectsReference.Instance.uiManager.Set_active(UICanvasGroupType.ADVANCEMENTS, false);
        }
        
        public void HideAllTutorials() {
            foreach (var tutorial in tutorialsButtons) {
                tutorial.HideTutorial();
            }
        }
    }
}
