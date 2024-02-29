using InGame.Items.ItemsProperties.Maps;
using Tags;
using UnityEngine;

namespace InGame.Interactions.InteractionsActions.SpaceTrafficControl {
    public class CommunicationsTabCannonInteraction : Interaction {
        public override void Activate(GameObject interactedGameObject) {
            // start rotating effective cannon
            var sceneData = (MapPropertiesScriptableObject)interactedGameObject.GetComponent<Tag>().itemScriptableObject; 
            
            ObjectsReference.Instance.spaceTrafficControlMiniGameManager.ActivateCannon(sceneData.regionName);
        }
    }
}
