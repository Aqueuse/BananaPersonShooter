using System;
using InGame.Items.ItemsData;
using Newtonsoft.Json;
using Save.Helpers;
using UnityEngine;

namespace InGame.Items.ItemsBehaviours {
    public class DebrisBehaviour : MonoBehaviour {
        public string debrisGuid;
        public int debrisPrefabIndex;
        public CharacterType characterType;
        public bool isInSpace;
    
        private void Start() {
            if(string.IsNullOrEmpty(debrisGuid)) {
                debrisGuid = Guid.NewGuid().ToString();
            }
        }

        public void DestroyIfUnreachable() {
            Invoke(nameof(DestroyMe), 60);
        }

        private void DestroyMe() {
            Destroy(gameObject);
        }
        
        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.layer == 11 & isInSpace) {
                Destroy(GetComponent<Rigidbody>());
                isInSpace = false;
            }
        }

        public void GenerateDebrisData() {
            var debrisData = new DebrisData {
                debrisGuid = debrisGuid,
                debrisPosition = JsonHelper.FromVector3ToString(transform.position),
                debrisRotation = JsonHelper.FromQuaternionToString(transform.rotation),
                prefabIndex = debrisPrefabIndex,
                characterType = characterType,
                isInSpace = isInSpace
            };
            
            ObjectsReference.Instance.gameSave.debrisSave.AddDebrisToDebrisDictionnary(characterType, JsonConvert.SerializeObject(debrisData));
        }
    }
}
