using UnityEngine;

namespace Tweaks {
    public class TestsManager : MonoBehaviour {
        // Give player 100 raw material type
        public void Give100rawMaterialType(int materialType) { }
        
        // Give player 100 food type
        public void Give100foodType(int foodType) { }

        // Give player 100 ingredients type
        public void Give100ingredientsType(int ingredientsType) { }

        // Give player 100 manufactured item type
        public void Give100manufacturedItemType(int manufacturedItemType) { }

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