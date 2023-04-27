using System.Collections.Generic;
using System.Linq;
using Building;
using Building.Plateforms;
using Enums;
using Monkeys.Gorilla;
using UnityEngine;

namespace Game {
    public class Map : MonoBehaviour {
        public MonkeyType activeMonkeyType;
        public float cleanliness;

        private Vector3 _bananaManDirectionToCenter;

        public int maxDebrisQuantity;
        private int _actualDebrisQuantity;

        public Vector3[] debrisPosition;
        public Quaternion[] debrisRotation;
        public int[] debrisIndex;

        public List<Vector3> plateformsPosition;
        public List<ItemType> plateformsTypes;
        
        public float monkeySasiety;

        public bool hasBananaTree;
        public bool hasDebris;
        public bool isDiscovered;

        public string mapName;
        
        private void Start() {
            maxDebrisQuantity = 28;
        }

        public void RecalculateHappiness() {
            _actualDebrisQuantity = MapItems.Instance.debrisContainer.GetComponentsInChildren<Debris>().Length;

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
        
        public void RefreshDebrisDataMap() {
            if (MapItems.Instance == null) return;

            if (ObjectsReference.Instance.mapsManager.currentMap.hasDebris) {
                var debrisClass = MapItems.Instance.debrisContainer.gameObject.GetComponentsInChildren<MeshRenderer>();

                var mapDebrisPrefabIndex = new int[debrisClass.Length];
                var mapDebrisPosition = new Vector3[debrisClass.Length];
                var mapDebrisRotation = new Quaternion[debrisClass.Length];

                for (var i = 0; i < debrisClass.Length; i++) {
                    mapDebrisPrefabIndex[i] = debrisClass[i].GetComponent<Debris>().prefabIndex;
                    mapDebrisPosition[i] = debrisClass[i].gameObject.transform.position;
                    mapDebrisRotation[i] = debrisClass[i].gameObject.transform.rotation;
                }

                debrisIndex = mapDebrisPrefabIndex;
                debrisPosition = mapDebrisPosition;
                debrisRotation = mapDebrisRotation;
            }
        }
        
        public void RefreshPlateformsDataMap() {
            var plateformsClass = MapItems.Instance.plateformsContainer.gameObject.GetComponentsInChildren<Plateform>().ToList();

            plateformsPosition = new List<Vector3>();
            plateformsTypes = new List<ItemType>();
            
            foreach (var plateformClass in plateformsClass) {
                plateformsPosition.Add(plateformClass.transform.position);
                plateformsTypes.Add(plateformClass.plateformType);
            }
        }
        
        public void StartBossFight(MonkeyType monkeyType) {
            ObjectsReference.Instance.audioManager.PlayMusic(MusicType.FIGHT);

            activeMonkeyType = monkeyType;
        }
    }
}