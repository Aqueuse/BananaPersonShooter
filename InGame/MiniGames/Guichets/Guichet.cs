using System.Collections.Generic;
using InGame.Monkeys.Chimpvisitors;
using UnityEngine;

namespace InGame.MiniGames.Guichets {
    public class Guichet : MonoBehaviour {
        public Transform waitingListStart;
        [SerializeField] private Transform barrierTransform;
        
        [SerializeField] private Transform cameraFollow;
        [SerializeField] private Transform cameraLookAt;

        public bool isOpen;
        
        public List<VisitorBehaviour> visitorsToWatch;
        

        public void OpenGuichet() {
            // change camera look at and follow to cameraTransform
            ObjectsReference.Instance.uiInGameVirtualCamera.transform.position = cameraFollow.position;
            ObjectsReference.Instance.uiInGameVirtualCamera.transform.rotation = cameraLookAt.rotation;
            ObjectsReference.Instance.uiInGameVirtualCamera.enabled = true;
            
            ObjectsReference.Instance.gameManager.gameContext = GameContext.GUICHET;
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GUICHET);
            
            // show Guichet UI
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.GUICHET, true);
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD_BANANAMAN, false);
            
            isOpen = true;
            
            // open barrier
            barrierTransform.localRotation = Quaternion.Euler(90f, 0, 135f);
        }

        public void CloseGuichet() {
            // change camera look at and follow to bananaMan camera transform
            ObjectsReference.Instance.uiInGameVirtualCamera.enabled = false;
            
            ObjectsReference.Instance.gameManager.gameContext = GameContext.BANANAMAN_CONTROL;
            ObjectsReference.Instance.bananaMan.SetToPlayable();

            // switch to banana man
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);

            // hide Guichet UI
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.GUICHET, false);
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD_BANANAMAN, true);

            isOpen = false;
            
            // close barrier
            barrierTransform.localRotation = Quaternion.Euler(0, 0, 135f);
        }
        
        public void SellTicket() {
            
        }

        public void Scan() {
            
        }

        public void TryToRecruit() {
            
        }
    }
}