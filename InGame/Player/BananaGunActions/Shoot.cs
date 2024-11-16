using InGame.Inventory;
using InGame.Items.ItemsProperties.Bananas;
using UnityEngine;

namespace InGame.Player.BananaGunActions {
    public class Shoot : MonoBehaviour {
        [SerializeField] private Transform launchingBananaPoint;
        [SerializeField] private GameObject bananaPrefab;
        
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
            banana = Instantiate(bananaPrefab, launchingBananaPoint.transform.position, Quaternion.identity);

            banana.transform.position = launchingBananaPoint.transform.position;
            banana.SetActive(true);
            
            banana.GetComponent<Rigidbody>().isKinematic = false;
            banana.GetComponent<Rigidbody>().AddForce(launchingBananaPoint.transform.forward * 10000, ForceMode.Force);

            ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.THROW_BANANA, 0);

            // ammo reduce
            activeWeaponData = ObjectsReference.Instance.bananaMan.bananaManData.activeBanana;

            bananasInventory.RemoveQuantity(activeWeaponData.bananaType, 1);
        }
    }
}