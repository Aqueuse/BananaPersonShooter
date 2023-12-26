using System;
using Data;
using Newtonsoft.Json;
using Save.Helpers;
using UnityEngine;

namespace Gestion {
    public class DebrisBehaviour : MonoBehaviour {
        public string debrisGuid;
        public int debrisPrefabIndex;
        public CharacterType characterType;
    
        private void Start() {
            if(string.IsNullOrEmpty(debrisGuid)) {
                debrisGuid = Guid.NewGuid().ToString();
            }
        }
        
        public void GenerateDebrisData() {
            var debrisData = new DebrisData {
                debrisGuid = debrisGuid,
                debrisPosition = JsonHelper.FromVector3ToString(transform.position),
                debrisRotation = JsonHelper.FromQuaternionToString(transform.rotation),
                prefabIndex = debrisPrefabIndex,
                characterType = characterType
            };
            
            ObjectsReference.Instance.gameData.currentMapData.AddDebrisToDebrisDictionnary(characterType, JsonConvert.SerializeObject(debrisData));
        }
    }
}
