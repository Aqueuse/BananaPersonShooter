using Data.Bananas;
using Gestion.Buildables.Plateforms;
using UnityEngine;

namespace Game {
    public class ActivatePlateform : MonoBehaviour {
        [SerializeField] private BananasDataScriptableObject bananasDataScriptableObject;
    
        private void Start() {
            GetComponent<Plateform>().ActivePlateform(bananasDataScriptableObject);
        }
    }
}