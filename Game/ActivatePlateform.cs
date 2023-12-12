using Gestion.BuildablesBehaviours;
using ItemsProperties.Bananas;
using UnityEngine;

namespace Game {
    public class ActivatePlateform : MonoBehaviour {
        [SerializeField] private BananasPropertiesScriptableObject bananasPropertiesScriptableObject;
    
        private void Start() {
            GetComponent<PlateformBehaviour>().ActivePlateform(bananasPropertiesScriptableObject.bananaType);
        }
    }
}