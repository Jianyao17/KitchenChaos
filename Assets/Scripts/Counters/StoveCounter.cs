using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgressBar
{
    public enum FryingState
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }
    
    [SerializeField] private List<FryingRecipeSO> listOfFryingRecipes;

    public event EventHandler<ProgressEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    { public FryingState State; }
    
    private float _fryingTimer;
    private float _burningTimer;
    private FryingState _fryingState;
    private FryingRecipeSO _currentFryingRecipe;

    private void Start()
    {
        _fryingState = FryingState.Idle;
    }

    private void Update()
    {
        // Return if it not have kitchen object to cook
        if (!HasKitchenObject()) return;
        
        switch (_fryingState)
        {
            case FryingState.Idle:
                break;
            case FryingState.Frying:
                _fryingTimer += Time.deltaTime; // Increment frying timer
                OnProgressChanged?.Invoke(this, new ProgressEventArgs 
                    { ProgressNormalized = _fryingTimer / _currentFryingRecipe.fryingTimerMax });
                
                // If timer runs out, destroy current input kitchen object
                // then create new fried kitchen object & update state
                if (_fryingTimer >= _currentFryingRecipe.fryingTimerMax)
                {
                    GetAndClearKitchenObject().DestroySelf();
                    SpawnKitchenObject(_currentFryingRecipe.friedObject);
                        
                    _burningTimer = 0;
                    _fryingState = FryingState.Fried;
                    OnStateChanged?.Invoke(this, 
                        new OnStateChangedEventArgs { State = _fryingState });
                }
                break;
            case FryingState.Fried:
                _burningTimer += Time.deltaTime; // Increment burning timer
                OnProgressChanged?.Invoke(this, new ProgressEventArgs 
                    { ProgressNormalized = _burningTimer / _currentFryingRecipe.burningTimerMax });
                
                // If there is a burned kitchen type and timer runs out,
                // then destroy current input kitchen object,
                // create new burned kitchen object & update state
                if (_burningTimer >= _currentFryingRecipe.burningTimerMax
                    && _currentFryingRecipe.burnedObject is not null)
                {
                    GetAndClearKitchenObject().DestroySelf();
                    SpawnKitchenObject(_currentFryingRecipe.burnedObject);
                        
                    _fryingState = FryingState.Burned;
                    OnStateChanged?.Invoke(this, 
                        new OnStateChangedEventArgs { State = _fryingState });
                    OnProgressChanged?.Invoke(this, 
                        new ProgressEventArgs { ProgressNormalized = 0 });
                }
                break;
            case FryingState.Burned:
                break;
        }
    }

    public override void Interact(Player player)
    {
        // If there is no kitchen object here & player is carried kitchen object
        // & kitchen object has frying recipe, then pass that kitchen object here
        if (_KitchenObject is null && player.HasKitchenObject())
        {
            if (IsHasFryingRecipe(player.GetKitchenObject().GetKitchenObjectSO()))
            {
                var kitchenObject = player.GetAndClearKitchenObject();
                SetKitchenObject(kitchenObject);
                
                // Start cooking kitchen object, update state
                _currentFryingRecipe = GetFryingRecipe(kitchenObject.GetKitchenObjectSO());
                
                _fryingTimer = 0;
                _fryingState = FryingState.Frying;
                
                OnProgressChanged?.Invoke(this, 
                    new ProgressEventArgs { ProgressNormalized = _fryingTimer / _currentFryingRecipe.fryingTimerMax });
                OnStateChanged?.Invoke(this, 
                    new OnStateChangedEventArgs { State = _fryingState });
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
                {
                    GetAndClearKitchenObject().DestroySelf();
                    
                    _fryingState = FryingState.Idle;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { State = _fryingState });
                    OnProgressChanged?.Invoke(this, new ProgressEventArgs { ProgressNormalized = 0 });
                }
            }
            // And player is not carried anything
            else if (!player.HasKitchenObject())
            {
                // then pass this kitchen object to player
                player.SetKitchenObject(GetAndClearKitchenObject());
                
                _fryingState = FryingState.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { State = _fryingState });
                OnProgressChanged?.Invoke(this, new ProgressEventArgs { ProgressNormalized = 0 });
            }
        }
    }
    
    private bool IsHasFryingRecipe(KitchenObjectSO input)
    {
        // Return true if it has recipe
        return listOfFryingRecipes
            .Any(recipes => recipes.inputObject == input);
    }

    private FryingRecipeSO GetFryingRecipe(KitchenObjectSO input)
    {
        // Get cutting recipes
        return listOfFryingRecipes
            .FirstOrDefault(recipes => recipes.inputObject == input);
    }

}
