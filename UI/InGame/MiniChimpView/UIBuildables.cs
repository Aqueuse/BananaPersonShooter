using UnityEngine;

namespace UI.InGame.MiniChimpView {
    public enum BuildableCategory {
        FUN,
        HUNGER,
        REST,
        KNOWLEDGE,
        SOUVENIR,
        OTHER
    }
    
    public class UIBuildables : MonoBehaviour {
        [SerializeField] private GenericDictionary<BuildableCategory, GameObject> buildablesSubMenuByBuildablesType;

        private void ShowSubMenu(BuildableCategory buildableCategory) {
            if (buildablesSubMenuByBuildablesType[buildableCategory].activeInHierarchy) {
                buildablesSubMenuByBuildablesType[buildableCategory].SetActive(false);
                return;
            }
            
            foreach (var buildableSubmenu in buildablesSubMenuByBuildablesType) {
                buildableSubmenu.Value.SetActive(false);
            }

            buildablesSubMenuByBuildablesType[buildableCategory].SetActive(true);
        }
        
        public void ShowFunSubMenu() { ShowSubMenu(BuildableCategory.FUN); }
        public void ShowHungerSubMenu() { ShowSubMenu(BuildableCategory.HUNGER); }
        public void ShowRestSubMenu() { ShowSubMenu(BuildableCategory.REST); }
        public void ShowKnowledgeSubMenu() { ShowSubMenu(BuildableCategory.KNOWLEDGE); }
        public void ShowSouvenirSubMenu() { ShowSubMenu(BuildableCategory.SOUVENIR); }
        public void ShowOtherSubMenu() { ShowSubMenu(BuildableCategory.OTHER); }

        public void Repair() {
            ObjectsReference.Instance.miniChimpViewMode.StartRepairing();
        }
        
        public void Destroy() {
            ObjectsReference.Instance.miniChimpViewMode.StartHarvesting();
        }
    }
}
