public class DeliveryCounter : BaseCounter
{
    public static DeliveryCounter Instance;
    
    private void Awake()
    {
        Instance = this;
    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject() && 
            player.GetKitchenObject().TryGetPlateObject(out var plateObject))
        {
            DeliveryManager.Instance.DeliverRecipe(plateObject);
            player.GetAndClearKitchenObject().DestroySelf();
        }
    }
}
