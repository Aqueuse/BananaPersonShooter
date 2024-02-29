using System;
using System.Collections.Generic;
using InGame.Items.ItemsData;
using InGame.SpaceTrafficControl;
using Newtonsoft.Json;
using Save.Templates;

namespace InGame.Items.ItemsBehaviours.SpaceshipsBehaviours {
    public class VisitorSpaceshipBehaviour : SpaceshipBehaviour {
        public List<VisitorData> visitorDatas;
        
        public override void Init() {
            // if scene is corolle
            // else
        }
    
        public override void GenerateSaveData() {
            if(string.IsNullOrEmpty(spaceshipGuid)) {
                spaceshipGuid = Guid.NewGuid().ToString();
            }

            SpaceshipSavedData visitorSpaceshipData = new SpaceshipSavedData {
                spaceshipGuid = spaceshipGuid,
                visitorDatas = visitorDatas
            };

            savedData = JsonConvert.SerializeObject(visitorSpaceshipData);
        }
        
        public override void Spawn3DSpacehip() {
            Hangars.Instance.Spawn3DSpaceshipInHangar(spaceshipSavedData.hangarNumber, ObjectsReference.Instance.meshReferenceScriptableObject.visitorSpaceshipPrefab);
        }
    }
}
