
using System.Collections.Generic;
using Building;
using Building.Buildables.Plateforms;
using Monkeys.Gorilla;
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
        
        private void Start() {
            maxDebrisQuantity = 99;
            debrisToSpawn = 0;
        }

        public void Init() {
            if (activeMonkeyType != MonkeyType.NONE) {
                RecalculateHappiness();
                MapItems.Instance.uiCanvasItemsHiddableManager.SetMonkeysVisibility(ObjectsReference.Instance.gameSettings.isShowingMonkeys);
                MapItems.Instance.uiCanvasItemsHiddableManager.SetDebrisCanvasVisibility(ObjectsReference.Instance.gameSettings.isShowingDebris);
                MapItems.Instance.uiCanvasItemsHiddableManager.SetBananaTreeVisibility(ObjectsReference.Instance.gameSettings.isShowingBananaTrees); 
            }
        }

        public void RecalculateHappiness() {
            _actualDebrisQuantity = MapItems.Instance.aspirablesContainer.GetComponentsInChildren<MeshFilter>().Length;

            cleanliness = 50-(_actualDebrisQuantity /(float)maxDebrisQuantity)*50;

            foreach (var monkey in MapItems.Instance.monkeys) {
                monkey.happiness = monkey.sasiety + cleanliness;

                if (monkey.happiness < 20 && monkey.monkeyState != MonkeyState.ANGRY) {
                    monkey.monkeyState = MonkeyState.ANGRY;
                }

                if (monkey.happiness is >= 20 and < 60 && monkey.monkeyState != MonkeyState.SAD) {
                    monkey.monkeyState = MonkeyState.SAD;
                    monkey.GetComponent<GorillaSounds>().PlaySadMonkeySounds();
                }

                if (monkey.happiness >= 60 && monkey.monkeyState != MonkeyState.HAPPY) {
                    monkey.monkeyState = MonkeyState.HAPPY;
                }
            }
            
            foreach (var monkey in MapItems.Instance.monkeys) {
                monkey.associatedUI.SetSliderValue(ObjectsReference.Instance.monkeysManager.colorByMonkeyState[monkey.monkeyState]);
            }
        }

        public void RefreshAspirablesDataMap() {
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
                aspirablesPrefabsIndex.Add(ObjectsReference.Instance.scriptableObjectManager._meshReferenceScriptableObject.debrisPrefabIndexByMesh[debris.GetComponent<MeshFilter>().sharedMesh]);
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
                var aspirableBuildableType = ObjectsReference.Instance.scriptableObjectManager.GetBuildableTypeByMesh(buildable.GetComponent<MeshFilter>().sharedMesh);
                aspirablesBuildableTypes.Add(aspirableBuildableType);
                aspirablesItemTypes.Add(aspirableBuildableType == BuildableType.PLATEFORM ? buildable.GetComponent<Plateform>().plateformType : ItemType.EMPTY);
            }
        }

        public int GetDebrisQuantity() {
            return GameObject.FindGameObjectsWithTag("Debris").Length;
        }

        public void StartBossFight(MonkeyType monkeyType) {
            ObjectsReference.Instance.audioManager.PlayMusic(MusicType.FIGHT);

            activeMonkeyType = monkeyType;
        }
    }
}