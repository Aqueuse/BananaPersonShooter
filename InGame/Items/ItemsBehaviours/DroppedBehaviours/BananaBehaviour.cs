using InGame.Items.ItemsData.Dropped;
using InGame.Items.ItemsProperties.Dropped.Bananas;
using Newtonsoft.Json;
using Save.Helpers;

namespace InGame.Items.ItemsBehaviours.DroppedBehaviours {
    public class BananaBehaviour : DroppedBehaviour {
        public BananasPropertiesScriptableObject bananasPropertiesScriptableObject;
        
        public override void GenerateDroppedData() {
            var bananaData = new BananaData {
                droppedGuid = droppedGuid,
                droppedPosition = JsonHelper.FromVector3ToString(transform.position),
                droppedRotation = JsonHelper.FromQuaternionToString(transform.rotation),
                bananaType = itemScriptableObject.bananaType
            };
            
            ObjectsReference.Instance.gameSave.droppedBananaSave.AddBananaToBananaDictionnary(
                bananasPropertiesScriptableObject.bananaType, 
                JsonConvert.SerializeObject(bananaData)
            );
        }
        
        public override void LoadSavedData(string stringifiedJson) {
            var bananaData = JsonConvert.DeserializeObject<BananaData>(stringifiedJson);

            droppedGuid = bananaData.droppedGuid;
            transform.position = JsonHelper.FromStringToVector3( bananaData.droppedPosition);
            transform.rotation = JsonHelper.FromStringToQuaternion(bananaData.droppedRotation);
        }
    }
}
