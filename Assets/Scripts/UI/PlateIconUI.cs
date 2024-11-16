using UnityEngine;

public class PlateIconUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private GameObject iconTemplate;
    
    private void Start()
    {
        iconTemplate.SetActive(false);
        plateKitchenObject.OnIngredientsAdded += UpdateUIVisual;
    }

    private void UpdateUIVisual(object sender, PlateKitchenObject.OnIngredientsAddedEventArgs e)
    {
        // Clean & Destroy icons game object except icon template 
        foreach (Transform child in transform)
        {
            if (child == iconTemplate.transform) continue;
            Destroy(child.gameObject);
        }
        
        // Spawn new list of kitchen object icon
        foreach (var kitchenObjectSO in plateKitchenObject.ListAddedIngredients)
        {
            var iconObject = Instantiate(iconTemplate, transform);
            iconObject.name = "IconIngredient";
            iconObject.SetActive(true);
            iconObject.GetComponent<PlateIconTemplate>().SetKitchenObjectIcon(kitchenObjectSO);
        }
    }
}
