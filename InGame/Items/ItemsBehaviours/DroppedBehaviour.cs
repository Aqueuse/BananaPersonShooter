using System;
using InGame.Items.ItemsData;
using Newtonsoft.Json;
using Save.Helpers;
using UnityEngine;

namespace InGame.Items.ItemsBehaviours {
    public class DroppedBehaviour : MonoBehaviour {
        public string droppedGuid;
        public DroppedType droppedType;
    
        private void Start() {
            if(string.IsNullOrEmpty(droppedGuid)) {
                droppedGuid = Guid.NewGuid().ToString();
            }
        }
        
        public void GenerateDroppedData() {
            var droppedData = new DroppedData {
                droppedGuid = droppedGuid,
                droppedPosition = JsonHelper.FromVector3ToString(transform.position),
                droppedRotation = JsonHelper.FromQuaternionToString(transform.rotation),
                droppedType = droppedType
            };
            
            ObjectsReference.Instance.gameSave.droppedSave.AddDroppedToDroppedDictionnary(JsonConvert.SerializeObject(droppedData));
        }
    }
}
