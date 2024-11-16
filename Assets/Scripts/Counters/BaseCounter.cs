using System;
using UnityEngine;

public abstract class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] protected Transform spawnPoint;

    public static event EventHandler OnAnyDrop;
    
    public event EventHandler OnShowHighlight; 
    public event EventHandler OnHideHighlight; 
    
    // Current kitchen object on the counter table
    protected KitchenObject _KitchenObject;
    
    public void ShowHighlight() 
        => OnShowHighlight?.Invoke(this, EventArgs.Empty);

    public void HideHighlight() 
        => OnHideHighlight?.Invoke(this, EventArgs.Empty);
    
    
    public virtual void Interact(Player player)
    { }

    public virtual void InteractAlternate(Player player)
    { }
    
    // Get spawn position for kitchen object
    public Transform GetSpawnPoint() => spawnPoint;
    
    // Get this kitchen object
    public KitchenObject GetKitchenObject() => _KitchenObject;
    
    // Get/transfer this kitchen object
    // Remove reference to current kitchen object
    public KitchenObject GetAndClearKitchenObject()
    {
        var kitchenObject = _KitchenObject;
        _KitchenObject = null;
        return kitchenObject;
    }

    // Check if kitchen object exist
    public bool HasKitchenObject() => _KitchenObject is not null;
    
    // Set/transfer Kitchen object to this instance
    public bool SetKitchenObject(KitchenObject kitchenObject)
    {
        // Return false if space not available 
        if (_KitchenObject is not null) return false;
        
        _KitchenObject = kitchenObject;
        _KitchenObject.transform.SetParent(GetSpawnPoint());
        _KitchenObject.transform.localPosition = Vector3.zero;
        
        OnAnyDrop?.Invoke(this, EventArgs.Empty);
        return true;
    }

    // Spawn new kitchen object and set kitchen object to this instance
    protected void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO)
    {
        // Return if space not available 
        if (_KitchenObject is not null) return;
        
        var newKitchenObject = Instantiate(kitchenObjectSO.prefab);
        SetKitchenObject(newKitchenObject.GetComponent<KitchenObject>());
    }
}