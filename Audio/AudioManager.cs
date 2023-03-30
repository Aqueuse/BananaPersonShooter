    using System;
    using System.Collections.Generic;
    using System.Linq;
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

        private MusicType actualMusicType;
        private AudioClip lastClip;

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
                    PlayMusic(MusicType.HOME);
                    break;
                
                case "MAP01":
                    PlayAmbiance(AmbianceType.DRONE_MAP01);
                    PlayMusic(MusicType.JUNGLE_MAP01);
                    break;
                    
                case "COROLLE":
                    PlayAmbiance(AmbianceType.DRONE_COROLLE);
                    PlayMusic(MusicType.COROLLE);
                    break;
                
                case "COMMANDROOM":
                    PlayAmbiance(AmbianceType.DRONE_COMMANDROOM);
                    PlayMusic(MusicType.COMMANDROOM);
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

        public void PlayMusic(MusicType musicType) {
            actualMusicType = musicType;
            var audioData = audioMusicsDictionnary[musicType];

            audioMusicsSource.volume = audioData.volume * musicLevel;

            audioMusicsSource.loop = audioData.IsLooping;

            if (audioData.isRandomlySilenced) {
                Invoke(nameof(PlayMusicDelayed), 0);
            }

            else {
                audioMusicsSource.clip = audioData.clip[0];
                audioMusicsSource.Play();
            }
        }

        private void PlayMusicDelayed() {
            var audioData = audioMusicsDictionnary[actualMusicType];
            
            HashSet<AudioClip> audioClips = new HashSet<AudioClip>(audioData.clip);
            if (lastClip != null) audioClips.Remove(lastClip);

            // prevent playing the same song
            var randomClip = audioClips.ElementAt(Random.Range(0, audioClips.Count));
            audioMusicsSource.clip = randomClip;
            lastClip = randomClip;

            audioMusicsSource.Play();
            var clipDuration = audioMusicsSource.clip.length;
            
            Invoke(nameof(PlayMusicDelayed), Random.Range(clipDuration+30, clipDuration+60));
        }

        private void PlayAmbiance(AmbianceType ambianceType) {
            var audioData = audioAmbianceDictionnary[ambianceType];

            audioAmbianceSource.volume = ambianceLevel * audioMusicsSource.volume;
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