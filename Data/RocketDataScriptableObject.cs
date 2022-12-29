using Enums;
using UnityEngine;

[CreateAssetMenu (fileName = "Data", menuName = "ScriptableObjects/rocketDataScriptableObject", order = 2)]
public class RocketDataScriptableObject : ScriptableObject {
    public Sprite sprite;
    [Multiline] public string[] description;

    // Game
    public ItemThrowableType itemThrowableType;
}