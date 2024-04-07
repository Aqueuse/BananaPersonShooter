using InGame.Interactions.InteractionsActions;
using InGame.Items.ItemsData.BuildablesData;
using Newtonsoft.Json;
using Save.Helpers;
using UnityEngine;

namespace InGame.Items.ItemsBehaviours.BuildablesBehaviours {
    public class PortalBehaviour : BuildableBehaviour {
        [SerializeField] private GameObject dotPrefab;
        
        public string portalName;

        public void ShowAllPossibleDestinations() {
            foreach (var portal in ObjectsReference.Instance.worldData.portals) {
                var dot = Instantiate(dotPrefab, transform);
                var dotItemAction = dot.GetComponent<PortalDestinationInteraction>(); 

                dotItemAction.destinationPosition = portal.position;
                dotItemAction.destinationRotation = portal.rotation;
                dotItemAction.portalUuid = portal.uuid;

                //dot.GetComponent<RectTransform>().localPosition = ????;
            }
        }

        public override void GenerateSaveData() {
            PortalData basicData = new PortalData {
                buildableGuid = buildableGuid,
                buildableType = buildableType,
                isBreaked = isBreaked,
                buildablePosition = JsonHelper.FromVector3ToString(transform.position),
                buildableRotation = JsonHelper.FromQuaternionToString(transform.rotation),
            };
            
            ObjectsReference.Instance.gameSave.buildablesSave.AddBuildableToBuildableDictionnary(BuildableType.PORTAL, JsonConvert.SerializeObject(basicData));
        }

        public override void LoadSavedData(string stringifiedJson) {
            PortalData portalData = JsonConvert.DeserializeObject<PortalData>(stringifiedJson);

            buildableGuid = portalData.buildableGuid;
            buildableType = portalData.buildableType;
            isBreaked = portalData.isBreaked;
            transform.position = JsonHelper.FromStringToVector3( portalData.buildablePosition);
            transform.rotation = JsonHelper.FromStringToQuaternion(portalData.buildableRotation);

            portalName = portalData.portalName;
        }
    }
}
