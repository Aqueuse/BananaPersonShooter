//JeCodeLeSoir
using UnityEngine;

public class DataMapMat : ScriptableObject
{
    public int[] TrianglesByMatIndex;
    public Material[] materials;
    
    public Material GetMaterial(int triangleIndex)
    {
        
        Material a = this.materials[this.TrianglesByMatIndex[triangleIndex * 3]];
        Material b = this.materials[this.TrianglesByMatIndex[triangleIndex * 3 + 1]];
        Material c = this.materials[this.TrianglesByMatIndex[triangleIndex * 3 + 2]];

        if (a == b && b == c) return a;

        return null;
    }
}
