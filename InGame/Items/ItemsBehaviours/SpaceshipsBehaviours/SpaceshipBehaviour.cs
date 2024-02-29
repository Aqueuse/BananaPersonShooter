using System;
using Newtonsoft.Json;
using Save.Templates;
using UnityEngine;

namespace InGame.Items.ItemsBehaviours.SpaceshipsBehaviours {
    public class SpaceshipBehaviour : MonoBehaviour {
        public string spaceshipGuid;

        public string savedData;
        public SpaceshipSavedData spaceshipSavedData;

        public void GenerateGuid() {
            if(string.IsNullOrEmpty(spaceshipGuid)) {
                spaceshipGuid = Guid.NewGuid().ToString();
            }
        }

        public void GenerateName() {
            spaceshipSavedData.spaceshipName = "nxh-" + spaceshipSavedData.characterType;
        }
        
        public virtual void Init() {}
        
        public virtual void Spawn3DSpacehip() { }
        
        public virtual void GenerateSaveData() { }
        
        public void LoadSavedData(string stringifiedJson) {
            spaceshipSavedData = JsonConvert.DeserializeObject<SpaceshipSavedData>(stringifiedJson); 

            spaceshipGuid = spaceshipSavedData.spaceshipGuid;
        
            ObjectsReference.Instance.spaceTrafficControlManager.LoadSpaceshipBehaviour(spaceshipSavedData.characterType, spaceshipSavedData);
        }
    }
}
