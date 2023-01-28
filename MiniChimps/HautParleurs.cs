using Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MiniChimps {
    public class HautParleurs : MonoSingleton<HautParleurs> {
        [SerializeField] private AudioSource[] hautParleurs;

        [SerializeField] private AudioClip[] miniChimpsAdverts;

        [SerializeField] private GenericDictionary<MonkeyState, AudioClip> happinessLevelClips;
    
        private void Start() {
            Invoke(nameof(MiniChimpRitournelle), 200);
        }
 
        private void MiniChimpRitournelle() {
            float randomTime = Random.Range(300, 600);
        
            foreach (var hautParleur in hautParleurs) {
                hautParleur.clip = miniChimpsAdverts[Random.Range(0, miniChimpsAdverts.Length)];
                hautParleur.Play();
            }
        
            Invoke(nameof(MiniChimpRitournelle), randomTime);
        }

        public void PlayHappinessLevelClip(MonkeyState monkeyState) {
            foreach (var hautParleur in hautParleurs) {
                hautParleur.clip = happinessLevelClips[monkeyState];
                hautParleur.Play();
            }
        }
    }
}