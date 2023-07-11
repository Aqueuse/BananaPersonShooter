using Enums;
using Items;
using UI.InGame;
using UnityEngine;

namespace Game.CommandRoomPanelControls {
    public class Assembler : MonoBehaviour {
        [SerializeField] private GenericDictionary<GameObject, Transform> bananaGunPiecesRepairedPosition;
        [SerializeField] private GameObject bananaGunRepaired;
        [SerializeField] private GameObject bananaGunPieces;
        
        [SerializeField] private MeshRenderer assemblerZoneMeshRenderer;
        [SerializeField] private Color activatedColor;
        [SerializeField] private Color baseColor;

        [SerializeField] private UIassembler uIassembler;

        private AudioSource assemblerAudioSource;
        private static readonly int colorPropertie = Shader.PropertyToID("_Color");

        private int bananaGunPiecesRepaired;
        private bool _bananaGunRepaired;

        private void Start() {
            assemblerAudioSource = GetComponent<AudioSource>();
            assemblerAudioSource.volume = ObjectsReference.Instance.audioManager.effectsLevel;
            
            if (ObjectsReference.Instance.gameData.bananaManSavedData.playerAdvancements.Contains(AdvancementState.GET_BANANAGUN)) {
                _bananaGunRepaired = true;
                bananaGunPieces.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other) {
            if (!_bananaGunRepaired) {
                var itemStaticClass = other.gameObject.GetComponent<ItemStatic>(); 

                if (itemStaticClass != null) {
                    if (itemStaticClass.itemStaticType == ItemStaticType.GRABBABLE_PIECE &&
                        itemStaticClass.GetComponent<Grabbable>().grabbablePieceType == GrabbablePieceType.BANANA_GUN) {
                        
                        if (bananaGunPiecesRepairedPosition.ContainsKey(other.gameObject)) {
                            assemblerAudioSource.pitch += 0.02f;
                            CommandRoomControlPanelsManager.Instance.SetAssemblerVolume(ObjectsReference.Instance.audioManager.effectsLevel);
                            if (!assemblerAudioSource.isPlaying) assemblerAudioSource.Play();

                            assemblerZoneMeshRenderer.material.SetColor(colorPropertie, activatedColor);

                            var bananaGunPiece = other.gameObject;
                
                            ObjectsReference.Instance.itemsManager.Release();
                            bananaGunPiece.layer = 0;

                            Destroy(bananaGunPiece.GetComponent<Rigidbody>());
                            Destroy(bananaGunPiece.GetComponent<BoxCollider>());
                
                            bananaGunPiece.transform.position = bananaGunPiecesRepairedPosition[bananaGunPiece].position;
                            bananaGunPiece.transform.rotation = bananaGunPiecesRepairedPosition[bananaGunPiece].rotation;

                            
                            bananaGunPiecesRepaired += 1;
                            if (uIassembler.assemblerMode != AssemblerMode.BANANA_GUN) uIassembler.SwitchToBananaGunReparationMode();
                            uIassembler.SetBananaGunPiecesQuantity(bananaGunPiecesRepaired);
                    
                            if (bananaGunPiecesRepaired == 8) {
                                assemblerZoneMeshRenderer.material.SetColor(colorPropertie, baseColor);

                                // setActive false all the pieces and setActive true the grabbable banana Gun
                                DesactivateBananaGunPieces();
                                bananaGunRepaired.SetActive(true);
                                _bananaGunRepaired = true;
                                
                                assemblerAudioSource.Stop();
                                
                                uIassembler.SwitchToIdleMode();
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
    }
}
