using System;
using System.Collections.Generic;
using System.Linq;
using ImprovedTimers;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliveryManager : MonoBehaviour
{
    [SerializeField] private ListRecipeSO listAvailableRecipes;
    [SerializeField] private float spawnTimerMax = 5;
    [SerializeField] private int maxRecipesQueue = 4;
    
    // Singleton
    public static DeliveryManager Instance { get; private set; }
    
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    
    public event EventHandler<AudioPosEventArgs> OnRecipeSuccess;
    public event EventHandler<AudioPosEventArgs> OnRecipeFailed;
    public class AudioPosEventArgs : EventArgs
    { public Vector3 Position; }

    
    public List<RecipeSO> ListDemandedRecipes { get; } = new();
    public int SuccessfulDeliveryCount => _successfulDeliveryCount;
    
    private int _successfulDeliveryCount = 0;
    private CountdownTimer _spawnTimer;

    private void Awake()
    {
        Instance = this;
        _spawnTimer = new CountdownTimer(spawnTimerMax);
    }

    private void Update()
    {
        // Spawn new demanded recipe and add to list when timer is finished
        // & list recipes less than maxRecipesQueue
        if (_spawnTimer.IsFinished)
        {
            _spawnTimer.Reset();
            _spawnTimer.Start();
            if (ListDemandedRecipes.Count < maxRecipesQueue)
            {
                var demandedRecipe = listAvailableRecipes.Recipes[Random.Range(0, listAvailableRecipes.Recipes.Count)];
                ListDemandedRecipes.Add(demandedRecipe);
            
                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    // Find and check if food delivered match in the demanded recipes
    public void DeliverRecipe(PlateKitchenObject plate)
    {
        var matchedRecipe = ListDemandedRecipes.FirstOrDefault(recipe => 
            recipe.KitchenObjects.Count == plate.ListAddedIngredients.Count &&
            recipe.KitchenObjects.All(obj => plate.ListAddedIngredients.Contains(obj)));
        
        // Remove recipe from list if recipe correct
        if (matchedRecipe is not null)
        {
            _successfulDeliveryCount++;
            ListDemandedRecipes.Remove(matchedRecipe);
            OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
            OnRecipeSuccess?.Invoke(this, new AudioPosEventArgs 
                { Position = DeliveryCounter.Instance.transform.position });
        }
        else
        { 
            OnRecipeFailed?.Invoke(this, new AudioPosEventArgs 
            { Position = DeliveryCounter.Instance.transform.position }); 
        }
    }

    private void OnDestroy()
    {
        _spawnTimer.Stop();
        _spawnTimer.Dispose();
    }
}
