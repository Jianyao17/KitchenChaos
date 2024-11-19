using System;
using System.Collections;
using System.Collections.Generic;
using ImprovedTimers;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO plateKitchenObject;
    [SerializeField] private float spawnTimerMax = 5f;
    [SerializeField] private int maxPlatesSpawned = 4;
    
    public event EventHandler OnPlatesSpawned; 
    public event EventHandler OnPlatesRemoved; 
    
    private CountupTimer _spawnPlateTimer;
    private int _platesSpawnedCount;

    private void Start()
    {
        _spawnPlateTimer = new CountupTimer(spawnTimerMax);
        _spawnPlateTimer.Start();
    }

    private void Update()
    {
        // If spawn timer reach limit, reset timer
        // And if plates spawned count less than max spawned limit
        // increment _platesSpawnedCount & invoke spawend event
        if (_spawnPlateTimer.IsFinished)
        {
            _spawnPlateTimer.Reset(spawnTimerMax);
            _spawnPlateTimer.Start();
            if (_platesSpawnedCount < maxPlatesSpawned)
            {
                _platesSpawnedCount++;
                OnPlatesSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    
    public override void Interact(Player player)
    {
        // If _platesSpawnedCount > 0 & player is not carried anything
        // then create new plate object, decrement counter and pass it to the player
        if (_platesSpawnedCount > 0 && !player.HasKitchenObject())
        {
            var plateObject = Instantiate(plateKitchenObject.prefab);
            player.SetKitchenObject(plateObject.GetComponent<KitchenObject>());
            
            _platesSpawnedCount--;
            OnPlatesRemoved?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnDestroy()
    {
        _spawnPlateTimer.Stop();
        _spawnPlateTimer.Dispose();
    }
}
