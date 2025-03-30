using System;
using InGame.Items.ItemsProperties;
using InGame.Monkeys.Chimpirates;
using Tags;
using UnityEngine;

namespace InGame.Items.ItemsBehaviours.DroppedBehaviours {
    public class DroppedBehaviour : MonoBehaviour {
        public string droppedGuid;
        public ItemScriptableObject itemScriptableObject;

        private Rigidbody _rigidbody;
        private Transform bananaGunTransform;
        
        private void Start() {
            _rigidbody = GetComponent<Rigidbody>();
            
            bananaGunTransform = ObjectsReference.Instance.bananaGun.bananaGunGameObject.transform; 
            
            if(string.IsNullOrEmpty(droppedGuid)) {
                droppedGuid = Guid.NewGuid().ToString();
            }

            if (ObjectsReference.Instance.bananaMan.tutorialFinished)
                Invoke(nameof(DestroyMe), 30);
        }

        private void FixedUpdate() {
            if (!ObjectsReference.Instance.shoot.isAspiring) return;
            
            if (Vector3.Distance(transform.position, bananaGunTransform.position) <= 100) {
                _rigidbody.velocity = 
                    (bananaGunTransform.position - 
                     transform.position) * 5;
            }

            if (Vector3.Distance(transform.position, bananaGunTransform.position) < 0.5f) {
                ObjectsReference.Instance.bananaMan.bananaManData.
                    inventoriesByDroppedType[itemScriptableObject.droppedType].
                    AddQuantity(itemScriptableObject, 1);

                ObjectsReference.Instance.uiFlippers.RefreshActiveDroppableQuantity();
                
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter (Collision collision) {
            // TODO : make tourists flee the same way
            if (TagsManager.Instance.HasTag(collision.gameObject, GAME_OBJECT_TAG.PIRATE)) {
                collision.transform.GetComponent<PirateBehaviour>().Flee();
            }
        }

        public void DestroyMe() {
            Destroy(gameObject);
        }

        public virtual void GenerateDroppedData() { }
        
        public virtual void LoadSavedData(string stringifiedJson) { }
    }
}
