using UnityEngine;

namespace Tweaks {
    public class TestsManager : MonoBehaviour {
        // Give player 100 raw material type
        public void Give100rawMaterialType(int materialType) { // la première option est select, donc on décale l'index de 1 pour fitter l'enum
            if (materialType == 0) return;
            ObjectsReference.Instance.bananaManRawMaterialInventory.AddQuantity(
                ObjectsReference.Instance.meshReferenceScriptableObject.rawMaterialPropertiesScriptableObjects[(RawMaterialType)materialType-1], 100);
        }
        
        // Give player 100 food type
        public void Give100foodType(int foodType) {
            if (foodType == 0) return;
            ObjectsReference.Instance.bananaManFoodInventory.AddQuantity(
                ObjectsReference.Instance.meshReferenceScriptableObject.foodPropertiesScriptableObjects[(FoodType)foodType-1], 100);
        }

        // Give player 100 ingredients type
        public void Give100ingredientsType(int ingredientsType) {
            if (ingredientsType == 0) return;
            ObjectsReference.Instance.bananaManIngredientsInventory.AddQuantity(
                ObjectsReference.Instance.meshReferenceScriptableObject.ingredientsPropertiesScriptableObjects[(IngredientsType)ingredientsType-1], 100);
        }

        // Give player 100 manufactured item type
        public void Give100manufacturedItemType(int manufacturedItemType) {
            if (manufacturedItemType == 0) return;
            ObjectsReference.Instance.bananaManManufacturedItemsInventory.AddQuantity(
                ObjectsReference.Instance.meshReferenceScriptableObject.manufacturedItemsPropertiesScriptableObjects[(ManufacturedItemsType)manufacturedItemType-1], 100);
        }

        // unlock tier (raw material type)
        public void UnlockTier(int rawMaterialType) { }
        
        // invoke meteorit rain
        public void InvokeMeteoritRain() { }

        // invoke spaceship (spaceship type)
        public void Invokespaceship(int spaceshipType) { }

        // clear spaceships
        public void ClearSpaceships() { }

        // clear visitors
        public void ClearVisitors() { }
    }
}