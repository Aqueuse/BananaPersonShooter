using System.Collections.Generic;
using InGame.Interactions;
using UI.InGame.CommandRoomControlPanels;
using UnityEngine;

namespace InGame.CommandRoomPanelControls {
    public class Assembler : MonoBehaviour {
        [SerializeField] private List<GameObject> bananaGunRepairedPieces;
        
        [SerializeField] private GameObject bananaGunRepaired;
        
        [SerializeField] private MeshRenderer assemblerZoneMeshRenderer;

        [SerializeField] private Color activatedEmissionColor;
        [SerializeField] private Light assemblerSpotLight;

        [SerializeField] private UIassembler uIassembler;

        private AudioSource assemblerAudioSource;

        private int bananaGunPiecesRepairedQuantity;
        private static readonly int EmissionColor = Shader.PropertyToID("_emission_color");
        private static readonly int Emission = Shader.PropertyToID("_emission");

        public void Init() {
            assemblerAudioSource = GetComponent<AudioSource>();
            assemblerAudioSource.volume = ObjectsReference.Instance.audioManager.effectsLevel;
            assemblerSpotLight.enabled = !ObjectsReference.Instance.bananaMan.tutorialFinished;
        }
        
        private void OnTriggerEnter(Collider other) {
            if (other.TryGetComponent(out Grabbable grabbable)) {
                if (grabbable.grabbablePieceType == GrabbablePieceType.BANANA_GUN) {
                    assemblerAudioSource.pitch += 0.02f;
                    SetAssemblerAudioVolume(ObjectsReference.Instance.audioManager.effectsLevel);
                    if (!assemblerAudioSource.isPlaying) assemblerAudioSource.Play();

                    assemblerZoneMeshRenderer.enabled = true;
                    assemblerZoneMeshRenderer.material.SetColor(EmissionColor, activatedEmissionColor);
                    assemblerZoneMeshRenderer.material.SetFloat(Emission, 1f);
                    
                    ObjectsReference.Instance.grab.Release();
                    
                    bananaGunRepairedPieces[other.GetComponent<BananaGunPiece>().repairedPieceIndex].SetActive(true);

                    bananaGunPiecesRepairedQuantity = 0;
                    foreach (var bananaGunRepairedPiece in bananaGunRepairedPieces) {
                        if (bananaGunRepairedPiece.activeInHierarchy) bananaGunPiecesRepairedQuantity += 1;
                    }
                    
                    Destroy(other.gameObject);
                    
                    uIassembler.SwitchToBananaGunReparationMode();
                    uIassembler.SetBananaGunPiecesQuantity(bananaGunPiecesRepairedQuantity);
            
                    if (bananaGunPiecesRepairedQuantity == bananaGunRepairedPieces.Count) {
                        assemblerAudioSource.Stop();
                        uIassembler.SwitchToIdleMode();
                        
                        DesactivateBananaGunPieces();
                        bananaGunRepaired.SetActive(true);
                        HideAssemblerActivatedZone();
                    }
                }
            }
        }

        public void HideBananaGunInteractableGameObject() {
            bananaGunRepaired.SetActive(false);
        }

        private void DesactivateBananaGunPieces() {
            foreach (var bananaGunRepairedPiece in bananaGunRepairedPieces) {
                bananaGunRepairedPiece.SetActive(false);
            }
        }

        private void SetAssemblerAudioVolume(float level) {
            assemblerAudioSource.volume = level;
        }
        
        private void HideAssemblerActivatedZone() {
            assemblerZoneMeshRenderer.enabled = false;
            assemblerSpotLight.enabled = false;
        }
    }
}
