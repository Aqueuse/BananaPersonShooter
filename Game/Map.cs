using System.Linq;
using Audio;
using Building;
using Enums;
using MiniChimps;
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

        public float monkeySasiety;

        public bool hasDebris;
        public bool isDiscovered;

        public string mapName;
        
        private void Start() {
            isDiscovered = false;
            maxDebrisQuantity = 28;
        }
    
        public void RecalculateHapiness() {
            _actualDebrisQuantity = GameObject.FindGameObjectWithTag("debrisContainer").GetComponentInChildren<Transform>().childCount;
            
            cleanliness = 50-(_actualDebrisQuantity /(float)maxDebrisQuantity)*50;
            activeMonkey.happiness = activeMonkey.sasiety + cleanliness;
            
            UIStatistics.Instance.Refresh_Map_Statistics(MapsManager.Instance.currentMap.mapName);
        
            if (activeMonkey.happiness < 20 && activeMonkey.monkeyState != MonkeyState.ANGRY) {
                activeMonkey.monkeyState = MonkeyState.ANGRY;
                HautParleurs.Instance.PlayHappinessLevelClip(MonkeyState.ANGRY);
            }
            
            if (activeMonkey.happiness is >= 20 and < 30 && activeMonkey.monkeyState != MonkeyState.SAD) {
                activeMonkey.monkeyState = MonkeyState.SAD;
                activeMonkey.GetComponent<GorillaSounds>().PlaySadMonkeySounds();
                HautParleurs.Instance.PlayHappinessLevelClip(MonkeyState.SAD);
            }
            
            if (activeMonkey.happiness >= 60 && activeMonkey.monkeyState != MonkeyState.HAPPY) {
                activeMonkey.monkeyState = MonkeyState.HAPPY;
                HautParleurs.Instance.PlayHappinessLevelClip(MonkeyState.HAPPY);
            }
        }

        public void Clean() {
            if (cleanliness <= 50) {
                RecalculateHapiness();
                activeMonkey.associatedUI.SetSliderValue(activeMonkey.happiness, activeMonkey.monkeyState);
            }
        }

        public void SaveDataOnMap() {
            var map = MapsManager.Instance.currentMap;

            if (activeMonkeyType != MonkeyType.NONE) {
                map.monkeySasiety = activeMonkey.sasiety;
                map.cleanliness = cleanliness;
            }

            if (GameObject.FindGameObjectWithTag("debrisContainer") != null) {
                var debrisTransforms = GameObject.FindGameObjectWithTag("debrisContainer")
                    .GetComponentsInChildren<Transform>().ToList();
                
                debrisTransforms.RemoveAt(0);
                
                var mapDebrisPrefabIndex = new int[debrisTransforms.Count];
                var mapDebrisPosition = new Vector3[debrisTransforms.Count];
                var mapDebrisRotation = new Quaternion[debrisTransforms.Count];

                for (var i = 0; i < debrisTransforms.Count; i++) {
                    mapDebrisPrefabIndex[i] = debrisTransforms[i].GetComponent<Debris>().prefabIndex;
                    mapDebrisPosition[i] = debrisTransforms[i].position;
                    mapDebrisRotation[i] = debrisTransforms[i].rotation;
                }

                map.debrisIndex = mapDebrisPrefabIndex;
                map.debrisPosition = mapDebrisPosition;
                map.debrisRotation = mapDebrisRotation;
            }
        }

        public GameObject GetActiveMonkey() {
            return activeMonkey.gameObject;
        }
        
        public void StartBossFight(MonkeyType monkeyType) {
            AudioManager.Instance.PlayMusic(MusicType.BOSS, true);

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