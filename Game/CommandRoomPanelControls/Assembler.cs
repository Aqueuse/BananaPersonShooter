using Interactions;
using UI.InGame;
using UI.InGame.CommandRoomControlPanels;
using UnityEngine;

namespace Game.CommandRoomPanelControls {
    public class Assembler : MonoBehaviour {
        [SerializeField] private GenericDictionary<GameObject, Transform> bananaGunPiecesRepairedPositionByBananaGunPieceGameObject;
        [SerializeField] private GameObject bananaGunRepaired;
        [SerializeField] private GameObject bananaGunPieces;
        
        [SerializeField] private MeshRenderer assemblerZoneMeshRenderer;

        [SerializeField] private Color activatedEmissionColor;
        [SerializeField] private Light assemblerSpotLight;

        [SerializeField] private UIassembler uIassembler;

        private AudioSource assemblerAudioSource;

        private int bananaGunPiecesRepaired;
        private static readonly int EmissionColor = Shader.PropertyToID("_emission_color");
        private static readonly int Emission = Shader.PropertyToID("_emission");

        private void Start() {
            assemblerAudioSource = GetComponent<AudioSource>();
            assemblerAudioSource.volume = ObjectsReference.Instance.audioManager.effectsLevel;

            bananaGunPieces.SetActive(!ObjectsReference.Instance.bananaMan.tutorialFinished);
            assemblerSpotLight.enabled = !ObjectsReference.Instance.bananaMan.tutorialFinished;
        }
        
        private void OnTriggerEnter(Collider other) {
            if (!ObjectsReference.Instance.bananaMan.tutorialFinished) {
                if (other.TryGetComponent(out Grabbable grabbable)) {
                    if (grabbable.grabbablePieceType == GrabbablePieceType.BANANA_GUN) {
                        if (bananaGunPiecesRepairedPositionByBananaGunPieceGameObject.ContainsKey(other.gameObject)) {
                            assemblerAudioSource.pitch += 0.02f;
                            SetAssemblerAudioVolume(ObjectsReference.Instance.audioManager.effectsLevel);
                            if (!assemblerAudioSource.isPlaying) assemblerAudioSource.Play();

                            assemblerZoneMeshRenderer.enabled = true;
                            assemblerZoneMeshRenderer.material.SetColor(EmissionColor, activatedEmissionColor);
                            assemblerZoneMeshRenderer.material.SetFloat(Emission, 1f);

                            var bananaGunPiece = other.gameObject;
                
                            ObjectsReference.Instance.grab.Release();
                            bananaGunPiece.layer = 0;

                            Destroy(bananaGunPiece.GetComponent<Rigidbody>());
                            Destroy(bananaGunPiece.GetComponent<BoxCollider>());
                
                            bananaGunPiece.transform.position = bananaGunPiecesRepairedPositionByBananaGunPieceGameObject[bananaGunPiece].position;
                            bananaGunPiece.transform.rotation = bananaGunPiecesRepairedPositionByBananaGunPieceGameObject[bananaGunPiece].rotation;
                            
                            bananaGunPiecesRepaired += 1;
                            if (uIassembler.assemblerMode != AssemblerMode.BANANA_GUN) uIassembler.SwitchToBananaGunReparationMode();
                            uIassembler.SetBananaGunPiecesQuantity(bananaGunPiecesRepaired);
                    
                            if (bananaGunPiecesRepaired == bananaGunPiecesRepairedPositionByBananaGunPieceGameObject.Count) {
                                assemblerAudioSource.Stop();
                                uIassembler.SwitchToIdleMode();
                                
                                DesactivateBananaGunPieces();
                                bananaGunRepaired.SetActive(true);
                                HideAssemblerActivatedZone();
                                CommandRoomControlPanelsManager.Instance.miniChimp.bubbleDialogue.SetBubbleDialogue(dialogueSet.REPAIRED_BANANA_GUN);
                                CommandRoomControlPanelsManager.Instance.miniChimp.bubbleDialogue.PlayDialogue();
                            }
                        }
                    }
                }
            }
        }

        public void HideBananaGunInteractableGameObject() {
            bananaGunRepaired.SetActive(false);
        }

        private void DesactivateBananaGunPieces() {
            bananaGunPieces.SetActive(false);
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
