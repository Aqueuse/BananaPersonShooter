using InGame.Inventory;
using InGame.Items.ItemsProperties.Bananas;
using UnityEngine;

namespace InGame.Player.PlayerActions {
    public class ThrowBanana : MonoBehaviour {
        [SerializeField] private Transform launchingBananaPoint;
        [SerializeField] private GenericDictionary<BananaType, GameObject> weaponsGameObjects;
        
        private GameObject banana;
        private BananasPropertiesScriptableObject activeWeaponData;

        private BananasInventory bananasInventory;

        private void Start() {
            bananasInventory = ObjectsReference.Instance.bananasInventory;
        }

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

            banana.GetComponent<Rigidbody>().AddForce(launchingBananaPoint.transform.forward * 100, ForceMode.Impulse);
            banana.GetComponent<Rigidbody>().AddForce(transform.forward * 100, ForceMode.Impulse);
            
            ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.THROW_BANANA, 0);

            // ammo reduce
            activeWeaponData = ObjectsReference.Instance.bananaMan.activeItem;

            switch (activeWeaponData.bananaEffect) {
                case BananaEffect.TWO_SPLIT:
                    bananasInventory.RemoveQuantity(activeWeaponData.bananaType, 2);
                    break;
                case BananaEffect.FIVE_SPLIT:
                    bananasInventory.RemoveQuantity(activeWeaponData.bananaType, 5);
                    break;
                default:
                    bananasInventory.RemoveQuantity(activeWeaponData.bananaType, 1);
                    break;
            }
            
            Invoke(nameof(throwBanana), 1f);
        }
    }
}