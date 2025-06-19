namespace InGame.Inventories {
    public class BuildablesInventory : Inventory {
        public void UnlockBuildablesTier(BuildableType[] buildableTypes) {
            // show buildable button in inventory
            ObjectsReference.Instance.uInventoriesManager.ActivateBuildablesInventory();
            
            foreach (var buildableType in buildableTypes) {
                ObjectsReference.Instance.bananaManUiBlueprintsInventory.Allow(buildableType);
            }
        }
    }
}