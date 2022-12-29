using Enums;
using UI.InGame;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public class Shoot : MonoBehaviour {
        [SerializeField] private GameObject launchingBananaPoint;
        [SerializeField] private GenericDictionary<ItemThrowableType, GameObject> weaponsGameObjects;

        public void ShootAction(InputAction.CallbackContext context) {
            var lastSelectedThrowableItem = BananaMan.Instance.activeItemThrowableType;
            
            if (context.performed && GameManager.Instance.isGamePlaying && lastSelectedThrowableItem != ItemThrowableType.PLATEFORM_CAVENDISH && lastSelectedThrowableItem != ItemThrowableType.ROCKET) {
                if (Inventory.Instance.GetQuantity(lastSelectedThrowableItem) > 0) {
                    // animation throw banana
                    BananaMan.Instance.tpsPlayerAnimator.ThrowAnimation();
                }
            }
        }
        
        public void ThrowBanana() {
            var banana = Instantiate(weaponsGameObjects[BananaMan.Instance.activeItem.itemThrowableType], null);

            // Instantiate Banana of this type
            // throw it with good speed forward the player
            banana.transform.position = launchingBananaPoint.transform.position;
            banana.GetComponent<Rigidbody>().AddForce(BananaMan.Instance.transform.forward*50, ForceMode.Impulse);
            AmmoReduce();
        }

        void AmmoReduce() {
            BananasDataScriptableObject activeWeaponData = BananaMan.Instance.activeItem;
            
            switch (activeWeaponData.bananaEffect) {
                case BananaEffect.TWO_SPLIT:
                    Inventory.Instance.RemoveQuantity(activeWeaponData.itemThrowableType, 2);
                    break;
                case BananaEffect.FIVE_SPLIT:
                    Inventory.Instance.RemoveQuantity(activeWeaponData.itemThrowableType, 5);
                    break;
                default:
                    Inventory.Instance.RemoveQuantity(activeWeaponData.itemThrowableType, 1);
                    break;
            }

            var newAmmoQuantity = Inventory.Instance.bananaManInventory[activeWeaponData.itemThrowableType]; 
            
            UISlotsManager.Instance.Get_Selected_Slot().SetAmmoQuantity(newAmmoQuantity.ToString());

        }
    }
}
