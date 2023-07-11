using System;
using System.Collections.Generic;
using Data;
using Game.CommandRoomPanelControls;
using Items;
using TMPro;
using UnityEngine;

namespace Monkeys.MiniChimps {
    public class MiniChimpDialogue : MonoBehaviour {
        [SerializeField] private GibberishAudioDataScriptableObject gibberishAudioDataScriptableObject;
        [SerializeField] private AudioSource miniChimpAudioSource;

        [SerializeField] private GameObject bubbleGameObject;
        [SerializeField] private TextMeshProUGUI minichimpText;
        
        private Queue<AudioClip> _clipsQueue;
        private Char[] charactersArray;

        private void Start() {
            _clipsQueue = new Queue<AudioClip>();
        }

        public void Play() {
            if (GetComponent<ItemStatic>().itemStaticType == ItemStaticType.MINI_CHIMP_COMMAND_ROOM) {
                CommandRoomControlPanelsManager.Instance.SetMiniChimpDialogue(ObjectsReference.Instance.advancements.GetBestAdvancement());
            }

            if (_clipsQueue.Count > 0) return;
            
            bubbleGameObject.SetActive(true);
            
            charactersArray = minichimpText.text.ToLower().ToCharArray();

            for (var i = 0; i < charactersArray.Length/4; i++) {
                _clipsQueue.Enqueue(gibberishAudioDataScriptableObject.characterToClip.ContainsKey(charactersArray[i]) 
                    ? gibberishAudioDataScriptableObject.characterToClip[charactersArray[i]]
                    : gibberishAudioDataScriptableObject.characterToClip['f']);
            }
        }

        private void Update() {
            if (miniChimpAudioSource.isPlaying) return;
            
            if (_clipsQueue.Count == 1) {
                if (GetComponent<ItemStatic>().itemStaticType == ItemStaticType.MINI_CHIMP_COMMAND_ROOM) {
                    ObjectsReference.Instance.uIadvancements.SetBestAdvancement();
                    // Invoke(nameof(HideBubble), 4);
                }
            }

            if (_clipsQueue.Count <= 0) return;
            
            miniChimpAudioSource.clip = _clipsQueue.Dequeue();
            miniChimpAudioSource.volume = ObjectsReference.Instance.audioManager.voicesLevel;
            miniChimpAudioSource.Play();
        }

        public void HideBubble() {
            bubbleGameObject.SetActive(false);
        }
    }
}
