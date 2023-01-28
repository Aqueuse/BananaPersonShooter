using Audio;
using Enums;
using Input;
using Player;
using UI.InGame;
using UnityEngine;

namespace Building {
    public class MoverPut : MonoSingleton<MoverPut> {
        [SerializeField] private GameObject launchingBananaPoint;
        [SerializeField] private GenericDictionary<ItemThrowableType, GameObject> weaponsGameObjects;

        public void LoadingGun() {
            var lastSelectedThrowableCategory = BananaMan.Instance.activeItemThrowableCategory;

            Mover.Instance.GrabMover();

            if (lastSelectedThrowableCategory == ItemThrowableCategory.BANANA) {
                AudioManager.Instance.PlayEffect(EffectType.LOADING_GUN_PUT);
                Invoke(nameof(ThrowBanana), 1f);
            }
        }
        
        public void ThrowBanana() {
            if (Inventory.Instance.GetQuantity(BananaMan.Instance.activeItemThrowableType) > 0) {
                if (GameActions.Instance.leftClickActivated || GameActions.Instance.rightTriggerActivated) {
                    var banana = Instantiate(weaponsGameObjects[BananaMan.Instance.activeItem.itemThrowableType],
                        launchingBananaPoint.transform.position, Quaternion.identity, null);

                    // Instantiate Banana of this type
                    banana.transform.SetParent(null);

                    // throw it with good speed forward the player
                    banana.GetComponent<Rigidbody>().AddForce(BananaMan.Instance.transform.forward * 100, ForceMode.Impulse);
                    AudioManager.Instance.PlayEffect(EffectType.THROW_BANANA);
                    AmmoReduce();
                }
            }
        }

        private void AmmoReduce() {
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

            UISlotsManager.Instance.Get_Selected_Slot().SetAmmoQuantity(newAmmoQuantity);
        }
    }
}