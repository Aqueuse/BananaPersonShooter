using Audio;
using TerrainDetection;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Monkeys {
    public class MonkeySounds : MonoBehaviour {
        [SerializeField] private AudioDataScriptableObject monkeyQuickRoarSounds;
        [SerializeField] private AudioDataScriptableObject monkeyNeutralSounds;
        [SerializeField] private AudioDataScriptableObject boomSound;
        
        [SerializeField] private AudioSource audioVocalSource;
        [SerializeField] private AudioSource audioFootstepsSource;

        [SerializeField] private LayerMask surfacesLayerMask;

        private AudioManager _audioManager;
        
        private RaycastHit _raycastHit;
        private FootStepType _footStepType;

        private void Start() {
            _audioManager = ObjectsReference.Instance.audioManager;
        }

        private void Update() {
            if (!ObjectsReference.Instance.gameManager.isGamePlaying) return;
            if (!Physics.Raycast(transform.position, -transform.up, out _raycastHit, 5, layerMask:surfacesLayerMask)) return;

            var terrainType = _raycastHit.transform.gameObject.GetComponent<TerrainType>(); 
            _footStepType = terrainType != null ? terrainType.footStepType : FootStepType.ROCK;
        }

        public void PlayQuickRoarSound() {
            audioVocalSource.clip = monkeyQuickRoarSounds.clip[Random.Range(0, monkeyQuickRoarSounds.clip.Length)];
            audioVocalSource.volume = monkeyQuickRoarSounds.volume;
            audioVocalSource.Play();
        }

        public void PlayBoomSound() {
            audioVocalSource.clip = boomSound.clip[0];
            audioVocalSource.volume = boomSound.volume;
            audioVocalSource.Play();
        }

        public void PlayRoarsSounds() {
            Invoke(nameof(PlayQuickRoarSound), Random.Range(10, 30));
        }

        public void PlayQuickMonkeySounds() {
            Invoke(nameof(MonkeyRitournelle), Random.Range(10, 20));
        }
        
        private void MonkeyRitournelle() {
            float randomTime = Random.Range(10, 20);
        
            audioVocalSource.clip = monkeyNeutralSounds.clip[Random.Range(0, monkeyNeutralSounds.clip.Length)];
            audioVocalSource.volume = monkeyNeutralSounds.volume;
            audioVocalSource.Play();
        
            Invoke(nameof(MonkeyRitournelle), randomTime);
        }

        public void PlayFootstep() {
            var audioData = _audioManager.audioFootStepsDictionnary[_footStepType];
            audioFootstepsSource.volume = _audioManager.effectsLevel-0.2f;
            audioFootstepsSource.clip = audioData.clip[Random.Range(0, audioData.clip.Length)]; 
            audioFootstepsSource.Play();
        }
    }
}
