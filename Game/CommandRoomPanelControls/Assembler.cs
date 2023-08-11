using Items;
using UI.InGame;
using UnityEngine;

namespace Game.CommandRoomPanelControls {
    public class Assembler : MonoBehaviour {
        [SerializeField] private GenericDictionary<GameObject, Transform> bananaGunPiecesRepairedPositionByBananaGunPieceGameObject;
        [SerializeField] private GameObject bananaGunRepaired;
        [SerializeField] private GameObject bananaGunPieces;
        
        [SerializeField] private GameObject blueprintsDataGameObject;
        
        [SerializeField] private MeshRenderer assemblerZoneMeshRenderer;

        [SerializeField] private Color activatedEmissionColor;

        [SerializeField] private UIassembler uIassembler;

        private AudioSource assemblerAudioSource;

        private int bananaGunPiecesRepaired;

        private void Start() {
            assemblerAudioSource = GetComponent<AudioSource>();
            assemblerAudioSource.volume = ObjectsReference.Instance.audioManager.effectsLevel;

            bananaGunPieces.SetActive(!ObjectsReference.Instance.bananaMan.tutorialFinished);
            
            ShowBlueprintDataIfAvailable();
        }

        public void ShowBlueprintDataIfAvailable() {
            if (ObjectsReference.Instance.gameData.bananaManSavedData.hasFinishedTutorial) {
                if (ObjectsReference.Instance.buildablesManager.buildablesToGive.Count != ObjectsReference.Instance.buildablesManager.playerBlueprints.Count) ShowBlueprintsData(); 
            }
            else {
                HideBlueprintsData();
            }
        }

        private void OnTriggerEnter(Collider other) {
            if (!ObjectsReference.Instance.bananaMan.tutorialFinished) {
                var itemStaticClass = other.gameObject.GetComponent<ItemStatic>(); 
                
                if (itemStaticClass != null) {
                    if (itemStaticClass.itemStaticType == ItemStaticType.GRABBABLE_PIECE &&
                        itemStaticClass.GetComponent<Grabbable>().grabbablePieceType == GrabbablePieceType.BANANA_GUN) {
                        
                        if (bananaGunPiecesRepairedPositionByBananaGunPieceGameObject.ContainsKey(other.gameObject)) {
                            assemblerAudioSource.pitch += 0.02f;
                            CommandRoomControlPanelsManager.Instance.SetAssemblerVolume(ObjectsReference.Instance.audioManager.effectsLevel);
                            if (!assemblerAudioSource.isPlaying) assemblerAudioSource.Play();

                            assemblerZoneMeshRenderer.enabled = true;
                            assemblerZoneMeshRenderer.material.SetColor("_emission_color", activatedEmissionColor);
                            assemblerZoneMeshRenderer.material.SetFloat("_emission", 1f);

                            var bananaGunPiece = other.gameObject;
                
                            ObjectsReference.Instance.itemsManager.Release();
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
                            }
                        }
                    }
                }
            }
        }

        private void DesactivateBananaGunPieces() {
            bananaGunPieces.SetActive(false);
        }

        public void SetAssemblerAudioVolume(float level) {
            assemblerAudioSource.volume = level;
        }

        private void ShowBlueprintsData() {
            blueprintsDataGameObject.SetActive(true);
        }

        private void HideBlueprintsData() {
            blueprintsDataGameObject.SetActive(false);
        }

        public void HideAssemblerActivatedZone() {
            assemblerZoneMeshRenderer.enabled = false;
        }
    }
}
