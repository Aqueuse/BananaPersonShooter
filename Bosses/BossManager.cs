using System.Collections.Generic;
using Audio;
using Enums;
using Player;
using UI.InGame;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

namespace Bosses {
    public class BossManager : MonoSingleton<BossManager> {
        [SerializeField] private GameObject[] bossesPrefabs;
        [SerializeField] private List<BossType> bossesTypes;

        [SerializeField] private GameObject arenaDoor;
        [SerializeField] private GameObject arenaTerrain;

        [SerializeField] private GameObject bossSpawnPoint;
        
        private Transform _bananaManTransform;
        private Vector3 _bananaManDirectionToCenter;
        
        public UIBoss associatedUI;

        private Dictionary<BossType, GameObject> _bossesDictionnary;
        
        public BossType activeBossType;
        private Boss _activeBoss;

        private Vector3 _bananaManDirectionFromCenter;
        private float _bananaManDistanceFromCenter;

        private float _arenaRadius;
        private static readonly int Fall = Animator.StringToHash("FALL");

        private void Start() {
            _bossesDictionnary = new Dictionary<BossType, GameObject>();

            for (int i = 0; i < bossesPrefabs.Length; i++) {
                _bossesDictionnary.Add(bossesTypes[i],bossesPrefabs[i]);
            }

            _bananaManTransform = BananaMan.Instance.transform;
            _arenaRadius = (arenaDoor.transform.position - transform.position).magnitude;
        }

        private void Update() {
            // Securize bananaMan in the arena when fighing
            _bananaManTransform = BananaMan.Instance.transform;
            var bananaManPosition = _bananaManTransform.position;
            
            // store the direction and distance from the center of banana man
            _bananaManDirectionFromCenter = bananaManPosition - transform.position;
            _bananaManDistanceFromCenter = _bananaManDirectionFromCenter.magnitude;

            if (_bananaManDistanceFromCenter > _arenaRadius+1.5 && GameManager.Instance.isFigthing) {
                _bananaManDirectionToCenter = transform.position - _bananaManTransform.position;
                BananaMan.Instance.transform.position += _bananaManDirectionToCenter * 0.05f;
            }
            
            // get the Y position of banana man
            // if position is below the terrain of the aren, move it back to position Y = 1
            if (_bananaManTransform.position.y < arenaTerrain.transform.position.y) {
                _bananaManTransform.position = new Vector3(
                    _bananaManTransform.position.x, 
                    arenaTerrain.transform.position.y+0.1f, 
                    bananaManPosition.z
                );
            }
        }

        public GameObject GetActiveBoss() {
            return _bossesDictionnary[activeBossType];
        }
        
        public void StartBossFight(BossType bossType) {
            GameManager.Instance.isFigthing = true;
            
            AudioManager.Instance.PlayMusic(MusicType.BOSS, true);
            
            associatedUI.Show_Boss_Life_Slider();
            UIFace.Instance.GetHorrified();

            activeBossType = bossType;

            if (bossType == BossType.KELSAIK) {
                NavMeshTriangulation navMeshTriangulation = NavMesh.CalculateTriangulation();

                int vertexIndex = Random.Range(0, navMeshTriangulation.vertices.Length);

                // instanciate Kelsaik
                var boss = Instantiate(_bossesDictionnary[activeBossType], bossSpawnPoint.transform.position, Quaternion.identity);
                _activeBoss = boss.GetComponent<Boss>();

                if (NavMesh.SamplePosition(navMeshTriangulation.vertices[vertexIndex], out NavMeshHit navMeshHit, 2f, 0)) {
                    boss.GetComponent<NavMeshAgent>().Warp(navMeshHit.position);
                }
                boss.transform.localScale = new Vector3(1, 1, 1);

                _activeBoss.isSatieted = false;
            }
        }

        public void Win() {
            associatedUI.Hide_Boss_Life_Slider();
            GameManager.Instance.isFigthing = false;
            AudioManager.Instance.PlayMusic(MusicType.BOSS_VICTORY, false);

            if (activeBossType == BossType.KELSAIK) {
                _activeBoss.GetComponent<NavMeshAgent>().isStopped = true;
                _activeBoss.isSatieted = true;
                _activeBoss.GetComponent<Animator>().SetTrigger(Fall);
                UIFace.Instance.GetHappy();
            }
        }
    }
}
