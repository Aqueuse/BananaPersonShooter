using UI.InGame;
using UnityEngine;

namespace Game.CommandRoomPanelControls {
    public class RepairStation : MonoBehaviour {
        [SerializeField] private GenericDictionary<GameObject, Transform> bananaGunPiecesRepairedPosition;
        [SerializeField] private GameObject bananaGunRepaired;
        [SerializeField] private GameObject bananaGunPieces;
        
        [SerializeField] private MeshRenderer repairZoneMeshRenderer;
        [SerializeField] private Color activatedColor;
        [SerializeField] private Color baseColor;

        [SerializeField] private UIrepairStation _uIrepairStation;

        private static readonly int colorPropertie = Shader.PropertyToID("_Color");

        private int bananaGunPiecesRepaired;
        private bool _bananaGunRepaired;

        private void Start() {
            if (ObjectsReference.Instance.gameData.bananaManSavedData.playerAdvancements.Contains(AdvancementState.GET_BANANAGUN)) {
                _bananaGunRepaired = true;
                bananaGunPieces.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other) {
            // s'il est dans la liste, on le met à sa place définitive
            if (!_bananaGunRepaired) {
                if (bananaGunPiecesRepairedPosition.ContainsKey(other.gameObject)) {
                    repairZoneMeshRenderer.material.SetColor(colorPropertie, activatedColor);

                    var bananaGunPiece = other.gameObject;
                
                    ObjectsReference.Instance.itemsManager.Release();
                    bananaGunPiece.layer = 0;

                    Destroy(bananaGunPiece.GetComponent<Rigidbody>());
                    Destroy(bananaGunPiece.GetComponent<BoxCollider>());
                
                    bananaGunPiece.transform.position = bananaGunPiecesRepairedPosition[bananaGunPiece].position;
                    bananaGunPiece.transform.rotation = bananaGunPiecesRepairedPosition[bananaGunPiece].rotation;

                    bananaGunPiecesRepaired += 1;
                    if (_uIrepairStation.repairStationMode != RepairStationMode.BANANA_GUN) _uIrepairStation.SwitchToBananaGunReparationMode();
                    _uIrepairStation.SetBananaGunPiecesQuantity(bananaGunPiecesRepaired);
                    
                    if (bananaGunPiecesRepaired == 8) {
                        repairZoneMeshRenderer.material.SetColor(colorPropertie, baseColor);

                        // setActive false all the pieces and setActive true the grabbable banana Gun
                        DesactivateBananaGunPieces();
                        bananaGunRepaired.SetActive(true);
                        _bananaGunRepaired = true;
                        
                        _uIrepairStation.SwitchToIdleMode();
                        
                        ObjectsReference.Instance.gameData.bananaManSavedData.playerAdvancements.Add(AdvancementState.GET_BANANAGUN);
                    }
                }
            }
        }

        private void DesactivateBananaGunPieces() {
            bananaGunPieces.SetActive(false);
        }
    }
}
