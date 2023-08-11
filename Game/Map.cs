using System.Collections.Generic;
using Building;
using Building.Buildables.Plateforms;
using Enums;
using Game.Steam;
using Items;
using UnityEngine;

namespace Game {
    public class Map : MonoBehaviour {
        public string mapName;
        public bool isDiscovered;

        public int debrisToSpawn;

        public MonkeyType activeMonkeyType;
        public float monkeySasiety;
        public float cleanliness;

        public int maxDebrisQuantity;
        private int _actualDebrisQuantity;

        public GameObject initialAspirablesOnMap;

        public List<Vector3> aspirablesPositions;
        public List<Quaternion> aspirablesRotations;

        public List<ItemCategory> aspirablesCategories;
        public List<int> aspirablesPrefabsIndex;
        public List<BuildableType> aspirablesBuildableTypes;
        public List<ItemType> aspirablesItemTypes;

        public List<PortalDestination> portals;

        private void Start() {
            maxDebrisQuantity = 99;
            debrisToSpawn = 0;
        }

        public void Init() {
            if (activeMonkeyType != MonkeyType.NONE) {
                RecalculateHappiness();
                MapItems.Instance.uiCanvasItemsHiddableManager.SetMonkeysVisibility(ObjectsReference.Instance.gameSettings.isShowingMonkeys);
                MapItems.Instance.uiCanvasItemsHiddableManager.SetDebrisSpriteRendererVisibility(ObjectsReference.Instance.gameSettings.isShowingDebris);
                MapItems.Instance.uiCanvasItemsHiddableManager.SetBananaTreeVisibility(ObjectsReference.Instance.gameSettings.isShowingBananaTrees); 
            }
        }

        public void RecalculateHappiness() {
            _actualDebrisQuantity = MapItems.Instance.aspirablesContainer.GetComponentsInChildren<MeshFilter>().Length;

            cleanliness = 50-_actualDebrisQuantity /(float)maxDebrisQuantity*50;
            
            if (cleanliness >= 50 && ObjectsReference.Instance.steamIntegration.isGameOnSteam) {
                ObjectsReference.Instance.steamIntegration.UnlockAchievement(SteamAchievement.STEAM_ACHIEVEMENT_JUNGLE_CLEANED); 
            }

            
            foreach (var monkey in MapItems.Instance.monkeys) {
                monkey.happiness = monkey.sasiety + cleanliness;

                if (monkey.happiness < 20 && monkey.monkeyState != MonkeyState.ANGRY) {
                    monkey.monkeyState = MonkeyState.ANGRY;
                    monkey.monkeySounds.PlayRoarsSounds();
                }

                if (monkey.happiness is >= 20 and < 60 && monkey.monkeyState != MonkeyState.SAD) {
                    monkey.monkeyState = MonkeyState.SAD;
                    monkey.monkeySounds.PlayQuickMonkeySounds();
                }

                if (monkey.happiness >= 60 && monkey.monkeyState != MonkeyState.HAPPY) {
                    monkey.monkeyState = MonkeyState.HAPPY;
                    monkey.monkeySounds.PlayQuickMonkeySounds();
                }
            }
            
            foreach (var monkey in MapItems.Instance.monkeys) {
                monkey.associatedUI.SetSasietySliderValue(monkey.sasiety);
                monkey.associatedUI.SetCleanlinessSliderValue(cleanliness);
            }
        }

        public void RefreshAspirablesItemsDataMap() {
            if (MapItems.Instance == null) return;

            aspirablesCategories = new List<ItemCategory>();
            aspirablesPositions = new List<Vector3>();
            aspirablesRotations = new List<Quaternion>();
            aspirablesPrefabsIndex = new List<int>();
            aspirablesBuildableTypes = new List<BuildableType>();
            aspirablesItemTypes = new List<ItemType>();
            
            // get the debris list
            var debrisList = GameObject.FindGameObjectsWithTag("Debris");
            
            foreach (var debris in debrisList) {
                aspirablesCategories.Add(ItemCategory.DEBRIS);
                aspirablesPositions.Add(debris.transform.position);
                aspirablesRotations.Add(debris.transform.rotation);
                aspirablesPrefabsIndex.Add(ObjectsReference.Instance.scriptableObjectManager._meshReferenceScriptableObject.debrisMeshes.IndexOf(debris.GetComponent<MeshFilter>().sharedMesh));
                aspirablesBuildableTypes.Add(BuildableType.EMPTY);
                aspirablesItemTypes.Add(ItemType.EMPTY);
            }
            
            // get the buildables list
            var buildablesList = GameObject.FindGameObjectsWithTag("Buildable");
                
            foreach (var buildable in buildablesList) {
                aspirablesCategories.Add(ItemCategory.BUILDABLE);
                aspirablesPositions.Add(buildable.transform.position);
                aspirablesRotations.Add(buildable.transform.rotation);
                aspirablesPrefabsIndex.Add(0);
                var aspirableBuildableType = ObjectsReference.Instance.buildablesManager.GetBuildableTypeByMesh(buildable.GetComponent<MeshFilter>().sharedMesh);
                aspirablesBuildableTypes.Add(aspirableBuildableType);
                if (aspirableBuildableType == BuildableType.PLATEFORM) {
                    aspirablesItemTypes.Add(buildable.GetComponent<Plateform>().plateformType);
                }
                else {
                    aspirablesItemTypes.Add(ItemType.EMPTY);
                }
            }
            
            var chimployee = GameObject.FindGameObjectWithTag("Monkeyman");

            if (chimployee != null) {
                aspirablesCategories.Add(ItemCategory.CHIMPLOYEE);
                aspirablesPositions.Add(chimployee.transform.position);
                aspirablesRotations.Add(chimployee.transform.rotation);
                aspirablesPrefabsIndex.Add(0);
                aspirablesBuildableTypes.Add(BuildableType.EMPTY);
                aspirablesItemTypes.Add(ItemType.EMPTY);
            }

            var ruinesList = GameObject.FindGameObjectsWithTag("Ruine");

            foreach (var ruine in ruinesList) {
                aspirablesCategories.Add(ItemCategory.RUINE);
                aspirablesPositions.Add(ruine.transform.position);
                aspirablesRotations.Add(ruine.transform.rotation);
                
                aspirablesPrefabsIndex.Add(ObjectsReference.Instance.scriptableObjectManager._meshReferenceScriptableObject.ruinesMeshes.IndexOf(ruine.GetComponent<MeshFilter>().sharedMesh));
                aspirablesBuildableTypes.Add(BuildableType.EMPTY);
                aspirablesItemTypes.Add(ItemType.EMPTY);
            }
        }

        public int GetDebrisQuantity() {
            return debrisToSpawn+GameObject.FindGameObjectsWithTag("Debris").Length;
        }

        public void StartBossFight(MonkeyType monkeyType) {
            ObjectsReference.Instance.audioManager.PlayMusic(MusicType.FIGHT, 0);

            activeMonkeyType = monkeyType;
        }
    }
}