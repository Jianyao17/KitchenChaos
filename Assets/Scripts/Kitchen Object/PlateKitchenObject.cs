

using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    [SerializeField] private List<KitchenObjectSO> listValidIngredients;
    
    public event EventHandler<OnIngredientsAddedEventArgs> OnIngredientsAdded;
    public class OnIngredientsAddedEventArgs : EventArgs
    { public KitchenObjectSO KitchenObjectSO; }

    public List<KitchenObjectSO> ListAddedIngredients { get; } = new ();

    public bool TryAddIngredient(KitchenObjectSO kitchenObject)
    {
        // Add ingredient if ingredient valid and doesn't already exist in list kitchen object
        if (listValidIngredients.Contains(kitchenObject) && !ListAddedIngredients.Contains(kitchenObject))
        {
            ListAddedIngredients.Add(kitchenObject);
            OnIngredientsAdded?.Invoke(this, 
                new OnIngredientsAddedEventArgs { KitchenObjectSO = kitchenObject });
            return true;
        }
        return false;
    }
}