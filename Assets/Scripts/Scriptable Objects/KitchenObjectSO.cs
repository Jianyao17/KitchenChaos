using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Kitchen Object")]
public class KitchenObjectSO : ScriptableObject
{
    public string objectName;
    public GameObject prefab;
    public Sprite sprite;
}