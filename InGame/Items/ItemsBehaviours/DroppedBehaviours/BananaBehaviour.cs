using InGame.Items.ItemsData.Dropped;
using Newtonsoft.Json;
using Save.Helpers;

namespace InGame.Items.ItemsBehaviours.DroppedBehaviours {
    public class BananaBehaviour : DroppedBehaviour {
        private BananaType bananaType;
        
        public override void GenerateDroppedData() {
            var bananaData = new BananaData {
                droppedGuid = droppedGuid,
                droppedPosition = JsonHelper.FromVector3ToString(transform.position),
                droppedRotation = JsonHelper.FromQuaternionToString(transform.rotation),
                bananaType = droppedPropertiesScriptableObject.bananaType
            };
            
            ObjectsReference.Instance.gameSave.bananaSave.AddBananaToBananaDictionnary(
                bananaType, 
                JsonConvert.SerializeObject(bananaData)
            );
        }
        
        public override void LoadSavedData(string stringifiedJson) {
            var bananaData = JsonConvert.DeserializeObject<BananaData>(stringifiedJson);

            droppedGuid = bananaData.droppedGuid;
            bananaType = bananaData.bananaType;
            transform.position = JsonHelper.FromStringToVector3( bananaData.droppedPosition);
            transform.rotation = JsonHelper.FromStringToQuaternion(bananaData.droppedRotation);
        }
    }
}
