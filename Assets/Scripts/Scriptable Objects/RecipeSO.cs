using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Recipe")]
public class RecipeSO : ScriptableObject
{
    public string recipeName;
    public List<KitchenObjectSO> KitchenObjects;

}