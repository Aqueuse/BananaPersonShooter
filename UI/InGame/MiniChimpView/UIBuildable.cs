using InGame.Items.ItemsProperties.Buildables;
using UnityEngine;

namespace UI.InGame.MiniChimpView {
    public class UIBuildable : MonoBehaviour {
        [SerializeField] private BuildablePropertiesScriptableObject buildablePropertiesScriptableObject;

        public void Activate() {
            ObjectsReference.Instance.miniChimpViewMode.ActivateGhostByScriptableObject(buildablePropertiesScriptableObject);
        }
        
        public void SetDescriptionAndName() {
            ObjectsReference.Instance.uiDescriptionsManager.SetDescription(buildablePropertiesScriptableObject);
            ObjectsReference.Instance.uiManager.ShowMiniChimpBlock();
            ObjectsReference.Instance.uiBananaGun.SwitchToDescription();
        }
    }
}
