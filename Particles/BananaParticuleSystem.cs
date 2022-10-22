using System.Collections.Generic;
using Player;
using UI;
using UnityEngine;

namespace Particles {
    public class BananaParticuleSystem : MonoBehaviour {
        List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();

        private void OnParticleTrigger() {
            int numEnter = GetComponent<ParticleSystem>().GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
            GetComponent<ParticleSystem>().SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

            BananaMan.Instance.resistance += numEnter;
            UIVitals.Instance.Set_Resistance(BananaMan.Instance.resistance);
        }
    }
}
