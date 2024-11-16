using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenSOToGameObject
    {
        public KitchenObjectSO kitchenSO;
        public GameObject gameObject;
    }
    
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenSOToGameObject> listOfVisualKitchenObjects;

    private void Start()
    {
        plateKitchenObject.OnIngredientsAdded += UpdateCompleteVisual;
        
        // Deactivate all visual at start
        foreach (var visualKitchenObject in listOfVisualKitchenObjects)
            visualKitchenObject.gameObject.SetActive(false);
    }

    private void UpdateCompleteVisual(object sender, PlateKitchenObject.OnIngredientsAddedEventArgs e)
    {
        // Activate selected visual
        listOfVisualKitchenObjects
            .FirstOrDefault(visualObject => visualObject.kitchenSO == e.KitchenObjectSO)
            .gameObject.SetActive(true);
    }
}
