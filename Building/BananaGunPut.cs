using Audio;
using Enums;
using Game;
using Input;
using Player;
using UI.InGame;
using UI.InGame.QuickSlots;
using UnityEngine;

namespace Building {
    public class BananaGunPut : MonoSingleton<BananaGunPut> {
        [SerializeField] private GameObject launchingBananaPoint;
        [SerializeField] private GenericDictionary<ItemThrowableType, GameObject> weaponsGameObjects;

        [SerializeField] private GameObject moverTarget;

        private void Update() {
            if (BananaMan.Instance.isGrabingBananaGun) {
                BananaGun.Instance.bananaGun.transform.LookAt(moverTarget.transform, Vector3.up);
            }
        }

        public void LoadingGun() {
            var lastSelectedThrowableCategory = BananaMan.Instance.activeItemThrowableCategory;

            if (lastSelectedThrowableCategory == ItemThrowableCategory.BANANA) {
                BananaGun.Instance.GrabBananaGun();
                UICrosshair.Instance.SetCrosshair(ItemThrowableType.CAVENDISH);
                UICrosshair.Instance.ShowHideCrosshairs(true);

                Invoke(nameof(ThrowBanana), 0.3f);
            }
        }

        public void CancelThrow() {
            AudioManager.Instance.StopAudioSource(AudioSourcesType.EFFECT);
            CancelInvoke(nameof(ThrowBanana));
            UICrosshair.Instance.ShowHideCrosshairs(false);
        }

        public void ThrowBanana() {
            if (Inventory.Instance.GetQuantity(BananaMan.Instance.activeItemThrowableType) > 0) {
                if (GameActions.Instance.leftClickActivated || GameActions.Instance.rightTriggerActivated) {
                    var banana = Instantiate(weaponsGameObjects[BananaMan.Instance.activeItem.itemThrowableType],
                        launchingBananaPoint.transform.position, Quaternion.identity, null);

                    // Instantiate Banana of this type
                    banana.transform.SetParent(null);

                    // throw it with good speed forward the player
                    banana.GetComponent<Rigidbody>().AddForce(GameManager.Instance.cameraMain.transform.forward * 200, ForceMode.Impulse);
                    AudioManager.Instance.PlayEffect(EffectType.THROW_BANANA, 0);
                    AmmoReduce();
                    
                    Invoke(nameof(ThrowBanana), 1f);
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