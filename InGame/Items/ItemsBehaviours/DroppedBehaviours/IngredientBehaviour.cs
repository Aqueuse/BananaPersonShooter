using InGame.Items.ItemsData.Dropped;
using Newtonsoft.Json;
using Save.Helpers;

namespace InGame.Items.ItemsBehaviours.DroppedBehaviours {
    public class IngredientBehaviour : DroppedBehaviour {
        public IngredientsType ingredientsType;

        public override void GenerateDroppedData() {
            var ingredientData = new IngredientData {
                droppedGuid = droppedGuid,
                droppedPosition = JsonHelper.FromVector3ToString(transform.position),
                droppedRotation = JsonHelper.FromQuaternionToString(transform.rotation),
                IngredientsType = droppedPropertiesScriptableObject.ingredientsType
            };
            
            ObjectsReference.Instance.gameSave.droppedIngredientsSave.AddIngredientsToIngredientsDictionnary(
                ingredientsType, JsonConvert.SerializeObject(ingredientData));
        }

        public override void LoadSavedData(string stringifiedJson) {
            var ingredientData = JsonConvert.DeserializeObject<IngredientData>(stringifiedJson);

            droppedGuid = ingredientData.droppedGuid;
            ingredientsType = ingredientData.IngredientsType;
            transform.position = JsonHelper.FromStringToVector3( ingredientData.droppedPosition);
            transform.rotation = JsonHelper.FromStringToQuaternion(ingredientData.droppedRotation);
        }
    }
}
