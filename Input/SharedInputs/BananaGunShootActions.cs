using InGame.Player.BananaGunActions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SharedInputs {
    public class BananaGunShootActions : InputActions {
        [SerializeField] private Shoot shoot;

        [SerializeField] private InputActionReference shootBananaActionReference;
        [SerializeField] private InputActionReference openBananaSelectorActionReference;

        private void OnEnable() {
            shootBananaActionReference.action.Enable();
            shootBananaActionReference.action.performed += ShootBanana;
            shootBananaActionReference.action.canceled += CancelShoot;

            openBananaSelectorActionReference.action.Enable();
            openBananaSelectorActionReference.action.performed += OpenBananaSelector;
            openBananaSelectorActionReference.action.canceled += CloseBananaSelector;
        }

        private void OnDisable() {
            shootBananaActionReference.action.Disable();
            shootBananaActionReference.action.performed -= ShootBanana;
            shootBananaActionReference.action.canceled -= CancelShoot;

            openBananaSelectorActionReference.action.Disable();
            openBananaSelectorActionReference.action.performed -= OpenBananaSelector;
            openBananaSelectorActionReference.action.canceled -= CloseBananaSelector;
        }

        private void ShootBanana(InputAction.CallbackContext callbackContext) {
            ObjectsReference.Instance.bananaGun.GrabBananaGun();
            if (ObjectsReference.Instance.bananasInventory.GetQuantity(ObjectsReference.Instance.bananaMan.bananaManData.activeBanana.bananaType) <= 0) return;
    

            shoot.LoadingGun();
            ObjectsReference.Instance.uiCrosshairs.SetCrosshair(ObjectsReference.Instance.bananaMan.bananaManData.activeBanana.bananaType);
        }

        private void CancelShoot(InputAction.CallbackContext callbackContext) {
            ObjectsReference.Instance.bananaGun.UngrabBananaGun();
            shoot.CancelThrow();
            ObjectsReference.Instance.uiCrosshairs.SetCrosshair(BananaType.EMPTY);
        }
    
        private static void OpenBananaSelector(InputAction.CallbackContext callbackContext) {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            
            ObjectsReference.Instance.inputManager.AlsoActivateUIinputActions();
            
            ObjectsReference.Instance.cameraPlayer.Set0Sensibility();
            ObjectsReference.Instance.playerController.canMove = false;
            ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = true;

            Time.timeScale = 0.01f;
            ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_BANANA_SELECTOR;

            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.BANANA_SELECTOR, true);
        }
    
        private static void CloseBananaSelector(InputAction.CallbackContext callbackContext) {
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.BANANA_SELECTOR, false);

            ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_GAME;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            ObjectsReference.Instance.cameraPlayer.SetNormalSensibility();
            ObjectsReference.Instance.playerController.canMove = true;
            ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = false;
        
            Time.timeScale = 1f;
        }
    }
}