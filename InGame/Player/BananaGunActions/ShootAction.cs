using System;
using UnityEngine;

namespace InGame.Player.BananaGunActions {
    public class ShootAction : MonoBehaviour {
        [SerializeField] private Transform launchingPoint;
        
        private GameObject droppable;
        
        private GenericDictionary<RawMaterialType, int> rawMaterialsWithQuantity;

        public void LoadingGun() {
            InvokeRepeating(nameof(Throw), 0.3f, 0.3f);
        }

        public void CancelThrow() {
            ObjectsReference.Instance.audioManager.StopAudioSource(AudioSourcesType.EFFECT);
            CancelInvoke(nameof(Throw));
        }

        public void Throw() {
            droppable = Instantiate(
                ObjectsReference.Instance.meshReferenceScriptableObject.GetActiveDroppablePrefab(), 
                launchingPoint.transform.position, 
                Quaternion.identity,
                ObjectsReference.Instance.gameSave.savablesItemsContainer);
            
            droppable.GetComponent<Rigidbody>().AddForce(launchingPoint.transform.forward * 10000, ForceMode.Force);
            
            ObjectsReference.Instance.bananaMan.bananaManData.RemoveActiveSlotItemQuantity(1);

            ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.THROW_BANANA, 0);
            
            ObjectsReference.Instance.bottomSlots.RefreshSlotsQuantities();
        }
        
    }
}