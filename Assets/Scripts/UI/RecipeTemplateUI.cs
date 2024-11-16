using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeTemplateUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeNameText;
    [SerializeField] private GameObject iconContainer;
    [SerializeField] private GameObject iconTemplate;

    private void Awake()
    {
        iconTemplate.SetActive(false);
    }

    public void SetRecipeUI(RecipeSO recipeSO)
    {
        recipeNameText.text = recipeSO.recipeName;
        
        // Clean & Destroy icons game object except icon template 
        foreach (Transform child in iconContainer.transform)
        {
            if (child == iconTemplate.transform) continue;
            Destroy(child.gameObject);
        }
        
        // Spawn new list of kitchen object icon
        foreach (var kitchenObject in recipeSO.KitchenObjects)
        {
            var iconObject = Instantiate(iconTemplate, iconContainer.transform);
            iconObject.name = $"Ingredient {kitchenObject.objectName}";
            iconObject.SetActive(true);
            iconObject.GetComponent<Image>().sprite = kitchenObject.sprite;
        }
    }
}
