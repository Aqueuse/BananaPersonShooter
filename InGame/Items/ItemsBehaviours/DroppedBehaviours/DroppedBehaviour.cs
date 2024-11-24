using System;
using InGame.Items.ItemsProperties.Dropped;
using InGame.Items.ItemsProperties.Dropped.Raw_Materials;
using InGame.Monkeys.Chimpirates;
using Tags;
using UnityEngine;

namespace InGame.Items.ItemsBehaviours.DroppedBehaviours {
    public class DroppedBehaviour : MonoBehaviour {
        public string droppedGuid;
        public DroppedPropertiesScriptableObject droppedPropertiesScriptableObject;

        private void Start() {
            if(string.IsNullOrEmpty(droppedGuid)) {
                droppedGuid = Guid.NewGuid().ToString();
            }

            Invoke(nameof(DestroyMe), 30);
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
