using Enums;
using UnityEngine;

[CreateAssetMenu (fileName = "Data", menuName = "ScriptableObjects/DebrisDataScriptableObject", order = 2)]
public class DebrisDataScriptableObject : ScriptableObject {
    public Sprite sprite;
    [Multiline] public string[] description;

    // Game
    public ItemThrowableType itemThrowableType;
}