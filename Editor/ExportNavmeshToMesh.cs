using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Formats.Fbx.Exporter;
using UnityEngine;
using UnityEngine.AI;

namespace Editor {
    public class ExportNavmeshToMesh : EditorWindow {
        private NavMeshTriangulation navMeshTriangulation;
        private static Mesh mesh;

        private static Material litMaterial;
        
        private void OnGUI() {
            if (GUILayout.Button("Save navmesh")) {
                navMeshTriangulation = NavMesh.CalculateTriangulation();

                mesh = new Mesh {
                    vertices = navMeshTriangulation.vertices,
                    triangles = navMeshTriangulation.indices
                };
                
                SaveMesh(mesh, mesh.name, false, true);
            }
        }
        
        private static void SaveMesh (Mesh newMesh, string name, bool makeNewInstance, bool optimizeMesh) {
            // save asset
            string assetPath = EditorUtility.SaveFilePanel("Save Separate Mesh Asset", "Assets/", name, "asset");
            if (string.IsNullOrEmpty(assetPath)) return;
        
            assetPath = FileUtil.GetProjectRelativePath(assetPath);

            Mesh meshToSave = (makeNewInstance) ? Instantiate(newMesh) : newMesh;
            if (optimizeMesh) MeshUtility.Optimize(meshToSave);
            
            AssetDatabase.CreateAsset(meshToSave, assetPath);
            AssetDatabase.SaveAssets();
            
            // create instance on the scene
            Mesh loadedMesh = (Mesh)AssetDatabase.LoadAssetAtPath(assetPath, typeof(Mesh));
            
            GameObject navmeshConversionGameObject = new GameObject("navmesh Mesh conversion");
            
            navmeshConversionGameObject.AddComponent<MeshFilter>();
            navmeshConversionGameObject.GetComponent<MeshFilter>().mesh = loadedMesh;

            litMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            navmeshConversionGameObject.AddComponent<MeshRenderer>();
            navmeshConversionGameObject.GetComponent<MeshRenderer>().material = litMaterial;
            
            // export to fbx instance
            string meshPath = EditorUtility.SaveFilePanel("Save Separate Mesh Asset", "Assets/", name, "fbx");
            if (string.IsNullOrEmpty(meshPath)) return;

            ExportBinaryFBX(meshPath, navmeshConversionGameObject);
            AssetDatabase.Refresh();
        }
        
        private static void ExportBinaryFBX (string filePath, UnityEngine.Object singleObject) {
            // Find relevant internal types in Unity.Formats.Fbx.Editor assembly
            Type[] types = AppDomain.CurrentDomain.GetAssemblies().First(x => x.FullName == "Unity.Formats.Fbx.Editor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null").GetTypes();
            Type optionsInterfaceType = types.First(x => x.Name == "IExportOptions");
            Type optionsType = types.First(x => x.Name == "ExportOptionsSettingsSerializeBase");
 
            // Instantiate a settings object instance
            MethodInfo optionsProperty = typeof(ModelExporter).GetProperty("DefaultOptions", BindingFlags.Static | BindingFlags.NonPublic).GetGetMethod(true);
            object optionsInstance = optionsProperty.Invoke(null, null);
 
            // Change the export setting from ASCII to binary
            FieldInfo exportFormatField = optionsType.GetField("exportFormat", BindingFlags.Instance | BindingFlags.NonPublic);
            exportFormatField.SetValue(optionsInstance, 1);
 
            // Invoke the ExportObject method with the settings param
            MethodInfo exportObjectMethod = typeof(ModelExporter).GetMethod("ExportObject", BindingFlags.Static | BindingFlags.NonPublic, Type.DefaultBinder, new Type[] { typeof(string), typeof(UnityEngine.Object), optionsInterfaceType }, null);
            exportObjectMethod.Invoke(null, new object[] { filePath, singleObject, optionsInstance });
        }
        
        [MenuItem("Tools/Export navmesh to mesh")]
        private static void OpenWindow() {
            GetWindow<ExportNavmeshToMesh>(false, "Export navmesh to mesh", true);
        }
    }
}
