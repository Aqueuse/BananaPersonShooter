using System.Collections.Generic;
using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using InGame.Monkeys.Chimpirates;
using InGame.Monkeys.Merchimps;
using UnityEngine;
using Random = UnityEngine.Random;

namespace InGame.Monkeys {
    public class ChimpManager : MonoBehaviour {
        public MerchimpsManager merchimpsManager;

        public Transform chimpmensContainer;
        
        public Transform[] spawnTransforms;
        private Transform spawnPointTransform;

        public Transform waitingSpot;

        private List<BuildableBehaviour> buildablesToBreak;

        public List<PirateBehaviour> spawnedPirates;
        private GameObject pirate;
        
        private void Start() {
            spawnedPirates = new List<PirateBehaviour>();
        }
        
        public Transform GetRandomSasSpawnPoint() {
            return spawnTransforms[Random.Range(0, spawnTransforms.Length)];
        }
    }
}
