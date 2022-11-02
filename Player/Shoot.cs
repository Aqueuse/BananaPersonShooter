using UI.InGame;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapons;

namespace Player {
    public class Shoot : MonoBehaviour {
        [SerializeField] private GameObject launchingBananaPoint;

        private PlayerController _bananaManPlayerController;

        private void Start() {
            _bananaManPlayerController = BananaMan.Instance.GetComponent<PlayerController>();
        }

        public void ShootAction(InputAction.CallbackContext context) {
            var lastSelectedBanana = BananaMan.Instance.activeBananaType;
            
            if (context.performed && GameManager.Instance.isPlaying && lastSelectedBanana != BananaType.EMPTY_HAND) {
                if (Inventory.Instance.GetQuantity(lastSelectedBanana) > 0) {
                    // animation throw banana
                    _bananaManPlayerController.RotateForward();
                    BananaMan.Instance.tpsPlayerAnimator.ThrowAnimation();
                }
                else {
                    // animation don't find banana
                    BananaMan.Instance.tpsPlayerAnimator.BananaNotFound();
                }
            }
        }
        
        public void ThrowBanana() {
            var banana = Instantiate(BananasManager.Instance.WeaponsGameObjects[BananaMan.Instance.activeBanana.bananaType], null);

            // Instantiate Banana of this type
            // throw it with good speed forward the player
            banana.transform.position = launchingBananaPoint.transform.position;
            banana.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward*50, ForceMode.Impulse);
            AmmoReduce();
        }

        void AmmoReduce() {
            BananasDataScriptableObject activeWeaponData = BananaMan.Instance.activeBanana;
            
            switch (activeWeaponData.bananaEffect) {
                case BananaEffect.TWO_SPLIT:
                    Inventory.Instance.RemoveQuantity(activeWeaponData.bananaType, 2);
                    break;
                case BananaEffect.FIVE_SPLIT:
                    Inventory.Instance.RemoveQuantity(activeWeaponData.bananaType, 5);
                    break;
                default:
                    Inventory.Instance.RemoveQuantity(activeWeaponData.bananaType, 1);
                    break;
            }

            var newAmmoQuantity = Inventory.Instance.BananaManInventory[activeWeaponData.bananaType]; 
            
            UISlotsManager.Instance.Get_Selected_Slot().SetAmmoQuantity(newAmmoQuantity.ToString());

        }
    }
}
