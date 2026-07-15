using System.Collections.Generic;
using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using InGame.Items.ItemsProperties.Buildables.VisitorsBuildable;
using InGame.WorldMaps;
using UnityEngine;

namespace InGame.Monkeys.Chimpvisitors {
    public class VisitorStateMachine : MonoBehaviour {
        [SerializeField] private VisitorBehaviour visitorBehaviour;
        public SearchBuildableToUse searchBuildableToUse;

        public TouristState touristState;
        private WorldMap chosenWorldMapToVisit;
        
        private BuildableBehaviour buildableToReach;

        private void Update() {
            if (visitorBehaviour.HasArrivedToDestination()) {
                if (touristState == TouristState.GO_TO_COROLLE_CENTER) {
                    touristState = TouristState.SEARCH_NEED;
                    searchNeedToFill();
                }

                if (touristState == TouristState.GO_FILL_NEED) {
                    touristState = TouristState.FILLING_NEED;
                    FillNeed();
                }
                
                if (touristState == TouristState.GO_TO_WAITING_LINE) {
                    touristState = TouristState.IN_WAITING_LINE;

                }

                if (touristState == TouristState.GO_TO_ENDMAP) {
                    touristState = TouristState.GO_BACK_TO_SPACESHIP;

                }

                if (touristState == TouristState.GO_BACK_TO_SPACESHIP) {
                    // remove the visitor from the game
                }
            }
        }

        public void goToTheCorolleCenter() {
            visitorBehaviour.SetDestination(ObjectsReference.Instance.gameManager.spawnPointsBySpawnType[SpawnPoint.TP_COROLLE].position);
        }
        
        private void searchNeedToFill() {
            searchBuildableToUse.enabled = true;
        }
        
        public void searchMapToVisit() {
            List<WorldMap> mapsOpen = new List<WorldMap>();
            
            // search all maps with a guichet open, choose one random
            foreach (var worldMap in ObjectsReference.Instance.meshReferenceScriptableObject.worldMaps) {
                if (worldMap.associatedGuichet.isOpen)
                    mapsOpen.Add(worldMap);
            }
            
            if (mapsOpen.Count == 0)
                goBackToSpaceship();

            else {
                chosenWorldMapToVisit = mapsOpen[Random.Range(0, mapsOpen.Count - 1)];
                goToGuichetWaitingLine(chosenWorldMapToVisit);
            }

            // if no map open, go back to spaceship
        }
        
        private void goToGuichetWaitingLine(WorldMap chosenWorldMap) {
            visitorBehaviour.SetDestination(chosenWorldMap.associatedGuichet.waitingListStart.position);
            touristState = TouristState.GO_TO_WAITING_LINE;
        }

        // called from Guichet if visitor is accepted
        public void visitMap() {
            visitorBehaviour.SetDestination(chosenWorldMapToVisit.mapEndPoint.position);
            touristState = TouristState.GO_TO_ENDMAP;
        }
        
        private void goBackToSpaceship() {
            visitorBehaviour.monkeyMenData.destination = visitorBehaviour.associatedSpaceshipBehaviour.visitorsSpawnPoint.position;
            visitorBehaviour.SetDestination(visitorBehaviour.monkeyMenData.destination);
        }
        
        // look around to see if their is a need to be fulfilled by a buildable
        private void PlayNeedAnimation() {
            var buildableData = (VisitorsBuildablePropertiesScriptableObject)buildableToReach.buildablePropertiesScriptableObject;
                    
            if (buildableData.isAnimationLooping) {
                Invoke(nameof(FillNeed), 10);
            }

            visitorBehaviour.navMeshAgent.updateRotation = false;
            visitorBehaviour.animator.SetTrigger(buildableData.animatorParameterToActivate);
        }
        
        public void FillNeed() {
            buildableToReach.isVisitorTargeted = false;
            
            PlayNeedAnimation();

            visitorBehaviour.navMeshAgent.updateRotation = true;
        }
        
        public void FinishSatisfyNeed() {
            visitorBehaviour.monkeyMenData.isSatisfied = true;
            touristState = TouristState.SEARCH_MAP_TO_VISIT;
            searchMapToVisit();
        }
    }
}