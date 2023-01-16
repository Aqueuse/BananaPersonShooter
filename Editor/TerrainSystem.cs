//JeCodeLeSoir

using System;
using UnityEngine;

public class TerrainSystem : MonoBehaviour
{

    [SerializeField]
    DataMapMat dataMapMat;
    
    void Update()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit raycastHit))
        {
            Debug.Log(GetSurfaceTypeFromMaterial(raycastHit.transform.gameObject, raycastHit.triangleIndex));
        }
        else
        {
            Debug.Log("null");
        }
    }

    private Material GetSurfaceTypeFromMaterial(GameObject obj, int triangleIndex)
    {

        if (obj.TryGetComponent(out GameObjectData gameObjectData))
        {
            return gameObjectData.dataMapMat.GetMaterial(triangleIndex);
        }
        else 
        { 
            
            Renderer renderer = obj.GetComponent<Renderer>();

            if (renderer.materials.Length > 2)
            {
                Debug.Log("renderer.materials.Length > 2");
                return null;
            }
            
            return renderer.material;
        }
    }
}