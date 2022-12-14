using Enums;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu (fileName = "Data", menuName = "ScriptableObjects/bananasDataScriptableObject", order = 1)]
public class BananasDataScriptableObject : ScriptableObject {
    public string bananaName;
    public Sprite sprite;
    [Multiline] public string[] description;
    [Multiline] public string[] effects;

    // Game
    public int regimeQuantity;

    public float damage;
    public float damageTime;

    public float healthBonus;
    public float resistanceBonus;

    public ItemThrowableType itemThrowableType;
    public BananaEffect bananaEffect;
}