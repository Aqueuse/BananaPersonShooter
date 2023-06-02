
using System;
using System.Collections.Generic;
using System.Linq;
using Building;
using Monkeys.Gorilla;
using UnityEngine;

namespace Game {
    public class Map : MonoBehaviour {
        public MonkeyType activeMonkeyType;
        public float cleanliness;

        private Vector3 _bananaManDirectionToCenter;

        public int maxDebrisQuantity;
        private int _actualDebrisQuantity;

        public List<Vector3> aspirablesPositions;
        public List<Quaternion> aspirablesRotations;
        
        public List<ItemCategory> aspirablesCategories;
        public List<int> aspirablesPrefabsIndex;
        public List<BuildableType> aspirablesBuildableTypes;
        public List<ItemType> aspirablesItemTypes;

        public float monkeySasiety;

        public bool hasBananaTree;
        public bool hasDebris;
        public bool isDiscovered;

        public string mapName;

        public int debrisToSpawn;
        
        private void Start() {
            maxDebrisQuantity = 99;
            debrisToSpawn = 0;
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

            var aspirables = MapItems.Instance.aspirablesContainer.gameObject.GetComponentsInChildren<MeshRenderer>();

            var mapAspirablesCategories = new ItemCategory[aspirables.Length];
            var mapAspirablePosition = new Vector3[aspirables.Length];
            var mapAspirableRotation = new Quaternion[aspirables.Length];
            var mapAspirablePrefabIndex = new string[aspirables.Length];

            for (int i = 0; i < aspirables.Length; i++) {
                if (aspirables[i].gameObject.layer == 7) {
                    // check if is debris or buildable
                    var aspirableMesh = aspirables[i].GetComponent<MeshFilter>().sharedMesh;

                    if (ObjectsReference.Instance.scriptableObjectManager.IsBuildable(aspirableMesh)) {
                        mapAspirablesCategories[i] = ItemCategory.DEBRIS;
                        mapAspirablePrefabIndex[i] = ObjectsReference.Instance.scriptableObjectManager._meshReferenceScriptableObject.debrisPrefabIndexByMesh[aspirableMesh].ToString();
                    }

                    else {
                        mapAspirablesCategories[i] = ItemCategory.BUILDABLE;
                        mapAspirablePrefabIndex[i] = ObjectsReference.Instance.scriptableObjectManager
                            ._meshReferenceScriptableObject.buildableTypeByMesh[aspirableMesh].ToString();
                    }

                    mapAspirablePosition[i] = aspirables[i].transform.position;
                    mapAspirableRotation[i] = aspirables[i].transform.rotation;
                }
            }
        }

        public int GetDebrisQuantity() {
            return Enum.GetValues(typeof(ItemCategory)).OfType<ItemCategory>().Count(itemCategory => itemCategory == ItemCategory.DEBRIS);
        }

        public void StartBossFight(MonkeyType monkeyType) {
            ObjectsReference.Instance.audioManager.PlayMusic(MusicType.FIGHT);

            activeMonkeyType = monkeyType;
        }
    }
}