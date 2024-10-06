using System;
using InGame.Items.ItemsData;
using Newtonsoft.Json;
using Save.Helpers;
using UnityEngine;

namespace InGame.Items.ItemsBehaviours {
    public class SpaceshipDebrisBehaviour : MonoBehaviour {
        public string spaceshipDebrisGuid;
        public int spaceshipDebrisPrefabIndex;
        public CharacterType characterType;
        public bool isInSpace;
    
        private void Start() {
            if(string.IsNullOrEmpty(spaceshipDebrisGuid)) {
                spaceshipDebrisGuid = Guid.NewGuid().ToString();
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

        public void GenerateSpaceshipDebrisData() {
            var spaceshipDebrisData = new SpaceshipDebrisData {
                droppedGuid = spaceshipDebrisGuid,
                spaceshipDebrisPosition = JsonHelper.FromVector3ToString(transform.position),
                spaceshipDebrisRotation = JsonHelper.FromQuaternionToString(transform.rotation),
                prefabIndex = spaceshipDebrisPrefabIndex,
                characterType = characterType,
                isInSpace = isInSpace
            };
            
            ObjectsReference.Instance.gameSave.spaceshipDebrisSave.AddSpaceshipDebrisToSpaceshipDebrisDictionnary(characterType, JsonConvert.SerializeObject(spaceshipDebrisData));
        }
    }
}
