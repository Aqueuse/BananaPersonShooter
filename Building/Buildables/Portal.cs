using Items.ItemsActions;
using UnityEngine;

namespace Building.Buildables {
    public class Portal : MonoBehaviour {
        [SerializeField] private GameObject dotPrefab;
    
        public void ShowAllPossibleDestinations() {
            foreach (var map in ObjectsReference.Instance.mapsManager.mapBySceneName) {
                foreach (var portal in map.Value.portals) {
                    var dot = Instantiate(dotPrefab, transform);
                    var dotItemAction = dot.GetComponent<PortalDestinationItemAction>(); 

                    dotItemAction.destinationPosition = portal.position;
                    dotItemAction.destinationRotation = portal.rotation;
                    dotItemAction.sceneName = map.Key;
                    dotItemAction.portalUuid = portal.uuid;

                    //dot.GetComponent<RectTransform>().localPosition = ????;
                }
            }
        }
    }
}
