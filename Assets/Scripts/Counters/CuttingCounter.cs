using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgressBar
{
    [SerializeField] private List<CuttingRecipeSO> listOfCuttingRecipes;

    public static event EventHandler OnAnyCutting;
    
    public event EventHandler OnCutting;
    public event EventHandler<ProgressEventArgs> OnProgressChanged;
    
    private int _cuttingProgress;

    public override void Interact(Player player)
    {
        // If there is no kitchen object here & player is carried kitchen object
        // & that kitchen object has cutting recipe, then pass that kitchen object here
        if (_KitchenObject is null && player.HasKitchenObject())
        {
            if (IsHasCuttingRecipe(player.GetKitchenObject().GetKitchenObjectSO()))
            {
                var kitchenObject = player.GetAndClearKitchenObject();
                SetKitchenObject(kitchenObject);

                _cuttingProgress = 0;
                int cuttingProgressMax = GetCuttingRecipe(kitchenObject.GetKitchenObjectSO()).cuttingProgressMax;
                OnProgressChanged?.Invoke(this, new ProgressEventArgs
                    { ProgressNormalized = (float) _cuttingProgress / cuttingProgressMax });
            }
        }
        // Else If there is kitchen object here
        else if (_KitchenObject is not null)
        {   
            // And player is carried plate kitchen object
            if (player.HasKitchenObject() &&
                player.GetKitchenObject().TryGetPlateObject(out var plateObject))
            {
                // Try to add this kitchen object to player's plate object 
                if (plateObject?.TryAddIngredient(_KitchenObject.GetKitchenObjectSO()) == true)
                    GetAndClearKitchenObject().DestroySelf();
            }
            // And player is not carried anything
            else if (!player.HasKitchenObject())
            {
                // then pass this kitchen object to player
                player.SetKitchenObject(GetAndClearKitchenObject());
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        // If there is kitchen object here and has cutting recipe,
        // Increment cuttingProgress until cuttingProgressMax
        // then create new kitchen object slices based on recipe 
        if (HasKitchenObject() && IsHasCuttingRecipe(_KitchenObject.GetKitchenObjectSO()))
        {
            _cuttingProgress++;
            OnCutting?.Invoke(this, EventArgs.Empty);
            OnAnyCutting?.Invoke(this, EventArgs.Empty);
            
            int cuttingProgressMax = GetCuttingRecipe(_KitchenObject.GetKitchenObjectSO()).cuttingProgressMax;
            OnProgressChanged?.Invoke(this, new ProgressEventArgs()
                { ProgressNormalized = (float) _cuttingProgress / cuttingProgressMax });

            if (_cuttingProgress >= cuttingProgressMax)
            {
                KitchenObjectSO productObject = GetKitchenObjectOutput(_KitchenObject.GetKitchenObjectSO());
                GetAndClearKitchenObject().DestroySelf(); // Clear reference & destroy object

                // Spawn slices kitchen object
                SpawnKitchenObject(productObject);
            }
        }
    }

    private bool IsHasCuttingRecipe(KitchenObjectSO input)
    {
        // Return true if it has recipe
        return listOfCuttingRecipes
            .Any(recipes => recipes.inputObject == input);
    }

    private KitchenObjectSO GetKitchenObjectOutput(KitchenObjectSO input)
    {
        // Get product kitchen object from cutting recipes
         return listOfCuttingRecipes
             .FirstOrDefault(recipes => recipes.inputObject == input)
             ?.outputObject;
    }

    private CuttingRecipeSO GetCuttingRecipe(KitchenObjectSO input)
    {
        // Get cutting recipes
        return listOfCuttingRecipes
            .FirstOrDefault(recipes => recipes.inputObject == input);
    }
}
