using Audio;
using Enums;
using UI.InGame;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

namespace Monkeys {
    public class MonkeyManager : MonoSingleton<MonkeyManager> {
        [SerializeField] private GenericDictionary<MonkeyType, GameObject> monkeyPrefabsByType;
        [SerializeField] private GameObject monkeySpawnPoint;

        private Vector3 _bananaManDirectionToCenter;
        
        public UIMonkey associatedUI;
        public MonkeyType activeMonkeyType;

        private Monkey _activeMonkey;
        public MonkeyState monkeyState;
        
        private void Start() {
            activeMonkeyType = MonkeyType.KELSAIK;
            SpawnMonkey(activeMonkeyType);

            monkeyState = MonkeyState.HUNGRY;
        }
        
        public GameObject GetActiveBoss() {
            return _activeMonkey.gameObject;
        }
        
        public void StartBossFight(MonkeyType monkeyType) {
            GameManager.Instance.isFigthing = true;
            
            AudioManager.Instance.PlayMusic(MusicType.BOSS, true);
            
            UIFace.Instance.GetHorrified();

            activeMonkeyType = monkeyType;
        }

        public void SpawnMonkey(MonkeyType monkeyType) {
            if (monkeyType == MonkeyType.KELSAIK) {
                NavMeshTriangulation navMeshTriangulation = NavMesh.CalculateTriangulation();
                
                int vertexIndex = Random.Range(0, navMeshTriangulation.vertices.Length);
                
                // instanciate monkey
                var boss = Instantiate(monkeyPrefabsByType[activeMonkeyType], monkeySpawnPoint.transform.position, Quaternion.identity);
                _activeMonkey = boss.GetComponent<Monkey>();

                if (NavMesh.SamplePosition(navMeshTriangulation.vertices[vertexIndex], out NavMeshHit navMeshHit, 2f, 0)) {
                    boss.GetComponent<NavMeshAgent>().Warp(navMeshHit.position);
                }

                boss.transform.localScale = new Vector3(1, 1, 1);
            }
        }

        public void GetHappy() {
            Debug.Log("monkey is happy !");
        }

    }
}
