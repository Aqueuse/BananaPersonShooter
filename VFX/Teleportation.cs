using UnityEngine;

namespace VFX {
    public class Teleportation : MonoBehaviour {
        private ParticleSystem _teleportParticleSystem;
        private ParticleSystem.ShapeModule _shape;
        
        public void TeleportUp() {
            _teleportParticleSystem = GetComponent<ParticleSystem>();
            _shape = _teleportParticleSystem.shape;
            _shape.rotation = new Vector3(0, 0, 0);

            _teleportParticleSystem.Play();
        }

        public void TeleportDown() {
            _teleportParticleSystem = GetComponent<ParticleSystem>();
            _shape = _teleportParticleSystem.shape;
            _shape.rotation = new Vector3(0, 180, 0);

            _teleportParticleSystem.Play();
        }
    }
}
