using UnityEngine;
using Random = UnityEngine.Random;

namespace MiniChimps {
    public class HautParleurs : MonoBehaviour {
        [SerializeField] private AudioSource[] hautParleurs;

        [SerializeField] private AudioClip[] miniChimpsAdverts;
    
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
    }
}