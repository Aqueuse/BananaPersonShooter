using System.Collections.Generic;
using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using InGame.Monkeys.Chimpirates;
using InGame.Monkeys.Merchimps;
using UnityEngine;
using Random = UnityEngine.Random;

namespace InGame.Monkeys {
    public class ChimpManager : MonoBehaviour {
        [SerializeField] private GameObject piratePrefab;
        public MerchimpsManager merchimpsManager;

        public Transform sasTransform;
        public Transform[] spawnTransforms;
        private Transform spawnPointTransform;

        private List<BuildableBehaviour> buildablesToBreak;

        public List<PirateBehaviour> spawnedPirates;
        private GameObject pirate;
        
        private void Start() {
            spawnedPirates = new List<PirateBehaviour>();
        }

        public void SpawnPirate() {
            spawnPointTransform = GetRandomSasSpawnPoint();
            pirate = Instantiate(piratePrefab, spawnPointTransform.position, spawnPointTransform.rotation);
            spawnedPirates.Add(pirate.GetComponent<PirateBehaviour>());
        }
        
        public Transform GetRandomSasSpawnPoint() {
            return spawnTransforms[Random.Range(0, spawnTransforms.Length)];
        }
    }
}
