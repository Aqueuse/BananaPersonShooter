using Cameras;
using Enums;
using Game;
using Input;
using MiniChimps;
using Player;
using UI;
using UnityEngine;

namespace Dialogues {
    public class DialoguesManager : MonoSingleton<DialoguesManager> {
        public GameObject dialoguePanel;
        
        public GameObject activeMiniChimp;

        public void SetActiveMiniChimp(GameObject activeMiniChimpGameObject) {
            activeMiniChimp = activeMiniChimpGameObject;
        }
        
        public void StartDialogue() {
            UIManager.Instance.Hide_menus();
            BananaMan.Instance.GetComponent<PlayerController>().canMove = false;

            GameManager.Instance.isGamePlaying = false;

            InputManager.Instance.uiSchemaContext = UISchemaSwitchType.DIALOGUES;
            InputManager.Instance.SwitchContext(InputContext.UI);
            
            GameManager.Instance.gameContext = GameContext.IN_DIALOGUE;
            dialoguePanel.SetActive(true);
        }

        public void PlayMiniChimpDialogue(int index) {
            activeMiniChimp.GetComponent<MiniChimp>().Speak(index);
        }

        public void QuitDialogue() {
            InputManager.Instance.SwitchContext(InputContext.GAME);
            MainCamera.Instance.SwitchToFreeLookCamera();
            BananaMan.Instance.GetComponent<PlayerController>().canMove = true;

            dialoguePanel.SetActive(false);

            GameManager.Instance.isGamePlaying = true;
            GameManager.Instance.gameContext = GameContext.IN_GAME;
        }
        
        public MiniChimpType GetActiveMiniChimpType() {
            return activeMiniChimp.GetComponent<MiniChimp>().miniChimpType;
        }
    }
}
