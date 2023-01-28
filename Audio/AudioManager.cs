    using System;
using E7.Introloop;
using Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Audio {
    public class AudioManager : MonoSingleton<AudioManager> {
        [SerializeField] private AudioSource audioEffectsSource;
        [SerializeField] private AudioSource audioVoicesSource;
        [SerializeField] private AudioSource audioAmbianceSource;
        [SerializeField] private AudioSource audioMusicsSource;
        [SerializeField] private AudioSource audioFootstepsSource;
        [SerializeField] private IntroloopPlayer audioIntroLoopMusicSource; // beautifully handle lopped music with intro

        [SerializeField] public GenericDictionary<EffectType, AudioDataScriptableObject> audioEffectsDictionnary;
        [SerializeField] private GenericDictionary<VoiceType, AudioDataScriptableObject> audioVoicesDictionnary;
        [SerializeField] private GenericDictionary<AmbianceType, AudioDataScriptableObject> audioAmbianceDictionnary;
        [SerializeField] private GenericDictionary<MusicType, AudioDataScriptableObject> audioMusicsDictionnary;
        [SerializeField] private GenericDictionary<FootStepType, AudioDataScriptableObject> audioFootStepsDictionnary;

        public float musicLevel = 0.1f;
        public float ambianceLevel = 0.1f;
        public float effectsLevel = 0.1f;

        private bool isAlreadyPlaying;
        public FootStepType footStepType;

        private void Start() {
            isAlreadyPlaying = false;
            audioFootstepsSource.loop = false;
        }

        public void StopAudioSource(AudioSourcesType audioSourceType) {
            switch (audioSourceType) {
                case AudioSourcesType.AMBIANCE:
                    audioAmbianceSource.loop = false;
                    audioAmbianceSource.Stop();
                    break;
                case AudioSourcesType.MUSIC:
                    audioMusicsSource.Stop();
                    audioIntroLoopMusicSource.Stop();
                    break;
                case AudioSourcesType.EFFECT:
                    audioEffectsSource.Stop();
                    break;
                case AudioSourcesType.VOICE:
                    audioVoicesSource.Stop();
                    break;
                case AudioSourcesType.FOOTSTEPS:
                    audioFootstepsSource.Stop();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(audioSourceType), audioSourceType, null);
            }

            isAlreadyPlaying = false;
        }

        public void PlayMusic(MusicType type, bool hasIntro) {
            var audioData = audioMusicsDictionnary[type];

            if (hasIntro) {
                audioIntroLoopMusicSource.DefaultIntroloopAudio = audioData.clipWithIntro;
                audioIntroLoopMusicSource.DefaultIntroloopAudio.Volume = audioData.volume * musicLevel;
                audioIntroLoopMusicSource.Play();
            }

            else {
                audioIntroLoopMusicSource.Stop();
                audioMusicsSource.clip = audioData.clip[0];
                audioMusicsSource.volume = audioData.volume * musicLevel;

                audioMusicsSource.loop = audioData.IsLooping;
                audioMusicsSource.Play();
            }
        }

        public void PlayAmbiance(AmbianceType ambianceType) {
            var audioData = audioAmbianceDictionnary[ambianceType];

            audioAmbianceSource.volume = ambianceLevel;
            audioAmbianceSource.clip = audioData.clip[Random.Range(0, audioData.clip.Length)];
            audioAmbianceSource.loop = true;
            
            audioAmbianceSource.Play();
        }

        public void PlayEffect(EffectType effectType) {
            var audioData = audioEffectsDictionnary[effectType]; 
    
            audioEffectsSource.volume = effectsLevel;
            audioEffectsSource.clip = audioData.clip[Random.Range(0, audioData.clip.Length)];
            audioEffectsSource.loop = audioData.IsLooping;

            audioEffectsSource.Play();
        }

        public void PlayLoopedEffect(EffectType effectType) {
            if (!isAlreadyPlaying) {
                var audioData = audioEffectsDictionnary[effectType]; 
    
                audioEffectsSource.volume = effectsLevel;
                audioEffectsSource.clip = audioData.clip[Random.Range(0, audioData.clip.Length)];
                audioEffectsSource.loop = true;

                audioEffectsSource.Play();
                isAlreadyPlaying = true;
            }
        }

        public void PlayVoice(VoiceType voiceType) {
            var audioData = audioVoicesDictionnary[voiceType];
            
            audioVoicesSource.volume = effectsLevel-0.2f;
            audioVoicesSource.clip = audioData.clip[Random.Range(0, audioData.clip.Length)];
            audioVoicesSource.loop = true;

            audioVoicesSource.Play();
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
                    effectsLevel = level;
                    audioEffectsSource.volume = effectsLevel;
                    break;
            }
        }

        public void TestEffectLevel() {
            PlayEffect(EffectType.BUTTON_ITERACTION);
        }
    }
}