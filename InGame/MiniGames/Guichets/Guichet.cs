using System.Collections.Generic;
using InGame.Monkeys.Chimpvisitors;
using UnityEngine;

namespace InGame.MiniGames.Guichets {
    public class Guichet : MonoBehaviour {
        [SerializeField] private Transform barrierTransform;
        
        [SerializeField] private Transform cameraFollow;
        [SerializeField] private Transform cameraLookAt;

        public bool isOpen;
        
        public List<VisitorBehaviour> visitorsToWatch;
        public Transform mapPointOfInterestCenter;
        
        [SerializeField] private Transform waitingListStart;

        public void OpenGuichet() {
            // change camera look at and follow to cameraTransform
            ObjectsReference.Instance.uiInGameVirtualCamera.Follow = cameraFollow;
            ObjectsReference.Instance.uiInGameVirtualCamera.LookAt = cameraLookAt;
            ObjectsReference.Instance.uiInGameVirtualCamera.Priority = 200;

            ObjectsReference.Instance.cameraPlayer.Set0Sensibility();
            ObjectsReference.Instance.playerController.canMove = false;
            
            ObjectsReference.Instance.bananaGunActionsSwitch.gameObject.SetActive(false);
            
            ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_GAME_UI_PANEL;

            ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CROSSHAIRS].alpha = 0;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            
            // switch to UI (desactive banana man etc)
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.UI);
            
            // show Guichet UI
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.GUICHET, true);
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD_BANANAMAN, false);
            
            isOpen = true;
            
            // open barrier
            barrierTransform.localRotation = Quaternion.Euler(90f, 0, 135f);
        }

        public void CloseGuichet() {
            // change camera look at and follow to bananaMan camera transform
            ObjectsReference.Instance.uiInGameVirtualCamera.Follow = cameraFollow;
            ObjectsReference.Instance.uiInGameVirtualCamera.LookAt = cameraLookAt;
            ObjectsReference.Instance.uiInGameVirtualCamera.Priority = 0;

            ObjectsReference.Instance.cameraPlayer.SetNormalSensibility();
            ObjectsReference.Instance.playerController.canMove = true;

            ObjectsReference.Instance.bananaGunActionsSwitch.gameObject.SetActive(true);

            ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_GAME;

            ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CROSSHAIRS].alpha = 1;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            // switch to banana man
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);

            // hide Guichet UI
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.GUICHET, false);
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD_BANANAMAN, true);

            isOpen = false;
            
            // close barrier
            barrierTransform.localRotation = Quaternion.Euler(0, 0, 135f);
        }
        
        public Stack<Vector3> GiveTwoRandomPointsOfInterest() {
            mapPointOfInterestCenter.rotation = Quaternion.Euler(0, 0, 0);
            
            List<Vector3> points = new List<Vector3>();
            
            for (int i = 0; i < 2; i++) {
                mapPointOfInterestCenter.Rotate(Vector3.left, Random.Range(0, 270));
                points.Add(mapPointOfInterestCenter.position + Vector3.forward * 15);
            }

            return new Stack<Vector3>(points);
        }
        
        public void SellTicket() {
            
        }

        public void Scan() {
            
        }

        public void TryToRecrute() {
            
        }
    }
}