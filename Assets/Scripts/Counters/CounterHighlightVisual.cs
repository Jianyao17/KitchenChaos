using System;
using UnityEngine;
using UnityEngine.Serialization;

public class CounterHighlightVisual : MonoBehaviour
{
    [SerializeField] BaseCounter kitchenCounter;
    [SerializeField] MeshRenderer[] listMeshRenderers;
    [SerializeField] Material highlightMaterial;

    private void Start()
    {
        kitchenCounter.OnShowHighlight += ShowHighlightMaterial;
        kitchenCounter.OnHideHighlight += HideHighlightMaterial;
    }

    private void OnDisable()
    {
        kitchenCounter.OnShowHighlight -= ShowHighlightMaterial;
        kitchenCounter.OnHideHighlight -= HideHighlightMaterial;
    }

    /* Function for displaying highlight material */
    // TODO: using different ways to change material/color
    private void ShowHighlightMaterial(object sender, EventArgs e)
    {
        foreach (var meshRenderer in listMeshRenderers) 
            meshRenderer.sharedMaterials = AddMaterials(meshRenderer, highlightMaterial);
    }

    private void HideHighlightMaterial(object sender, EventArgs e)
    {
        foreach (var meshRenderer in listMeshRenderers) 
            meshRenderer.sharedMaterials = RemoveMaterials(meshRenderer, highlightMaterial);
    }

    private Material[] AddMaterials(MeshRenderer renderer, Material mat)
    {
        Material[] sharedMat = renderer.sharedMaterials;
        Material[] newMaterial = new Material[sharedMat.Length + 1];
        
        sharedMat.CopyTo(newMaterial, 0);
        newMaterial[sharedMat.Length] = mat;
        return newMaterial;
    }

    private Material[] RemoveMaterials(MeshRenderer renderer, Material mat)
    {
        Material[] sharedMat = renderer.sharedMaterials;
        Material[] newMaterial = new Material[sharedMat.Length - 1];
        
        int matIndex = Array.IndexOf(sharedMat, mat);
        if (matIndex == -1) return sharedMat; // Material not found
        
        // Copy all material except target mat
        for (int i = 0, j = 0; i < sharedMat.Length; i++)
        {
            if (i == matIndex) continue; // Skip target mat
            newMaterial[j++] = sharedMat[i];
        }
        return newMaterial;
    }
}