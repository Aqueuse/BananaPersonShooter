namespace InGame.Inventories {
    public class BuildablesInventory : Inventory {
        public void AllowBuildable(BuildableType buildableType) {
            ObjectsReference.Instance.bananaManUiBlueprintsInventory.Allow(buildableType);
        }
    }
}