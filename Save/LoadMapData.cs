using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Save {
    public class LoadMapData : MonoSingleton<LoadMapData> {
        private readonly NumberFormatInfo americaNumberFormatInfo = new CultureInfo("en-US").NumberFormat;
        private string sceneName;

        public bool HasData() {
            var appPath = Path.GetDirectoryName(Application.persistentDataPath);
            if (appPath != null) {
                var mapDataSavesPath = Path.Combine(appPath, "MAPDATA");

                sceneName = SceneManager.GetActiveScene().name;

                string savefilePath = Path.Combine(mapDataSavesPath, sceneName.ToUpper() + "_debris.data");

                return File.Exists(savefilePath);
            }

            return false;
        }

        public void Load() {
            var appPath = Path.GetDirectoryName(Application.persistentDataPath);
            if (appPath != null) {
                var mapDataSavesPath = Path.Combine(appPath, "MAPDATA");
                
                foreach (var mapData in GameSave.Instance.mapDatasBySceneNames) {
                    if (mapData.Value.hasDebris) {
                        string savefilePath = Path.Combine(mapDataSavesPath, mapData.Key.ToUpper()+"_debris.data");

                        if (File.Exists(savefilePath)) {
                            using StreamReader streamReader = new StreamReader(savefilePath);

                            List<string> debrisData = new List<string>();

                            while (!streamReader.EndOfStream) {
                                debrisData.Add(streamReader.ReadLine());
                            }

                            GameSave.Instance.mapDatasBySceneNames[mapData.Key.ToUpper()].debrisPosition = new Vector3[debrisData.Count];
                            GameSave.Instance.mapDatasBySceneNames[mapData.Key.ToUpper()].debrisRotation = new Quaternion[debrisData.Count];
                            GameSave.Instance.mapDatasBySceneNames[mapData.Key.ToUpper()].debrisIndex = new int[debrisData.Count];

                            for (var i=0; i<debrisData.Count; i++) {
                                var dataSplit = debrisData[i].Split("/");
                    
                                GameSave.Instance.mapDatasBySceneNames[mapData.Key.ToUpper()].debrisPosition[i] = Vector3FromString(dataSplit[0]);
                                GameSave.Instance.mapDatasBySceneNames[mapData.Key.ToUpper()].debrisRotation[i] = QuaternionFromString(dataSplit[1]);
                                GameSave.Instance.mapDatasBySceneNames[mapData.Key.ToUpper()].debrisIndex[i] = int.Parse(dataSplit[2]);
                            }
                        }
                    }
                }
            }
        }
        
        private Vector3 Vector3FromString(string vector3String){
            string[] temp = vector3String.Substring(1,vector3String.Length-2).Split(',');
            
            return new Vector3(
                float.Parse(temp[0], americaNumberFormatInfo), 
                float.Parse(temp[1], americaNumberFormatInfo),
                float.Parse(temp[2], americaNumberFormatInfo)
            );
        }

        private Quaternion QuaternionFromString(string quaternionString){
            string[] temp = quaternionString.Substring(1,quaternionString.Length-2).Split(',');
            return new Quaternion(
                float.Parse(temp[0], americaNumberFormatInfo), 
                float.Parse(temp[1], americaNumberFormatInfo),
                float.Parse(temp[2], americaNumberFormatInfo),
                float.Parse(temp[3], americaNumberFormatInfo)
            );
        }
    }
}
