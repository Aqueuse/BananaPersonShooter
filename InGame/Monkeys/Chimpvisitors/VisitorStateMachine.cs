using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using InGame.Items.ItemsProperties.Buildables.VisitorsBuildable;
using UnityEngine;

namespace InGame.Monkeys.Chimpvisitors {
    public class VisitorStateMachine : MonoBehaviour {
        [SerializeField] private VisitorBehaviour visitorBehaviour;
        public TouristState touristState;
        
        public SearchBuildableToUse searchBuildableToUse;
        private BuildableBehaviour buildableToReach;

        private void Update() {
            if (visitorBehaviour.HasArrivedToDestination()) {
                if (touristState == TouristState.GO_TO_COROLLE_CENTER) {
                    searchNeedToFill();
                }

                if (touristState == TouristState.GO_FILL_NEED) {
                    touristState = TouristState.FILLING_NEED;
                    PlayNeedAnimation();
                }
                
                if (touristState == TouristState.GO_TO_WAITING_LINE) {
                    
                }

                if (touristState == TouristState.GO_TO_ENDMAP) {
                    
                }

                if (touristState == TouristState.GO_BACK_TO_SPACESHIP) {
                     visitorBehaviour.monkeyMenData.destination = visitorBehaviour.associatedSpaceshipBehaviour.visitorsSpawnPoint.position;
                     visitorBehaviour.SetDestination(visitorBehaviour.monkeyMenData.destination);
                }
            }
            
        }

        private void goToTheCorolleCenter() {
            visitorBehaviour.SetDestination(ObjectsReference.Instance.gameManager.spawnPointsBySpawnType[SpawnPoint.TP_COROLLE].position);
        }
        
        private void searchNeedToFill() {
            searchBuildableToUse.enabled = true;
        }

        private void goFillNeed() {
            visitorBehaviour.SetDestination(buildableToReach.transform.position);
        }
        
        private void searchMapToVisit() {
            
        }
        
        private void goToGuichetWaitingLine() {
            
        }

        private void visitMap() {
            
        }
        
        private void goBackToSpaceship() {
            
        }

        // go to the center of the corolle to look at the map

        // look around to see if their is a need to be fulfilled by a buildable
        public void NeedDetected(GameObject needSource) {
            if (visitorBehaviour.monkeyMenData.isSatisfied) return;
            
            var needLocation = needSource.transform.GetComponent<VisitorBuildableBehaviour>();

            if (needLocation.need != visitorBehaviour.monkeyMenData.need) return;
            if (needLocation.isOccupied) return;
            
            searchBuildableToUse.enabled = false;
            
            needLocation.enabled = true;
            needLocation.PrepareOccupation(visitorBehaviour.navMeshAgent, visitorBehaviour.monkeyMenData.need == NeedType.PILLAGE);
            visitorBehaviour.SetDestination(needSource.transform.position);
        }

        public void PlayNeedAnimation() {
            var buildableData = (VisitorsBuildablePropertiesScriptableObject)buildableToReach.buildablePropertiesScriptableObject;
                    
            if (buildableData.isAnimationLooping) {
                Invoke(nameof(FillNeed), 10);
            }

            visitorBehaviour.navMeshAgent.updateRotation = false;
            visitorBehaviour.animator.SetTrigger(buildableData.animatorParameterToActivate);
        }
        
        // else choose a random map and go to the corresponding guichet

        // follow the path of the map

        // go back to the spaceship
        public void FillNeed() {
            var buildableData = (VisitorsBuildablePropertiesScriptableObject)buildableToReach.buildablePropertiesScriptableObject;
            
            buildableToReach.isVisitorTargeted = false;
            
            searchBuildableToUse.enabled = true;

            visitorBehaviour.navMeshAgent.updateRotation = true;
            touristState = TouristState.GO_TO_COROLLE_CENTER;
        }
        
        public void FinishSatisfyNeed() {
            visitorBehaviour.monkeyMenData.isSatisfied = true;
        }

    }
}