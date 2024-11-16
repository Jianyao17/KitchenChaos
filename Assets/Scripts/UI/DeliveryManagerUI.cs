using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private GameObject container;
    [SerializeField] private GameObject recipeTemplate;

    private void Awake()
    {
        recipeTemplate.SetActive(false);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSpawned += UpdateUIVisual;
        DeliveryManager.Instance.OnRecipeCompleted += UpdateUIVisual;
    }

    private void UpdateUIVisual(object sender, EventArgs eventArgs)
    {
        // Clean & Destroy icons game object except recipe template 
        foreach (Transform child in container.transform)
        {
            if (child == recipeTemplate.transform) continue;
            Destroy(child.gameObject);
        }
        
        // Spawn new list of recipe
        foreach (var recipeSO in DeliveryManager.Instance.ListDemandedRecipes)
        {
            var recipeObject = Instantiate(recipeTemplate, container.transform);
            recipeObject.name = $"Recipe {recipeSO.recipeName}";
            recipeObject.SetActive(true);
            recipeObject.GetComponent<RecipeTemplateUI>().SetRecipeUI(recipeSO);
        }
    }
}

