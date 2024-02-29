using UnityEngine;

namespace InGame.Items.ItemsBehaviours.BuildablesBehaviours {
    public class BananaDryerSlot : MonoBehaviour {
        [SerializeField] private MeshRenderer leftBananaPeel;
        [SerializeField] private MeshRenderer leftFabric;
        
        [SerializeField] private MeshRenderer rightBananaPeel;
        [SerializeField] private MeshRenderer rightFabric;

        public BoxCollider addBananaPeelBoxCollider;
        
        public BananasDryerBehaviour bananasDryerBehaviour;
        
        public void Init() {
            if (bananasDryerBehaviour.bananaPeelsQuantity > 0) {
                AddBananaPeel();
                bananasDryerBehaviour.bananaPeelsQuantity -= 1;
            }
        }

        public void AddBananaPeel() {
            if (bananasDryerBehaviour.bananaPeelsQuantity < 20) {
                if (leftBananaPeel.enabled == false) {
                    leftBananaPeel.enabled = true;
                    
                    bananasDryerBehaviour.bananaPeelsQuantity += 1;

                    Invoke(nameof(ProduceFabric), 3);
                }

                else {
                    if (rightBananaPeel.enabled == false) {
                        rightBananaPeel.enabled = true;
                        
                        bananasDryerBehaviour.bananaPeelsQuantity += 1;

                        addBananaPeelBoxCollider.enabled = false;
                        
                        Invoke(nameof(ProduceFabric), 3);
                    }
                }
            }
        }
    
        public void ProduceFabric() {
            if (leftFabric.enabled == false) {
                leftBananaPeel.enabled = false;
                leftFabric.enabled = true;
                leftFabric.GetComponentInChildren<BoxCollider>().enabled = true;
                
                bananasDryerBehaviour.bananaPeelsQuantity -= 1;
                bananasDryerBehaviour.fabricQuantity += 1;
            }

            else {
                if (rightFabric.enabled == false) {
                    rightBananaPeel.enabled = false;
                    rightFabric.enabled = true;
                    rightFabric.GetComponentInChildren<BoxCollider>().enabled = true;
                    
                    bananasDryerBehaviour.bananaPeelsQuantity -= 1;
                    bananasDryerBehaviour.fabricQuantity += 1;
                }
            }
        }
    }
}
