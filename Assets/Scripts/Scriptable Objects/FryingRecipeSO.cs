using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Frying Recipe")]
public class FryingRecipeSO : ScriptableObject
{
    public KitchenObjectSO inputObject;
    public KitchenObjectSO friedObject;
    public KitchenObjectSO burnedObject;
    public float fryingTimerMax;
    public float burningTimerMax;
}