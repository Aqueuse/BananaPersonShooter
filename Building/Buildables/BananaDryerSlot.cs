using UnityEngine;

namespace Building.Buildables {
    public class BananaDryerSlot : MonoBehaviour {
        [SerializeField] private MeshRenderer bananaPeel;
        [SerializeField] private  MeshRenderer fabric;

        public void AddBananaPeel() {
            bananaPeel.enabled = true;

            Invoke(nameof(GiveFabric), 3);
        }
    
        public void GiveFabric() {
            bananaPeel.enabled = false;
            fabric.enabled = true;
            
            GetComponentInParent<BananasDryer>().takeInteractionUI.ShowUI();
        }

        public MeshRenderer BananaPeel => bananaPeel;
        public MeshRenderer Fabric => fabric;
    }
}
