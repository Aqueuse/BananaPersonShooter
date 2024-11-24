using InGame.Items.ItemsData.Dropped;
using Newtonsoft.Json;
using Save.Helpers;

namespace InGame.Items.ItemsBehaviours.DroppedBehaviours {
    public class RawMaterialBehaviour : DroppedBehaviour {
        private RawMaterialType rawMaterialType;
        
        public override void GenerateDroppedData() {
            var rawMaterialData = new RawMaterialData {
                droppedGuid = droppedGuid,
                droppedPosition = JsonHelper.FromVector3ToString(transform.position),
                droppedRotation = JsonHelper.FromQuaternionToString(transform.rotation),
                RawMaterialType = droppedPropertiesScriptableObject.rawMaterialType
            };
            
            ObjectsReference.Instance.gameSave.rawMaterialSave.AddRawMaterialToRawMaterialDictionnary(
                rawMaterialType,
                JsonConvert.SerializeObject(rawMaterialData));
        }
        
        public override void LoadSavedData(string stringifiedJson) {
            var rawMaterialData = JsonConvert.DeserializeObject<RawMaterialData>(stringifiedJson);

            droppedGuid = rawMaterialData.droppedGuid;
            rawMaterialType = rawMaterialData.RawMaterialType;
            transform.position = JsonHelper.FromStringToVector3( rawMaterialData.droppedPosition);
            transform.rotation = JsonHelper.FromStringToQuaternion(rawMaterialData.droppedRotation);
        }
    }
}
