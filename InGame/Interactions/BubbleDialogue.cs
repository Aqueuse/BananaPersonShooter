using System;
using System.Collections.Generic;
using System.Linq;
using Audio.Data;
using TMPro;
using UI.InGame;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace InGame.Interactions {
    public class BubbleDialogue : MonoBehaviour {
        [SerializeField] private RectTransform visualTransform;

        [SerializeField] private GibberishAudioClipsScriptableObject gibberishAudioClipsScriptableObject;
        [SerializeField] private AudioSource bubbleAudioSource;

        [SerializeField] private TextMeshPro bubbleText;
        private UInteraction uInteraction;

        private Transform _cameraTransform;
        private Transform _bananaManTransform;
        private float _bananaManDistance;

        private Queue<String> _wordsQueue;
        private List<String> wordsToShow;
        private Char nextCharacter;

        [SerializeField] private LocalizeStringEvent _bubbleLocalizeStringEvent;
        [SerializeField] private GenericDictionary<dialogueSet, LocalizedString[]> dialogueTextByAdvancement;
        
        public int dialogueIndex;
        public bool dialogueShown;

        public dialogueSet thisBubbleDialogueSet;
        private int itemsLayer;

        private void OnEnable() {
            _wordsQueue = new Queue<string>();
            itemsLayer = LayerMask.NameToLayer("Interaction");
            uInteraction = GetComponent<UInteraction>();

            _cameraTransform = ObjectsReference.Instance.mainCamera.transform;
            _bananaManTransform = ObjectsReference.Instance.bananaMan.transform;
        }

        public void SetBubbleDialogue(dialogueSet dialogueSet) {
            thisBubbleDialogueSet = dialogueSet;
            _bubbleLocalizeStringEvent.StringReference = dialogueTextByAdvancement[thisBubbleDialogueSet][0];
        }

        public dialogueSet GetCurrentDialogueSet() {
            return thisBubbleDialogueSet;
        }

        public void PlayDialogue() {
            _wordsQueue.Clear();

            if (dialogueIndex < dialogueTextByAdvancement[thisBubbleDialogueSet].Length-1) dialogueIndex += 1;
            else {
                dialogueIndex = 0;
            }

            _bubbleLocalizeStringEvent.StringReference = dialogueTextByAdvancement[thisBubbleDialogueSet][dialogueIndex];
            
            Play();
        }

        public void Play() {
            if (_wordsQueue.Count > 0) return;

            var words = bubbleText.text.Split(" ").ToList();

            foreach (var word in words) {
                _wordsQueue.Enqueue(word);
            }

            wordsToShow = new List<string>();
            bubbleText.text = "";
        }

        private void Update() {
            _bananaManDistance = Vector3.Distance(transform.position, _bananaManTransform.position);

            if (_bananaManDistance < 4.5f) {
                EnableBubble();
            }

            else {
                DisableBubble();
            }
            
            transform.LookAt(_cameraTransform);
            
            if (bubbleAudioSource.isPlaying) return;
            
            if (_wordsQueue.Count <= 0) return;

            var nextWord = _wordsQueue.Dequeue();
            wordsToShow.Add(nextWord);
            
            nextCharacter = nextWord.ToLower().Length > 0 ? nextWord.ToLower()[0] : 'f';
            
            if (gibberishAudioClipsScriptableObject.characterToClip.TryGetValue(
                    nextCharacter, 
                    out var clip)
            ) {
                bubbleAudioSource.clip = clip;
            }
            else {
                bubbleAudioSource.clip = gibberishAudioClipsScriptableObject.characterToClip['f'];
            }
            bubbleAudioSource.volume = ObjectsReference.Instance.audioManager.voicesLevel;
            bubbleAudioSource.Play();

            bubbleText.text = string.Join(" ", wordsToShow);
        }

        public void ShowBubble() {
            visualTransform.localScale = Vector3.one;
        }

        public void HideBubble() {
            visualTransform.localScale = Vector3.zero;
        }

        public void EnableBubble() {
            ShowBubble();
            gameObject.layer = itemsLayer;
        }

        public void DisableBubble() {
            HideBubble();
            uInteraction.HideUI();
            gameObject.layer = 0; // default
        }
    }
}
