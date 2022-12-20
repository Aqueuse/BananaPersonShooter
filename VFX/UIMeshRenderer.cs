//Custom UI Meshes script by @MinionsArt
using UnityEngine;
using UnityEngine.UI;
 
[RequireComponent(typeof(CanvasRenderer))]
[ExecuteAlways]
public class UIMeshRenderer : MonoBehaviour {
    [SerializeField]
    Material Material;
    [SerializeField]
    private Mesh mesh;
    [SerializeField]
    bool mask;
 
    [SerializeField]
    bool showMaskGraphic;
 
    [SerializeField]
    bool maskable;
 
    [SerializeField]
    bool preserveAspect;
 
    CanvasRenderer canvasRenderer;
 
    Image[] childImage;
 
    Vector3[] baseVertices;
 
    RectTransform rect;
    float cachedHeight, cachedWidth;
    void Start()
    {
        SetupMesh();
    }
 
    void SetupMesh()
    {
        // grab the canvasrenderer
        if (canvasRenderer == null)
        {
            canvasRenderer = GetComponent<CanvasRenderer>();
        }
        if (rect == null)
        {
            rect = GetComponent<RectTransform>();
        }
        // set the mesh and material
        canvasRenderer.SetMaterial(Material, null);
        // create new mesh with scale
        canvasRenderer.SetMesh(CreateNewMesh());
        if (mask)
        {
            // set the stencil buffer of this mesh to be a mask
            SetStencilSelf();
            // check if there are images to mask
            childImage = GetComponentsInChildren<Image>();
            if (childImage.Length > 0)
            {
                SetStencilChildren(childImage);
            }
        }
        else if (maskable)
        {
            // set stencil buff of this mesh to be maskable
            SetMaskableSelf();
        }
    }
 
    void Update()
    {
        // if rect changed, update
        if (cachedWidth != rect.rect.width || cachedHeight != rect.rect.height)
        {
            canvasRenderer.SetMesh(CreateNewMesh());
            cachedWidth = rect.rect.width;
            cachedHeight = rect.rect.height;
        }
    }
 
    void OnEnable()
    {
        SetupMesh();
        canvasRenderer.cull = false;
    }
 
    void OnDisable()
    {
        canvasRenderer.Clear();
        canvasRenderer.cull = true;
    }
 
    private Mesh CreateNewMesh()
    {
        // create copy of the mesh
        Mesh newMesh = Instantiate(mesh);
        baseVertices = newMesh.vertices;
        var vertices = new Vector3[baseVertices.Length];
        // base size on mesh bounds
        Vector2 size = new Vector2(newMesh.bounds.extents.x, newMesh.bounds.extents.y);
        Rect r = rect.rect;
        // handle preserving aspect
        if (preserveAspect && size.sqrMagnitude > 0.0f)
        {
            var meshRatio = size.x / size.y;
            var rectRatio = r.width / r.height;
 
            if (meshRatio > rectRatio)
            {
                var oldHeight = r.height;
                r.height = r.width * (1.0f / meshRatio);
                r.y += (oldHeight - r.height) * rect.pivot.y;
            }
            else
            {
                var oldWidth = r.width;
                r.width = r.height * meshRatio;
                r.x += (oldWidth - r.width) * rect.pivot.x;
            }
        }
 
        // scale to rect transform size
        float scaleY = (r.height / newMesh.bounds.max.y) * 0.5f;
        float scaleX = (r.width / newMesh.bounds.max.x) * 0.5f;
 
        // use scale on all vertices
        for (var i = 0; i < vertices.Length; i++)
        {
            var vertex = baseVertices[i];
            vertex.x = vertex.x * scaleX;
 
            vertex.y = vertex.y * scaleY;
 
            vertex.z = vertex.z * scaleX;
 
            vertices[i] = vertex;
        }
        // set new vertices
        newMesh.vertices = vertices;
 
        newMesh.RecalculateNormals();
        newMesh.RecalculateBounds();
 
        return newMesh;
    }
 
    void SetStencilSelf()
    {
        Material instancedMat = Instantiate(Material);
        canvasRenderer.SetMaterial(instancedMat, null);
 
        // stencil buffer for mask
        canvasRenderer.GetMaterial().SetInt("_Stencil", 1);
        canvasRenderer.GetMaterial().SetInt("_StencilComp", 8);
        canvasRenderer.GetMaterial().SetInt("_StencilOp", 2);
        if (showMaskGraphic)
        {
            // set colormask all
            canvasRenderer.GetMaterial().SetInt("_ColorMask", 15);
        }
        else
        {
            // no colormask
            canvasRenderer.GetMaterial().SetInt("_ColorMask", 0);
        }
    }
 
    void SetMaskableSelf()
    {
        // instantiate and adjust the material for this mesh
        Material instancedMat = Instantiate(Material);
        canvasRenderer.SetMaterial(instancedMat, null);
        // stencil buffer for maskable
        canvasRenderer.GetMaterial().SetInt("_Stencil", 1);
        canvasRenderer.GetMaterial().SetInt("_StencilComp", 3);
        canvasRenderer.GetMaterial().SetInt("_StencilOp", 0);
        canvasRenderer.GetMaterial().SetInt("_StencilReadMask", 1);
        canvasRenderer.GetMaterial().SetInt("_StencilWriteMask", 0);
    }
 
    void SetStencilChildren(Image[] images)
    {
        for (int i = 0; i < images.Length; i++)
        {
            // if child has maskable enabled
            if (images[i].maskable)
            {
                // instantiate the standard UI material (If you have custom image shaders, you will want to grab them seperately and cache them)
                Material instancedMat = new Material(Shader.Find("UI/Default"));
                images[i].material = instancedMat;
                // stencil buffer for maskable
                images[i].material.SetInt("_Stencil", 1);
                images[i].material.SetInt("_StencilComp", 3);
                images[i].material.SetInt("_StencilOp", 0);
                images[i].material.SetInt("_StencilReadMask", 1);
                images[i].material.SetInt("_StencilWriteMask", 0);
            }
 
        }
    }
    void OnValidate()
    {
        if (!Application.isPlaying)
        {
            SetupMesh();
        }
 
    }
 
}
