using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertToRegularMesh : MonoBehaviour
{
    [ContextMenu("Convert to regular mesh")]
    void Convert() 
    {
        SkinnedMeshRenderer SkinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        MeshRenderer MeshRenderer = gameObject.AddComponent<MeshRenderer>();
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

        meshFilter.sharedMesh = SkinnedMeshRenderer.sharedMesh;
        MeshRenderer.sharedMaterials = SkinnedMeshRenderer.sharedMaterials;

        DestroyImmediate(SkinnedMeshRenderer);
        DestroyImmediate(this);
    }
}
