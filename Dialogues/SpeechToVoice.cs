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
        
        private Queue<AudioClip> clipsQueue;

        private void Start() {
            clipsQueue = new Queue<AudioClip>();
        }

        public void Play() {
            Char[] charactersArray = minichimpText.text.ToLower().ToCharArray();
            
            foreach (var character in charactersArray) {
                clipsQueue.Enqueue(gibberishAudioDataScriptableObject.characterToClip.ContainsKey(character) 
                    ? gibberishAudioDataScriptableObject.characterToClip[character]
                    : gibberishAudioDataScriptableObject.characterToClip['f']);
            }
        }

        private void Update() {
            if (miniChimpAudioSource.isPlaying || clipsQueue.Count <= 0) return;
            
            miniChimpAudioSource.clip = clipsQueue.Dequeue();
            miniChimpAudioSource.Play();
        }
    }
}
