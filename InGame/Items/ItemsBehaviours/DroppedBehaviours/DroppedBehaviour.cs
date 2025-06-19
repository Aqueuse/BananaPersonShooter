using System;
using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using InGame.Items.ItemsProperties;
using InGame.Monkeys.Chimpvisitors;
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

            if (itemScriptableObject.droppedType != DroppedType.BLUEPRINT)
                Invoke(nameof(DestroyMe), 30);
        }

        private void FixedUpdate() {
            if (!ObjectsReference.Instance.aspireAction.isAspiring) return;
            
            if (Vector3.Distance(transform.position, bananaGunTransform.position) <= 100) {
                _rigidbody.velocity = 
                    (bananaGunTransform.position - 
                     transform.position) * 5;
            }

            if (Vector3.Distance(transform.position, bananaGunTransform.position) < 0.7f) {
                if (itemScriptableObject.droppedType == DroppedType.BLUEPRINT) {
                    BlueprintBehaviour blueprintBehaviour = (BlueprintBehaviour)this;
                    
                    ObjectsReference.Instance.bananaManBuildablesInventory.UnlockBuildablesTier(blueprintBehaviour.associatedBuildables);
                }

                else {
                    ObjectsReference.Instance.bananaMan.bananaManData.
                        inventoriesByDroppedType[itemScriptableObject.droppedType].
                        AddQuantity(itemScriptableObject, 1);

                    ObjectsReference.Instance.bottomSlots.RefreshSlotsQuantities();
                }
                
                Destroy(gameObject);
            }
        }
        
        private void OnTriggerEnter (Collider collider) {
            // TODO : make tourists and pirates flee the same way
            if (TagsManager.Instance.HasTag(collider.gameObject, GAME_OBJECT_TAG.VISITOR)) {
                collider.transform.GetComponent<VisitorBehaviour>().Flee();
            }

            if (TagsManager.Instance.HasTag(collider.gameObject, GAME_OBJECT_TAG.BUILDABLE)) {
                collider.gameObject.GetComponent<BuildableBehaviour>().TryAbsorbeRawMaterial(this);
            }
        }

        public void DestroyMe() {
            Destroy(gameObject);
        }

        public virtual void GenerateDroppedData() { }
        
        public virtual void LoadSavedData(string stringifiedJson) { }
    }
}
