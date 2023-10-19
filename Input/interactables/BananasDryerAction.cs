using Gestion.Buildables;
using UnityEngine;

namespace Input.interactables {
    public class BananasDryerAction : MonoBehaviour {
        public BananasDryer activeBananaDryer;
        
        void Update() {
            if (activeBananaDryer == null) return;

            AddPeel();
            TakeFabric();
        }

        private void AddPeel() { // Q => A
            if (UnityEngine.Input.GetKeyDown(KeyCode.Q) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton2)) activeBananaDryer.AddBananaPeel();
        }

        private void TakeFabric() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.E) || UnityEngine.Input.GetKeyDown(KeyCode.JoystickButton3)) activeBananaDryer.GiveFabric();
        }

    }
}
