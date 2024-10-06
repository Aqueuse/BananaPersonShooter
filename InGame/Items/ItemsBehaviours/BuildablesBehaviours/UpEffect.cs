using InGame.Monkeys.PhysicToNavMeshCoordinations;
using UnityEngine;

namespace InGame.Items.ItemsBehaviours.BuildablesBehaviours {
    public class UpEffect : MonoBehaviour {
        public void Activate(Rigidbody targetRigidbody, float force) {
            if (targetRigidbody.transform.TryGetComponent(out PhysicNavMeshCoordination physicNavMeshCoordination)) physicNavMeshCoordination.SwitchToPhysic();
            
            targetRigidbody.AddForce(transform.up * force, ForceMode.Acceleration);
        }
    }
}