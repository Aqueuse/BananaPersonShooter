using InGame.Items.ItemsProperties.Dropped.Raw_Materials;
using UnityEngine;

namespace InGame.Player.BananaGunActions {
    public class Shoot : MonoBehaviour {
        [SerializeField] private Transform launchingPoint;
        
        private GameObject droppable;
        private DroppedPropertiesScriptableObject activeWeaponData;
        
        public void LoadingGun() {
            Invoke(nameof(Throw), 0.3f);
        }

        public void CancelThrow() {
            ObjectsReference.Instance.audioManager.StopAudioSource(AudioSourcesType.EFFECT);
            CancelInvoke(nameof(Throw));
        }

        public void Throw() {
            droppable = Instantiate(ObjectsReference.Instance.meshReferenceScriptableObject.GetActiveDroppablePrefab(), launchingPoint.transform.position, Quaternion.identity);

            droppable.transform.position = launchingPoint.transform.position;
            droppable.SetActive(true);

            droppable.GetComponent<Rigidbody>().isKinematic = false;
            droppable.GetComponent<Rigidbody>().AddForce(launchingPoint.transform.forward * 10000, ForceMode.Force);

            ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.THROW_BANANA, 0);

            // ammo reduce
            ObjectsReference.Instance.uiFlippers.SetDroppableQuantity(ObjectsReference.Instance.inventoriesHelper.RemoveActiveDroppedQuantity().ToString());
        }
    }
}