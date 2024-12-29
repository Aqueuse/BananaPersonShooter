using InGame.Interactions;
using InGame.Items.ItemsBehaviours;
using InGame.Items.ItemsBehaviours.BuildablesBehaviours;
using InGame.Items.ItemsProperties;
using InGame.Items.ItemsProperties.Buildables;
using InGame.Items.ItemsProperties.Dropped;
using Tags;
using UnityEngine;

namespace InGame.Player.BananaGunActions {
    public class Scan : MonoBehaviour {
        public GameObject targetedGameObject;

        [SerializeField] private LayerMask GestionViewSelectableLayerMask;

        private Mesh _targetedGameObjectMesh;
        private BananaType _targetType;

        private GenericDictionary<RawMaterialType, int> rawMaterialsWithQuantity;

        private Camera mainCamera;

        private Ray ray;
        private RaycastHit raycastHit;

        private GenericDictionary<RawMaterialType, int> craftingMaterials;
        private Regime regimeClass;

        private Tag gameObjectTagClass;
        private GAME_OBJECT_TAG gameObjectTag;
        
        private void Start() {
            mainCamera = Camera.main;
        }
    }
}