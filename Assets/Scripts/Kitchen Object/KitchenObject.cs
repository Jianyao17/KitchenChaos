using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSo;

    public KitchenObjectSO GetKitchenObjectSO()
        => kitchenObjectSo;

    public bool TryGetPlateObject(out PlateKitchenObject plateObject)
    {
        // Return true and give plate object if this is PlateKitchenObject 
        if (this is PlateKitchenObject)
        {
            plateObject = this as PlateKitchenObject;
            return true;
        }
        plateObject = null;
        return false;
    }
    public void DestroySelf() 
        => Destroy(gameObject);
}