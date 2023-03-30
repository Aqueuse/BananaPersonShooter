using System.Collections.Generic;
using System.Linq;
using Audio;
using Building;
using Building.Plateforms;
using Enums;
using Monkeys;
using Monkeys.Gorilla;
using UI.InGame.Statistics;
using UnityEngine;
using UnityEngine.AI;

namespace Game {
    public class Map : MonoBehaviour {
        [SerializeField] private GameObject monkeyPrefab;
        [SerializeField] private GameObject monkeySpawnPoint;

        public MonkeyType activeMonkeyType;
        public float cleanliness;

        private Vector3 _bananaManDirectionToCenter;
        public Monkey activeMonkey;

        public int maxDebrisQuantity;
        private int _actualDebrisQuantity;

        public Vector3[] debrisPosition;
        public Quaternion[] debrisRotation;
        public int[] debrisIndex;

        public List<Vector3> plateformsPosition;
        public List<PlateformType> plateformsTypes;
        
        public float monkeySasiety;

        public bool hasBananaTree;
        public bool hasDebris;
        public bool isDiscovered;

        public string mapName;
        
        private void Start() {
            maxDebrisQuantity = 28;
        }

        public void RecalculateHapiness() {
            _actualDebrisQuantity = MapItems.Instance.debrisContainer.GetComponentsInChildren<Debris>().Length;

            cleanliness = 50-(_actualDebrisQuantity /(float)maxDebrisQuantity)*50;
            activeMonkey.happiness = activeMonkey.sasiety + cleanliness;

            UIStatistics.Instance.Refresh_Map_Statistics(MapsManager.Instance.currentMap.mapName);

            if (activeMonkey.happiness < 20 && activeMonkey.monkeyState != MonkeyState.ANGRY) {
                activeMonkey.monkeyState = MonkeyState.ANGRY;
            }

            if (activeMonkey.happiness is >= 20 and < 60 && activeMonkey.monkeyState != MonkeyState.SAD) {
                activeMonkey.monkeyState = MonkeyState.SAD;
                activeMonkey.GetComponent<GorillaSounds>().PlaySadMonkeySounds();
            }

            if (activeMonkey.happiness >= 60 && activeMonkey.monkeyState != MonkeyState.HAPPY) {
                activeMonkey.monkeyState = MonkeyState.HAPPY;
            }
        }

        public void Clean() {
            if (cleanliness <= 50) {
                RecalculateHapiness();
                activeMonkey.associatedUI.SetSliderValue(activeMonkey.happiness, activeMonkey.monkeyState);
            }
        }

        public void RefreshDebrisDataMap() {
            if (MapItems.Instance == null) return;

            if (MapsManager.Instance.currentMap.hasDebris) {
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

        public void RefreshMonkeyDataMap() {
            if (activeMonkeyType != MonkeyType.NONE) {
                monkeySasiety = activeMonkey.sasiety;
            }
        }

        public void RefreshPlateformsDataMap() {
            if (MapItems.Instance == null) return;
            
            var plateformsClass =
                MapItems.Instance.plateformsContainer.gameObject.GetComponentsInChildren<Plateform>().ToList();

            plateformsPosition = new List<Vector3>();
            plateformsTypes = new List<PlateformType>();
            
            foreach (var plateformClass in plateformsClass) {
                plateformsPosition.Add(plateformClass.initialPosition);
                plateformsTypes.Add(plateformClass.plateformType);
            }
        }
        
        public GameObject GetActiveMonkey() {
            return activeMonkey.gameObject;
        }

        public void StartBossFight(MonkeyType monkeyType) {
            AudioManager.Instance.PlayMusic(MusicType.FIGHT);

            activeMonkeyType = monkeyType;
        }

        public void SpawnMonkey() {
            if (activeMonkeyType == MonkeyType.KELSAIK) {
                NavMeshTriangulation navMeshTriangulation = NavMesh.CalculateTriangulation();
                
                int vertexIndex = Random.Range(0, navMeshTriangulation.vertices.Length);
                
                // instanciate monkey
                var boss = Instantiate(monkeyPrefab, monkeySpawnPoint.transform.position, Quaternion.identity);
                activeMonkey = boss.GetComponent<Monkey>();

                if (NavMesh.SamplePosition(navMeshTriangulation.vertices[vertexIndex], out NavMeshHit navMeshHit, 2f, 0)) {
                    boss.GetComponent<NavMeshAgent>().Warp(navMeshHit.position);
                }

                boss.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}