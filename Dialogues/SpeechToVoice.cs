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

        private void Start() {
            _clipsQueue = new Queue<AudioClip>();
        }

        public void Play() {
            Char[] charactersArray = minichimpText.text.ToLower().ToCharArray();
            
            foreach (var character in charactersArray) {
                _clipsQueue.Enqueue(gibberishAudioDataScriptableObject.characterToClip.ContainsKey(character) 
                    ? gibberishAudioDataScriptableObject.characterToClip[character]
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
