using System;
using Unity.GraphToolkit.Editor;
using UnityEditor;

namespace Editor {
    [Graph(AssetExtension)]
    [Serializable]
    public class AiBehaviourGraph : Graph {
        public const string AssetExtension = "AiBehaviour";

        [MenuItem("Assets/Create/IA/Ai Behaviour Graph", false)]
        private static void CreateAssetFile() {
            GraphDatabase.PromptInProjectBrowserToCreateNewAsset<AiBehaviourGraph>();
        }
    }
}