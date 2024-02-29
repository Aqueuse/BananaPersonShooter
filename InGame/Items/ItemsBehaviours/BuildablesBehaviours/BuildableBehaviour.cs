using System;
using System.Collections.Generic;
using InGame.Items.ItemsProperties.Buildables.VisitorsBuildable;
using UnityEngine;

namespace InGame.Items.ItemsBehaviours.BuildablesBehaviours {
    public class BuildableBehaviour : MonoBehaviour {
        public string buildableGuid;
        public BuildableType buildableType;
        public bool isBreaked;
        
        public Transform ChimpTargetTransform;
        public Transform ChimpTargetLookAtTransform;

        public bool isVisitorTargetable;
        public VisitorsBuildablePropertiesScriptableObject visitorsBuildablePropertiesScriptableObject;
        
        public bool isPirateTargeted;
        public bool isVisitorTargeted;

        [SerializeField] private List<MeshRenderer> meshRenderersToBreak;
        private static readonly int crackOpacity = Shader.PropertyToID("_crack_opacity");

        private void Start() {
            if(string.IsNullOrEmpty(buildableGuid)) {
                buildableGuid = Guid.NewGuid().ToString();
            }
            
            if (isBreaked) BreakBuildable();
        }

        public void BreakBuildable() {
            isBreaked = true;
            isPirateTargeted = false;

            foreach (var meshRenderer in meshRenderersToBreak) {
                meshRenderer.material.SetFloat(crackOpacity, 1);
            }
        }

        public void RepairBuildable() {
            isBreaked = false;
            isPirateTargeted = false;

            foreach (var meshRenderer in meshRenderersToBreak) {
                meshRenderer.material.SetFloat(crackOpacity, 0);
            }
        }
        
        public virtual void GenerateSaveData() { }
        
        public virtual void LoadSavedData(string stringifiedJson) { }
    }
}
