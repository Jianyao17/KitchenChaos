using System;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSo;
    
    public event EventHandler OnPlayerInteract;
    
    public override void Interact(Player player)
    {
        // Return if player already carry kitchen object 
        if (player.HasKitchenObject()) return;
        
        // Instantiate new kitchen object type from scriptable object
        var kitchenObject = Instantiate(kitchenObjectSo.prefab);
        player.SetKitchenObject(kitchenObject.GetComponent<KitchenObject>());
        
        OnPlayerInteract?.Invoke(this, EventArgs.Empty);
    }

    public Sprite GetKitchenObjectSprite() 
        => kitchenObjectSo.sprite;
}