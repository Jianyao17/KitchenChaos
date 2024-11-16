public class ClearCounter : BaseCounter
{

    public override void Interact(Player player)
    {
        // If there is no kitchen object here & player is carrying kitchen object
        // then pass that kitchen object here
        if (_KitchenObject is null && player.HasKitchenObject())
        {
            SetKitchenObject(player.GetAndClearKitchenObject());
        }
        // Else If there is kitchen object here
        else if (_KitchenObject is not null)
        {   
            // And player is carrying kitchen object
            if (player.HasKitchenObject())
            {
                // And that player kitchen object is type PlateKitchenObject
                if (player.GetKitchenObject().TryGetPlateObject(out var playerPlateObject))
                {
                    // Try to add this kitchen object to player's plate object 
                    if (playerPlateObject?.TryAddIngredient(_KitchenObject.GetKitchenObjectSO()) == true)
                        GetAndClearKitchenObject().DestroySelf();
                }
                // And this kitchen object is type PlateKitchenObject
                else if (_KitchenObject.TryGetPlateObject(out var thisPlateObject))
                {
                    // Try to add player kitchen object to this plate object 
                    if (thisPlateObject?.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()) == true)
                        player.GetAndClearKitchenObject().DestroySelf();
                }
            }
            // And player is not carrying anything
            else if (!player.HasKitchenObject())
            {
                // then pass this kitchen object to player
                player.SetKitchenObject(GetAndClearKitchenObject());
            }
        }
    }
}
