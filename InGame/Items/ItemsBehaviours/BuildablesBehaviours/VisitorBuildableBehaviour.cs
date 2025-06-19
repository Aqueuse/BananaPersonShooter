using System;
using InGame.Monkeys.Chimpvisitors;
using UnityEngine;
using UnityEngine.AI;

namespace InGame.Items.ItemsBehaviours.BuildablesBehaviours {
    public class VisitorBuildableBehaviour : BuildableBehaviour {
        public NeedType need;
        public bool willBeBreaked;
        public bool isOccupied;
        public float occupationDuration;

        private NavMeshAgent occupyingNavmeshAgent;

        private void Update() {
            if (isOccupied && occupyingNavmeshAgent) {
                // check for arrival of the agent
                if (NavMeshAgentHasArrived()) {
                    Occupy();
                }
            }
        }

        private bool NavMeshAgentHasArrived() {
            if (occupyingNavmeshAgent.pathPending || occupyingNavmeshAgent.remainingDistance > occupyingNavmeshAgent.stoppingDistance)
                return false;

            // S'il croit être arrivé mais continue de glisser
            if (occupyingNavmeshAgent.hasPath && occupyingNavmeshAgent.velocity.sqrMagnitude > 0.05f)
                return false;

            return true;
        }

        public void PrepareOccupation(NavMeshAgent navMeshAgent, bool willBeBreaked) {
            occupyingNavmeshAgent = navMeshAgent;
            isOccupied = true;
            this.willBeBreaked = willBeBreaked;
        }

        private void Occupy() {
            Invoke(nameof(FreeLocation), occupationDuration);
        }

        public void FreeLocation() {
            try {
                if (willBeBreaked) {
                    // TODO : give some materials to the pirate
                    // TODO : return to state blueprint
                }

                occupyingNavmeshAgent.GetComponent<VisitorBehaviour>().FinishSatisfyNeed();

                isOccupied = false;
                enabled = false;
            }
            catch (Exception e) {
                Debug.Log(e);
                throw;
            }
        }
    }
}