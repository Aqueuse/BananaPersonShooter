using Data.Bananas;
using UnityEngine;

namespace Building {
    public class BananaGunPut : MonoBehaviour {
        [SerializeField] private GameObject launchingBananaPoint;
        [SerializeField] private GenericDictionary<ItemType, GameObject> weaponsGameObjects;

        [SerializeField] private GameObject moverTarget;

        private GameObject banana;
        private BananasDataScriptableObject activeWeaponData;

        private void Update() {
            if (ObjectsReference.Instance.bananaMan.isGrabingBananaGun) {
                ObjectsReference.Instance.bananaGun.bananaGun.transform.LookAt(moverTarget.transform, Vector3.up);
            }
        }

        public void LoadingGun() {
            if (ObjectsReference.Instance.bananaMan.activeItemCategory == ItemCategory.BANANA) {
                ObjectsReference.Instance.bananaGun.GrabBananaGun();
                ObjectsReference.Instance.uiCrosshair.SetCrosshair(ItemCategory.BANANA, ItemType.CAVENDISH);

                Invoke(nameof(ThrowBanana), 0.3f);
            }
        }

        public void CancelThrow() {
            ObjectsReference.Instance.audioManager.StopAudioSource(AudioSourcesType.EFFECT);
            CancelInvoke(nameof(ThrowBanana));
        }

        public void ThrowBanana() {
            if (ObjectsReference.Instance.inventory.GetQuantity(ObjectsReference.Instance.bananaMan.activeItemType) <= 0) return;
            
            if (ObjectsReference.Instance.gameActions.leftClickActivated || ObjectsReference.Instance.gameActions.rightTriggerActivated) {
                banana = Instantiate(weaponsGameObjects[ObjectsReference.Instance.bananaMan.activeItem.itemType],
                    launchingBananaPoint.transform.position, Quaternion.identity, null);

                // Instantiate Banana of this type
                banana.transform.SetParent(null);

                // throw it with good speed forward the player
                banana.GetComponent<Rigidbody>().AddForce(launchingBananaPoint.transform.forward * 200, ForceMode.Impulse);
                ObjectsReference.Instance.audioManager.PlayEffect(EffectType.THROW_BANANA, 0);

                // ammo reduce
                activeWeaponData = ObjectsReference.Instance.bananaMan.activeItem;

                switch (activeWeaponData.bananaEffect) {
                    case BananaEffect.TWO_SPLIT:
                        ObjectsReference.Instance.inventory.RemoveQuantity(ItemCategory.BANANA, activeWeaponData.itemType, 2);
                        break;
                    case BananaEffect.FIVE_SPLIT:
                        ObjectsReference.Instance.inventory.RemoveQuantity(ItemCategory.BANANA, activeWeaponData.itemType, 5);
                        break;
                    default:
                        ObjectsReference.Instance.inventory.RemoveQuantity(ItemCategory.BANANA, activeWeaponData.itemType, 1);
                        break;
                }

                var newAmmoQuantity = ObjectsReference.Instance.inventory.bananaManInventory[activeWeaponData.itemType];

                ObjectsReference.Instance.uiSlotsManager.Get_Selected_Slot().SetAmmoQuantity(newAmmoQuantity);
                    
                Invoke(nameof(ThrowBanana), 1f);
            }
        }
    }
}