    using System;

using Enums;
    using Game;
    using UnityEngine;
using Random = UnityEngine.Random;

namespace Audio {
    public class AudioManager : MonoSingleton<AudioManager> {
        [SerializeField] private AudioSource audioEffectsSource;
        [SerializeField] private AudioSource audioAmbianceSource;
        [SerializeField] private AudioSource audioMusicsSource;
        
        [SerializeField] private AudioSource audioFootstepsSource;

        [SerializeField] public GenericDictionary<EffectType, AudioDataScriptableObject> audioEffectsDictionnary;
        [SerializeField] private GenericDictionary<AmbianceType, AudioDataScriptableObject> audioAmbianceDictionnary;
        [SerializeField] private GenericDictionary<MusicType, AudioDataScriptableObject> audioMusicsDictionnary;
        [SerializeField] private GenericDictionary<FootStepType, AudioDataScriptableObject> audioFootStepsDictionnary;

        
        
        public float musicLevel = 0.1f;
        public float ambianceLevel = 0.1f;
        public float effectsLevel = 0.1f;
        public float voicesLevel = 0.1f;

        public FootStepType footStepType;

        private void Start() {
            audioFootstepsSource.loop = false;
        }

        public void SetMusiqueBySceneName(string sceneName) {
            switch (sceneName) {
                case "HOME":
                    StopAudioSource(AudioSourcesType.AMBIANCE);
                    PlayMusic(MusicType.HOME, false);
                    break;
                
                case "MAP01":
                    PlayAmbiance(AmbianceType.MAP01);
                    PlayMusic(MusicType.MAP01, false);
                    break;
                    
                case "COROLLE":
                    PlayAmbiance(AmbianceType.MAP01);
                    PlayMusic(MusicType.MAP01, false);
                    break;
                
                case "COMMANDROOM":
                    StopAudioSource(AudioSourcesType.AMBIANCE);
                    StopAudioSource(AudioSourcesType.MUSIC);
                    break;
            }  // 7 secondes 955  / 39 secondes 775
        }

        public void StopAudioSource(AudioSourcesType audioSourceType) {
            switch (audioSourceType) {
                case AudioSourcesType.AMBIANCE:
                    audioAmbianceSource.loop = false;
                    audioAmbianceSource.Stop();
                    break;
                case AudioSourcesType.MUSIC:
                    audioMusicsSource.Stop();
                    break;
                case AudioSourcesType.EFFECT:
                    audioEffectsSource.Stop();
                    break;
                case AudioSourcesType.FOOTSTEPS:
                    audioFootstepsSource.Stop();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(audioSourceType), audioSourceType, null);
            }
        }

        public void PlayMusic(MusicType type, bool hasIntro) {
            var audioData = audioMusicsDictionnary[type];

            if (hasIntro) {

            }

            else {

                audioMusicsSource.clip = audioData.clip[0];
                audioMusicsSource.volume = audioData.volume * musicLevel;

                audioMusicsSource.loop = audioData.IsLooping;
                audioMusicsSource.Play();
            }
        }

        private void PlayAmbiance(AmbianceType ambianceType) {
            var audioData = audioAmbianceDictionnary[ambianceType];

            audioAmbianceSource.volume = ambianceLevel;
            audioAmbianceSource.clip = audioData.clip[Random.Range(0, audioData.clip.Length)];
            audioAmbianceSource.loop = true;
            
            audioAmbianceSource.Play();
        }

        public void PlayEffect(EffectType effectType, ulong delay) {
            var audioData = audioEffectsDictionnary[effectType]; 
    
            audioEffectsSource.volume = effectsLevel;
            audioEffectsSource.clip = audioData.clip[Random.Range(0, audioData.clip.Length)];
            audioEffectsSource.loop = audioData.IsLooping;

            audioEffectsSource.PlayDelayed(delay);
        }
        
        public void PlayFootStepOneShot() {
            if (GameManager.Instance.isGamePlaying) {
                var audioData = audioFootStepsDictionnary[footStepType];
                if (!audioFootstepsSource.isPlaying) {
                    audioFootstepsSource.volume = effectsLevel-0.2f;
                    audioFootstepsSource.PlayOneShot(audioData.clip[Random.Range(0, audioData.clip.Length)]);
                }
            }
        }

        public void PlayFootstep() {
            if (GameManager.Instance.isGamePlaying) {
                var audioData = audioFootStepsDictionnary[footStepType];
                audioFootstepsSource.volume = effectsLevel-0.2f;
                audioFootstepsSource.clip = audioData.clip[Random.Range(0, audioData.clip.Length)]; 
                audioFootstepsSource.Play();
            }
        }
        
        public void SetVolume(AudioSourcesType audioSourcesType, float level) {
            switch (audioSourcesType) {
                case AudioSourcesType.AMBIANCE:
                    ambianceLevel = level;
                    audioAmbianceSource.volume = ambianceLevel;
                    break;
                case AudioSourcesType.MUSIC:
                    musicLevel = level;
                    audioMusicsSource.volume = musicLevel;
                    break;
                case AudioSourcesType.EFFECT:
                    effectsLevel = level;
                    audioEffectsSource.volume = effectsLevel;
                    break;
                case AudioSourcesType.VOICE:
                    voicesLevel = level;
                    break;
            }
        }

        public void TestEffectLevel() {
            PlayEffect(EffectType.BUTTON_INTERACTION, 0);
        }
    }
}