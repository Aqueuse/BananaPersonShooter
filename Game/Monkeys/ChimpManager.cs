using System.Collections.Generic;
using System.Linq;
using Game.Monkeys.Chimpirates;
using Game.Monkeys.Visichimps;
using Gestion.BuildablesBehaviours;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Game.Monkeys {
    public class ChimpManager : MonoSingleton<ChimpManager> {
        [SerializeField] private GameObject piratePrefab;

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

        public void SetPirateDestination(NavMeshAgent navMeshAgent) {
            var pirateBehaviour = navMeshAgent.GetComponent<PirateBehaviour>();
            buildablesToBreak = GetReachableBuildables(navMeshAgent);

            if (buildablesToBreak.Count > 0) {
                var buildableToBreak = buildablesToBreak[Random.Range(0, buildablesToBreak.Count)];
                buildableToBreak.isPirateTargeted = true;
                pirateBehaviour.buildableToBreak = buildableToBreak;
                pirateBehaviour.destination = buildableToBreak.transform.position;
                pirateBehaviour.pirateState = PirateState.GO_BREAK_THING;
            }

            else {
                pirateBehaviour.pirateState = PirateState.SEARCH_RANDOM_POINT;
            }
        }

        public void SetNextVisitorDestination(VisitorBehaviour visitorBehaviour) {
            var topNeed = visitorBehaviour.visitorNeeds.OrderByDescending(pair => pair.Value).First();

            var visitorNavmeshAgent = visitorBehaviour.GetComponent<NavMeshAgent>();

            var nearestBuildable = GetNearestReachableBuildableOfNeedType(visitorNavmeshAgent, topNeed.Key);

            if (nearestBuildable != null) {
                visitorBehaviour.buildableToReach = nearestBuildable;
                visitorBehaviour.destination = nearestBuildable.transform.position;
                visitorBehaviour.visitorState = VisitorState.GO_FILL_NEED;
                nearestBuildable.isVisitorTargeted = true;
                return;
            }
            
            visitorBehaviour.visitorState = VisitorState.SEARCH_RANDOM_POINT;
        }
        
        [CanBeNull]
        private BuildableBehaviour GetNearestReachableBuildableOfNeedType(NavMeshAgent navMeshAgent, NeedType needType) {
            // regarde autour de toi
            // si tu trouve des trucs qui répondent à ton besoins, va au plus près
            // sinon regarde un peu loin autour de toi
            // si y a vraiment que dalle, dis qu'il y a rien
            
            for (int i = 5; i < 100; i+=20) {
                Collider[] buildablesColliders = new Collider[5];

                var buildablesFound = Physics.OverlapSphereNonAlloc(navMeshAgent.transform.position, i, buildablesColliders);

                if (buildablesFound == 0) continue;
                
                foreach (var buildableCollider in buildablesColliders) {
                    var buildableBehaviour = buildableCollider.GetComponent<BuildableBehaviour>();
                    
                    if (buildableBehaviour.visitorsBuildablePropertiesScriptableObject == null) continue;
                    
                    if (!buildableBehaviour.isBreaked &&
                        !buildableBehaviour.isVisitorTargeted &&
                        CanReachBuildable(navMeshAgent, buildableBehaviour.transform.position) &&
                        buildableBehaviour.visitorsBuildablePropertiesScriptableObject.needType == needType) {
                        return buildableBehaviour;
                    }
                }
            }

            return null;
        }
        
        private List<BuildableBehaviour> GetReachableBuildables(NavMeshAgent navMeshAgent) {
            var buildablesBehaviours = FindObjectsOfType<BuildableBehaviour>();

            var reachablesBuildables = new List<BuildableBehaviour>();

            foreach (var buildableBehaviour in buildablesBehaviours) {
                if (!buildableBehaviour.isBreaked && !buildableBehaviour.isPirateTargeted) {
                    if (CanReachBuildable(navMeshAgent, buildableBehaviour.transform.position))
                        reachablesBuildables.Add(buildableBehaviour);
                }
            }

            return reachablesBuildables;
        }
        
        private bool CanReachBuildable(NavMeshAgent navMeshAgent, Vector3 buildablePosition) {
            if (NavMesh.SamplePosition(buildablePosition, out var navMeshHit, 2, 1)) {
                var path = new NavMeshPath();
                if (navMeshAgent.CalculatePath(navMeshHit.position, path)) {
                    if (path.status == NavMeshPathStatus.PathComplete) {
                        return true;
                    }
                }
            }

            return false;
        }

        public Transform GetRandomSasSpawnPoint() {
            return spawnTransforms[Random.Range(0, spawnTransforms.Length)];
        }
    }
}
