using System.Collections.Generic;
using Building;
using Building.Buildables.Plateforms;
using Data.Maps;
using Enums;
using Game.Steam;
using Monkeys;
using Tags;
using UnityEngine;

namespace Game {
    public class Map : MonoBehaviour {
        public MapDataScriptableObject mapDataScriptableObject;
        
        public bool isDiscovered;

        public int debrisToSpawn;

        public float cleanliness;

        public int maxDebrisQuantity;
        private int _actualDebrisQuantity;

        public GameObject initialAspirablesOnMap;

        public List<Vector3> aspirablesPositions;
        public List<Quaternion> aspirablesRotations;

        public List<ItemCategory> aspirablesCategories;
        public List<int> aspirablesPrefabsIndex;
        public List<BuildableType> aspirablesBuildableTypes;
        public List<BananaType> aspirablesItemTypes;

        public List<PortalDestination> portals;

        private void Start() {
            maxDebrisQuantity = 99;
            debrisToSpawn = 0;
        }

        public void Init() {
            if (mapDataScriptableObject.monkeyType != MonkeyType.NONE) {
                foreach (var monkey in MapItems.Instance.monkeys) {
                    RecalculateHappiness(monkey);
                }
            }
        }

        public void RecalculateHappiness(Monkey monkey) {
            _actualDebrisQuantity = MapItems.Instance.aspirablesContainer.GetComponentsInChildren<MeshFilter>().Length;

            cleanliness = 50-_actualDebrisQuantity /(float)maxDebrisQuantity*50;
            
            if (cleanliness >= 50 && ObjectsReference.Instance.steamIntegration.isGameOnSteam) {
                ObjectsReference.Instance.steamIntegration.UnlockAchievement(SteamAchievement.STEAM_ACHIEVEMENT_JUNGLE_CLEANED); 
            }

            monkey.monkeyDataScriptableObject.happiness = monkey.monkeyDataScriptableObject.sasiety + cleanliness;
            
            if (monkey.monkeyDataScriptableObject.happiness < 20 && monkey.monkeyState != MonkeyState.ANGRY) {
                monkey.monkeyState = MonkeyState.ANGRY;
                monkey.monkeySounds.PlayRoarsSounds();
            }
            
            if (monkey.monkeyDataScriptableObject.happiness is >= 20 and < 60 && monkey.monkeyState != MonkeyState.SAD) {
                monkey.monkeyState = MonkeyState.SAD;
                monkey.monkeySounds.PlayQuickMonkeySounds();
            }
            
            if (monkey.monkeyDataScriptableObject.happiness >= 60 && monkey.monkeyState != MonkeyState.HAPPY) {
                monkey.monkeyState = MonkeyState.HAPPY;
                monkey.monkeySounds.PlayQuickMonkeySounds();
            }
            
        }

        public void RefreshAspirablesItemsDataMap() {
            if (MapItems.Instance == null) return;

            aspirablesCategories = new List<ItemCategory>();
            aspirablesPositions = new List<Vector3>();
            aspirablesRotations = new List<Quaternion>();
            aspirablesPrefabsIndex = new List<int>();
            aspirablesBuildableTypes = new List<BuildableType>();
            aspirablesItemTypes = new List<BananaType>();
            
            // get the debris list
            var debrisList = TagsManager.Instance.GetAllGameObjectsWithTag(GAME_OBJECT_TAG.DEBRIS);
            
            foreach (var debris in debrisList) {
                aspirablesCategories.Add(ItemCategory.DEBRIS);
                aspirablesPositions.Add(debris.transform.position);
                aspirablesRotations.Add(debris.transform.rotation);
                aspirablesPrefabsIndex.Add(ObjectsReference.Instance.scriptableObjectManager._meshReferenceScriptableObject.debrisMeshes.IndexOf(debris.GetComponent<MeshFilter>().sharedMesh));
                aspirablesBuildableTypes.Add(BuildableType.EMPTY);
                aspirablesItemTypes.Add(BananaType.EMPTY);
            }
            
            // get the buildables list
            var buildablesList = TagsManager.Instance.GetAllGameObjectsWithTag(GAME_OBJECT_TAG.BUILDABLE);
                
            foreach (var buildable in buildablesList) {
                aspirablesCategories.Add(ItemCategory.BUILDABLE);
                aspirablesPositions.Add(buildable.transform.position);
                aspirablesRotations.Add(buildable.transform.rotation);
                aspirablesPrefabsIndex.Add(0);
                var aspirableBuildableType = ObjectsReference.Instance.scriptableObjectManager.GetBuildableTypeByMesh(buildable.GetComponent<MeshFilter>().sharedMesh);
                aspirablesBuildableTypes.Add(aspirableBuildableType);
                
                if (aspirableBuildableType == BuildableType.PLATEFORM) {
                    aspirablesItemTypes.Add(buildable.GetComponent<Plateform>().plateformType);
                }
                else {
                    aspirablesItemTypes.Add(BananaType.EMPTY);
                }
            }

            var ruinesList = TagsManager.Instance.GetAllGameObjectsWithTag(GAME_OBJECT_TAG.RUINE);

            foreach (var ruine in ruinesList) {
                aspirablesCategories.Add(ItemCategory.RUINE);
                aspirablesPositions.Add(ruine.transform.position);
                aspirablesRotations.Add(ruine.transform.rotation);
                
                aspirablesPrefabsIndex.Add(ObjectsReference.Instance.scriptableObjectManager._meshReferenceScriptableObject.ruinesMeshes.IndexOf(ruine.GetComponent<MeshFilter>().sharedMesh));
                aspirablesBuildableTypes.Add(BuildableType.EMPTY);
                aspirablesItemTypes.Add(BananaType.EMPTY);
            }
        }

        public int GetDebrisQuantity() {
            return debrisToSpawn+TagsManager.Instance.GetAllGameObjectsWithTag(GAME_OBJECT_TAG.DEBRIS).Count;
        }

        public void StartBossFight(MonkeyType monkeyType) {
            ObjectsReference.Instance.audioManager.PlayMusic(MusicType.FIGHT, 0);
        }
    }
}