using Enums;
using UnityEngine;

[CreateAssetMenu (fileName = "Data", menuName = "ScriptableObjects/plateformDataScriptableObject", order = 2)]
public class PlateformDataScriptableObject : ScriptableObject {
    public Sprite sprite;
    [Multiline] public string[] description;

    // Game
    public ItemThrowableType itemThrowableType;
    public GenericDictionary<ItemThrowableType, int> Cost;
}