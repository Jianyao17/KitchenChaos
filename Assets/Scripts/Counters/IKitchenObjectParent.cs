using UnityEngine;

public interface IKitchenObjectParent
{
    // Get spawn position for kitchen object
    public Transform GetSpawnPoint();
    
    // Get this kitchen object
    public KitchenObject GetKitchenObject();
    
    // Get this kitchen object & clear reference
    public KitchenObject GetAndClearKitchenObject();
    
    // Check if kitchen object exist
    public bool HasKitchenObject();
    
    // Set/transfer Kitchen object to this instance
    public bool SetKitchenObject(KitchenObject kitchenObject);
}