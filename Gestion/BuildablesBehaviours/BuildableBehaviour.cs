using UnityEngine;

namespace Gestion.BuildablesBehaviours {
    public class BuildableBehaviour : MonoBehaviour {
        public string buildableGuid;
        public BuildableType buildableType;
    
        public virtual void GenerateSaveData() { }
        
        public virtual void LoadSavedData(string stringifiedJson) { }
    }
}
