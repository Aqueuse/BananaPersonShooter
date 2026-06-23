using Editor.FlowNoddles;
using Unity.GraphToolkit.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor {
    public static class AiGraphConverter {
        private static IPort outputPort;
        private static IPort input1Port;
        
        private static BaseNode connectedNode;
        
        [MenuItem("Assets/Convert To Runtime Graph")]
        private static void Convert() {
            var sourceGraph = Selection.activeObject;

            string sourcePath = AssetDatabase.GetAssetPath(sourceGraph);

            var graph = GraphDatabase.LoadGraph<AiBehaviourGraph>(sourcePath);

            if (sourceGraph == null || graph == null) {
                Debug.LogError("Not an AI Behaviour graph");
                return;
            }

            var runtimeGraph = ScriptableObject.CreateInstance<AiRuntimeGraph>();

            // 3. Create a valid path to save the asset
            var directory = System.IO.Path.GetDirectoryName(sourcePath);
            System.Diagnostics.Debug.Assert(directory != null, nameof(directory) + " != null");
            
            var runtimePath = System.IO.Path.Combine(directory, sourceGraph.name + "_Runtime.asset");

            AssetDatabase.CreateAsset(runtimeGraph, runtimePath);

            // 1. Copier les nodes
            foreach (var node in graph.GetNodes()) {
                if (node.GetType().BaseType == typeof(BaseNode)) {
                    var executor = ((BaseNode)node).ConvertToExecutor(runtimeGraph);

                    runtimeGraph.executors.Add(executor.executorUuid, executor);
                    AssetDatabase.AddObjectToAsset(executor, runtimeGraph);
                }
            }
            
            // 4. Sauvegarder l’asset runtime
            AssetDatabase.SaveAssets();

            Debug.Log($"Converted {sourceGraph.name} → {runtimePath}");
        }
    }
}
