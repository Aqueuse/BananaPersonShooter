using Data.Bananas;
using UnityEngine;

namespace Gestion.Actions {
    public class ThrowBanana : MonoBehaviour {
        [SerializeField] private Transform launchingBananaPoint;
        [SerializeField] private GenericDictionary<BananaType, GameObject> weaponsGameObjects;

        private GameObject banana;
        private BananasDataScriptableObject activeWeaponData;
        
        public void LoadingGun() {
            Invoke(nameof(throwBanana), 0.3f);
        }

        public void CancelThrow() {
            ObjectsReference.Instance.audioManager.StopAudioSource(AudioSourcesType.EFFECT);
            CancelInvoke(nameof(throwBanana));
        }

        public void throwBanana() {
            banana = Instantiate(weaponsGameObjects[ObjectsReference.Instance.bananaMan.activeItem.bananaType],
                launchingBananaPoint.transform.position, Quaternion.identity, null);

            // Instantiate Banana of this type
            banana.transform.SetParent(null);

            // throw it with good speed forward the player
            banana.GetComponent<Rigidbody>().AddForce(launchingBananaPoint.transform.forward * 100, ForceMode.Impulse);
            banana.GetComponent<Rigidbody>().AddForce(transform.forward * 100, ForceMode.Impulse);
            ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.THROW_BANANA, 0);

            // ammo reduce
            activeWeaponData = ObjectsReference.Instance.bananaMan.activeItem;

            switch (activeWeaponData.bananaEffect) {
                case BananaEffect.TWO_SPLIT:
                    ObjectsReference.Instance.bananasInventory.RemoveQuantity(activeWeaponData.bananaType, 2);
                    break;
                case BananaEffect.FIVE_SPLIT:
                    ObjectsReference.Instance.bananasInventory.RemoveQuantity(activeWeaponData.bananaType, 5);
                    break;
                default:
                    ObjectsReference.Instance.bananasInventory.RemoveQuantity(activeWeaponData.bananaType, 1);
                    break;
            }

            var newAmmoQuantity = ObjectsReference.Instance.bananasInventory.bananasInventory[activeWeaponData.bananaType];

            ObjectsReference.Instance.quickSlotsManager.SetBananaQuantity(newAmmoQuantity);

            Invoke(nameof(throwBanana), 1f);
        }
    }
}