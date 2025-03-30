using InGame.Items.ItemsBehaviours;
using UI.InGame.CommandRoomControlPanels;
using UnityEngine;

namespace InGame.CommandRoomPanelControls {
    public class Blueprinter : MonoBehaviour {
        [SerializeField] private Transform blueprintSpawnTransform;
        
        [SerializeField] private UIblueprinter uIblueprinter;
        
        [SerializeField] private GameObject blueprintPrefab;
        
        public void CreateBlueprint(BuildableType[] buildableToGive) {
            Instantiate(
                blueprintPrefab, 
                new Vector3(
                    blueprintSpawnTransform.position.x,
                    blueprintSpawnTransform.position.y,
                    blueprintSpawnTransform.position.z
                ), 
                blueprintSpawnTransform.rotation
            ).GetComponent<BlueprintBehaviour>().associatedBuildables = buildableToGive;
        }
    }
}
