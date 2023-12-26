using Data.BuildablesData;
using Newtonsoft.Json;
using Save.Helpers;

namespace Gestion.BuildablesBehaviours {
    public class BasicBehaviour : BuildableBehaviour {
        public override void GenerateSaveData() {
            BasicData basicData = new BasicData {
                buildableGuid = buildableGuid,
                buildableType = buildableType,
                isBreaked = isBreaked,
                buildablePosition = JsonHelper.FromVector3ToString(transform.position),
                buildableRotation = JsonHelper.FromQuaternionToString(transform.rotation)
            };

            ObjectsReference.Instance.gameData.currentMapData.AddBuildableToBuildableDictionnary(buildableType, JsonConvert.SerializeObject(basicData));
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
