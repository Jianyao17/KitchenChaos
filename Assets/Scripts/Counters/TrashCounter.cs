
using System;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnAnyObjectTrash;
    
    public override void Interact(Player player)
    {
        // Destroy player kitchen object
        if (player.HasKitchenObject())
        {
            player.GetAndClearKitchenObject().DestroySelf();
            OnAnyObjectTrash?.Invoke(this, EventArgs.Empty);
        }
    }
}