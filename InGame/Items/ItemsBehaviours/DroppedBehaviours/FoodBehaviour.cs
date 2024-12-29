using InGame.Items.ItemsData.Dropped;
using Newtonsoft.Json;
using Save.Helpers;

namespace InGame.Items.ItemsBehaviours.DroppedBehaviours {
    public class FoodBehaviour : DroppedBehaviour {
        public override void GenerateDroppedData() {
            var foodData = new FoodData {
                droppedGuid = droppedGuid,
                droppedPosition = JsonHelper.FromVector3ToString(transform.position),
                droppedRotation = JsonHelper.FromQuaternionToString(transform.rotation),
                foodType = itemScriptableObject.foodType
            };
            
            ObjectsReference.Instance.gameSave.droppedFoodSave.AddFoodToFoodDictionnary(
                itemScriptableObject.foodType,
                JsonConvert.SerializeObject(foodData));
        }
        
        public override void LoadSavedData(string stringifiedJson) {
            var foodData = JsonConvert.DeserializeObject<FoodData>(stringifiedJson);

            droppedGuid = foodData.droppedGuid;
            transform.position = JsonHelper.FromStringToVector3( foodData.droppedPosition);
            transform.rotation = JsonHelper.FromStringToQuaternion(foodData.droppedRotation);
        }
    }
}
