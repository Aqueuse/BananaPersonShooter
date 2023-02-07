using UnityEngine;

namespace VFX {
    public class SpaceshipExplosion : MonoBehaviour {
        [SerializeField] private GameObject[] explodedParts;
        
        public void Explode() {
            foreach (var explodedPart in explodedParts) {
                explodedPart.GetComponent<Rigidbody>().isKinematic = false;
                explodedPart.GetComponent<Rigidbody>().AddExplosionForce(10, transform.position, 100);
            }
        }
    }
}
