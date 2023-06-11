using System;
using System.Collections.Generic;
using Data;
using TMPro;
using UnityEngine;

namespace Dialogues {
    public class SpeechToVoice : MonoBehaviour {
        [SerializeField] private GibberishAudioDataScriptableObject gibberishAudioDataScriptableObject;
        [SerializeField] private AudioSource miniChimpAudioSource;

        [SerializeField] private TextMeshProUGUI minichimpText;
        
        private Queue<AudioClip> _clipsQueue;
        private Char[] charactersArray;

        private void Start() {
            _clipsQueue = new Queue<AudioClip>();
        }

        public void Play() {
            charactersArray = minichimpText.text.ToLower().ToCharArray();

            for (int i = 0; i < charactersArray.Length/4; i++) {
                _clipsQueue.Enqueue(gibberishAudioDataScriptableObject.characterToClip.ContainsKey(charactersArray[i]) 
                    ? gibberishAudioDataScriptableObject.characterToClip[charactersArray[i]]
                    : gibberishAudioDataScriptableObject.characterToClip['f']);
            }
        }

        private void Update() {
            if (miniChimpAudioSource.isPlaying || _clipsQueue.Count <= 0) return;
            
            miniChimpAudioSource.clip = _clipsQueue.Dequeue();
            miniChimpAudioSource.Play();
        }
    }
}
