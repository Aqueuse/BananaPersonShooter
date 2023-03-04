using System.Collections.Generic;
using Game;
using UnityEngine;

namespace UI.Tutorials {
    public class TutorialsManager : MonoSingleton<TutorialsManager> {
        [SerializeField] private List<UiTutorialButton> tutorialsButtons;

        public void Show_Help() {
            UIManager.Instance.Set_active(GetComponent<CanvasGroup>(), true);
        }

        public void Hide_Help() {
            GameManager.Instance.PauseGame(false);
            UIManager.Instance.Set_active(GetComponent<CanvasGroup>(), false);
        }
        
        public void HideAllTutorials() {
            foreach (var tutorial in tutorialsButtons) {
                tutorial.HideTutorial();
            }
        }
    }
}
