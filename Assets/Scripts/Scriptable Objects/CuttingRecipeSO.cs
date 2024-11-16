using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Cutting Recipe")]
public class CuttingRecipeSO : ScriptableObject
{
    public KitchenObjectSO inputObject;
    public KitchenObjectSO outputObject;
    public int cuttingProgressMax;
}