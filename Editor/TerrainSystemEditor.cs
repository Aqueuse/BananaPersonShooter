//JeCodeLeSoir
#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class TerrainSystemEditor : EditorWindow
{
    [MenuItem("Terrain/TerrainDataMap")]
    static void Init()
    {
        TerrainSystemEditor window = (TerrainSystemEditor)EditorWindow.GetWindow(typeof(TerrainSystemEditor));
        window.Show();
    }

    GameObject terrain = null;
   
    
    void OnGUI()
    {
        GUILayout.Label("Terrain Data Map Material", EditorStyles.boldLabel);

        terrain = (GameObject)EditorGUILayout.ObjectField("Terrain", terrain, typeof(GameObject), true);

        /* Select path to save the DataMapMat ScriptableObject */
 
        if (GUILayout.Button("Generate Map"))
        {
            string path = EditorUtility.SaveFilePanelInProject("Save DataMapMat", "DataMapMat", "asset", "Save DataMapMat", "Assets");
            if (!File.Exists(path)) {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            this.GenerateMap(terrain, path);
        }
    }

    public void GenerateMap(GameObject gameObjectTerrain, string path)
    {
        
        DataMapMat dataMapMat = ScriptableObject.CreateInstance <DataMapMat>();
        
        
        Mesh mesh = null;
        Renderer renderer;

        if (gameObjectTerrain.TryGetComponent(out MeshFilter meshFilter))
            mesh = meshFilter.sharedMesh;

        renderer = gameObjectTerrain.GetComponent<Renderer>();

        dataMapMat.materials = renderer.sharedMaterials;

        if (mesh == null)
            return;

        dataMapMat.TrianglesByMatIndex = new int[mesh.triangles.Length];
        Array.Clear(dataMapMat.TrianglesByMatIndex, 0, mesh.triangles.Length);

        int[] triangles = mesh.triangles;
        for (var x = 0; x < triangles.Length; x += 3)
        {
            for (var material = 0; material < dataMapMat.materials.Length; material++)
            {
                var material_vertices = mesh.GetTriangles(material);
                for (var i = 0; i < material_vertices.Length; i += 3)
                {
                    if (
                            triangles[x + 0] == material_vertices[i + 0]
                         && triangles[x + 1] == material_vertices[i + 1]
                         && triangles[x + 2] == material_vertices[i + 2]
                    )
                    {
                        dataMapMat.TrianglesByMatIndex[x + 0] = material;
                        dataMapMat.TrianglesByMatIndex[x + 1] = material;
                        dataMapMat.TrianglesByMatIndex[x + 2] = material;
                    }
                }
            }
        }

        /* Save Asset plz */

        AssetDatabase.CreateAsset(dataMapMat, path);

        GameObjectData gameObjectData = gameObjectTerrain.AddComponent<GameObjectData>();
        gameObjectData.dataMapMat = dataMapMat;
    }
}
#endif