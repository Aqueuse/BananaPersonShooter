using Player;
using UnityEngine;

namespace Particles {
    public class BananaParticuleSystemManager : MonoSingleton<BananaParticuleSystemManager> {
        [SerializeField] private ParticleSystem yellowParticleSystem;
        [SerializeField] private ParticleSystem greenParticleSystem;
        [SerializeField] private ParticleSystem redParticleSystem;
        [SerializeField] private ParticleSystem blueParticleSystem;
        [SerializeField] private ParticleSystem marronParticleSystem;

        public ParticleSystem activeParticleSystem;
        public ParticleSystem.EmissionModule ActiveEmissionModule;

        private void Start() {
            activeParticleSystem = yellowParticleSystem;
            ActiveEmissionModule = activeParticleSystem.emission;
        }

        private void FixedUpdate() {
            if (BananaMan.Instance.isShooting) {
                ActiveEmissionModule.enabled = true;
            }
        }

        public void SetBananaParticleSystem(BananaType bananaType) {
            ActiveEmissionModule.enabled = false;
        
            switch (bananaType) {
                case BananaType.BARANGAN:
                    activeParticleSystem = marronParticleSystem;
                    break;
                case BananaType.BLUE_JAVA:
                    activeParticleSystem = blueParticleSystem;
                    break;
                case BananaType.RED:
                    activeParticleSystem = redParticleSystem;
                    break;
                case BananaType.MATOKE: 
                case BananaType.TINDOK:
                case BananaType.GOLD_FINGER:
                case BananaType.GROS_MICHEL:
                case BananaType.LADY_FINGER:
                case BananaType.PRAYING_HANDS:
                    activeParticleSystem = greenParticleSystem;
                    break;
                default:
                    activeParticleSystem = yellowParticleSystem;
                    break;
            }

            ActiveEmissionModule = activeParticleSystem.emission;
            ActiveEmissionModule.enabled = true;
        
        }
    }
}
