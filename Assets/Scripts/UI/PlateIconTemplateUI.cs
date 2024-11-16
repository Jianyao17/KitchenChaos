using UnityEngine;
using UnityEngine.UI;

public class PlateIconTemplate : MonoBehaviour
{
    [SerializeField] private Image imageIcon;

    public void SetKitchenObjectIcon(KitchenObjectSO kitchenObject) 
        => imageIcon.sprite = kitchenObject.sprite;
}