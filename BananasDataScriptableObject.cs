using UnityEngine;

[CreateAssetMenu (fileName = "Data", menuName = "ScriptableObjects/bananasDataScriptableObject", order = 1)]
public class BananasDataScriptableObject : ScriptableObject {
    public string banana_name;
    [Multiline] public string bananaDescription;

    // Game
    public int regimeQuantity;

    public float damage;
    public float damage_time;

    public float healthBonus;
    public float resistanceBonus;

    public BananaType bananaType;
    public BananaEffect bananaEffect;
}