using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Audio {
    public class AudioManager : MonoBehaviour {
        [SerializeField] private AudioSource audioEffectsSource;
        [SerializeField] private AudioSource audioAmbianceSource;
        [SerializeField] private AudioSource audioMusicsSource;
        
        [SerializeField] private AudioSource audioFootstepsSource;

        [SerializeField] public GenericDictionary<SoundEffectType, AudioDataScriptableObject> audioEffectsDictionnary;
        [SerializeField] private GenericDictionary<AmbianceType, AudioDataScriptableObject> audioAmbianceDictionnary;
        [SerializeField] private GenericDictionary<MusicType, AudioDataScriptableObject> audioMusicsDictionnary;
        public GenericDictionary<FootStepType, AudioDataScriptableObject> audioFootStepsDictionnary;

        private MusicType _actualMusicType;
        private AudioClip _lastClip;

        public float musicLevel = 0.1f;
        public float ambianceLevel = 0.1f;
        public float effectsLevel = 0.1f;
        public float voicesLevel = 0.1f;
        
        private void Start() {
            audioFootstepsSource.loop = false;
        }

        public void SetMusiqueAndAmbianceByRegion(RegionType regionName) {
            switch (regionName) {
                case RegionType.HOME:
                    StopAudioSource(AudioSourcesType.AMBIANCE);
                    PlayMusic(MusicType.HOME, 0);
                    break;
                
                case RegionType.MAP01:
                    PlayAmbiance(AmbianceType.DRONE_MAP01);
                    PlayMusic(MusicType.JUNGLE_MAP01, 1);
                    break;
                    
                case RegionType.COROLLE:
                    PlayAmbiance(AmbianceType.DRONE_COROLLE);
                    PlayMusic(MusicType.COROLLE, 1);
                    break;
            }
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

        public void PlayMusic(MusicType musicType, float delay) {
            _actualMusicType = musicType;
            var audioData = audioMusicsDictionnary[musicType];

            audioMusicsSource.volume = audioData.volume * musicLevel;

            audioMusicsSource.loop = audioData.isLooping;

            if (audioData.isRandomlySilenced) {
                Invoke(nameof(PlayMusicDelayed), 0);
            }

            else {
                audioMusicsSource.clip = audioData.clip[0];
                audioMusicsSource.time = 0;
                audioMusicsSource.PlayDelayed(delay);
            }
        }

        private void PlayMusicDelayed() {
            var audioData = audioMusicsDictionnary[_actualMusicType];
            
            var audioClips = new HashSet<AudioClip>(audioData.clip);
            if (_lastClip != null) audioClips.Remove(_lastClip);

            // prevent playing the same song
            if (audioClips.Count > 1) {
                var randomClip = audioClips.ElementAt(Random.Range(0, audioClips.Count));
                audioMusicsSource.clip = randomClip;
                _lastClip = randomClip;

                audioMusicsSource.Play();
                var clipDuration = audioMusicsSource.clip.length;
            
                Invoke(nameof(PlayMusicDelayed), Random.Range(clipDuration+30, clipDuration+60));
            }
            else {
                if (audioClips.Count > 0) {
                    audioMusicsSource.clip = audioClips.ElementAt(0);
                    var clipDuration = audioMusicsSource.clip.length;
                    Invoke(nameof(PlayMusicDelayed), clipDuration);
                }
            }
        }

        public void PlayAmbiance(AmbianceType ambianceType) {
            var audioData = audioAmbianceDictionnary[ambianceType];

            audioAmbianceSource.volume = ambianceLevel * audioMusicsSource.volume;
            audioAmbianceSource.clip = audioData.clip[Random.Range(0, audioData.clip.Length)];
            audioAmbianceSource.loop = true;
            
            audioAmbianceSource.Play();
        }

        public void PlayEffect(SoundEffectType soundEffectType, float delay) {
            var audioData = audioEffectsDictionnary[soundEffectType]; 
    
            audioEffectsSource.volume = effectsLevel;
            audioEffectsSource.clip = audioData.clip[Random.Range(0, audioData.clip.Length)];
            audioEffectsSource.loop = audioData.isLooping;

            audioEffectsSource.PlayDelayed(delay);
        }

        public void PlayFootstep() {
            if (ObjectsReference.Instance.gameManager.isGamePlaying) {
                var audioData = audioFootStepsDictionnary[ObjectsReference.Instance.footStepSurfaceDetector.footStepType];
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
            PlayEffect(SoundEffectType.BUTTON_INTERACTION, 0);
        }
    }
}