using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Save {
    public class SaveMapData : MonoSingleton<SaveMapData> {
        private string[] debrisDatas;

        public void DeleteSaves() {
            var appPath = Path.GetDirectoryName(Application.persistentDataPath);
            if (appPath != null) {
                var mapDataSavesPath = Path.Combine(appPath, "MAPDATA");

                foreach (var mapData in GameSave.Instance.mapDatasBySceneNames) {
                    if (mapData.Value.hasDebris) {
                        string savefilePath = Path.Combine(mapDataSavesPath, mapData.Key.ToUpper() + "_debris.data");

                        File.Delete(savefilePath);
                    }
                }
            }
        }

        public void Save(Vector3[] debrisPosition, Quaternion[] debrisRotation, int[] debrisIndex) {
            debrisDatas = new string[debrisPosition.Length];
        
            for (var i = 0; i < debrisPosition.Length; i++) {
                debrisDatas[i] = debrisPosition[i] + "/" + debrisRotation[i] + "/" + debrisIndex[i];
            }
        
            var appPath = Path.GetDirectoryName(Application.persistentDataPath);
            if (appPath != null) {
                var mapDataSavesPath = Path.Combine(appPath, "MAPDATA");
        
                if(!File.Exists(mapDataSavesPath))
                    Directory.CreateDirectory(mapDataSavesPath);

                string savefilePath = Path.Combine(mapDataSavesPath, SceneManager.GetActiveScene().name.ToUpper()+"_debris.data");

                using StreamWriter streamWriter = new StreamWriter(savefilePath, append:false);

                foreach (var debrisData in debrisDatas) {
                    streamWriter.WriteLine(debrisData);
                }
                
                streamWriter.Flush();
            }
        }
    }
}
