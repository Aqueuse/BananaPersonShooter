using InGame.Items.ItemsData.BuildablesData;
using Newtonsoft.Json;
using Save.Helpers;

namespace InGame.Items.ItemsBehaviours.BuildablesBehaviours {
    public class BasicBehaviour : BuildableBehaviour {
        public override void GenerateSaveData() {
            BasicData basicData = new BasicData {
                buildableGuid = buildableGuid,
                buildableType = buildableType,
                isBreaked = isBreaked,
                buildablePosition = JsonHelper.FromVector3ToString(transform.position),
                buildableRotation = JsonHelper.FromQuaternionToString(transform.rotation)
            };

            ObjectsReference.Instance.gameData.worldData.AddBuildableToBuildableDictionnary(buildableType, JsonConvert.SerializeObject(basicData));
        }

        public override void LoadSavedData(string stringifiedJson) {
            BasicData basicData = JsonConvert.DeserializeObject<BasicData>(stringifiedJson);

            buildableGuid = basicData.buildableGuid;
            buildableType = basicData.buildableType;
            isBreaked = basicData.isBreaked;
            transform.position = JsonHelper.FromStringToVector3( basicData.buildablePosition);
            transform.rotation = JsonHelper.FromStringToQuaternion(basicData.buildableRotation);
        }
    }
}
