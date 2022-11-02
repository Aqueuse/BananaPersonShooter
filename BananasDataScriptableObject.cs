using UnityEngine;

[CreateAssetMenu (fileName = "Data", menuName = "ScriptableObjects/bananasDataScriptableObject", order = 1)]
public class BananasDataScriptableObject : ScriptableObject {
    public string bananaName;
    [Multiline] public string bananaDescription;

    // Game
    public int regimeQuantity;

    public float damage;
    public float damageTime;

    public float healthBonus;
    public float resistanceBonus;

    public BananaType bananaType;
    public BananaEffect bananaEffect;
}