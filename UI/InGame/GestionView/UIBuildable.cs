using InGame.Items.ItemsProperties.Buildables;
using UnityEngine;

namespace UI.InGame.MiniChimpView {
    public class UIBuildable : MonoBehaviour {
        [SerializeField] private BuildablePropertiesScriptableObject buildablePropertiesScriptableObject;

        public void Activate() {
            ObjectsReference.Instance.gestionViewMode.ActivateGhostByScriptableObject(buildablePropertiesScriptableObject);
        }
    }
}
