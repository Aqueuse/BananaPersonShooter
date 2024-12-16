using InGame.Items.ItemsData.Dropped;
using Newtonsoft.Json;
using Save.Helpers;

namespace InGame.Items.ItemsBehaviours.DroppedBehaviours {
    public class ManufacturedItemBehaviour : DroppedBehaviour {
        private ManufacturedItemsType manufacturedItemsType;
        
        public override void GenerateDroppedData() {
            var manufacturedItemData = new ManufacturedItemData {
                droppedGuid = droppedGuid,
                droppedPosition = JsonHelper.FromVector3ToString(transform.position),
                droppedRotation = JsonHelper.FromQuaternionToString(transform.rotation),
                manufacturedItemsType = itemScriptableObject.manufacturedItemsType
            };

            ObjectsReference.Instance.gameSave.droppedManufacturedItemSave.AddManufacturedItemToDictionnary(
                manufacturedItemsType,
                JsonConvert.SerializeObject(manufacturedItemData));
        }
        
        public override void LoadSavedData(string stringifiedJson) {
            var manufacturedData = JsonConvert.DeserializeObject<ManufacturedItemData>(stringifiedJson);

            droppedGuid = manufacturedData.droppedGuid;
            manufacturedItemsType = manufacturedData.manufacturedItemsType;
            transform.position = JsonHelper.FromStringToVector3( manufacturedData.droppedPosition);
            transform.rotation = JsonHelper.FromStringToQuaternion(manufacturedData.droppedRotation);
        }
    }
}
