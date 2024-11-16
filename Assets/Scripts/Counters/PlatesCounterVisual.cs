using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private GameObject plateVisualPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnOffsetY = .1f;

    private List<GameObject> _listOfPlatesVisual = new();
    
    private void Start()
    {
        platesCounter.OnPlatesSpawned += SpawnPlateVisual;
        platesCounter.OnPlatesRemoved += RemovePlateVisual;
    }

    private void RemovePlateVisual(object sender, EventArgs e)
    {
        // Get last index element then remove/destroy from list
        var plateVisual = _listOfPlatesVisual[^1];
        _listOfPlatesVisual.Remove(plateVisual);
        Destroy(plateVisual);
    }

    private void SpawnPlateVisual(object sender, EventArgs e)
    {
        var plateVisual = Instantiate(plateVisualPrefab, spawnPoint);
        plateVisual.transform.localPosition = new Vector3(0, spawnOffsetY * _listOfPlatesVisual.Count, 0);
        _listOfPlatesVisual.Add(plateVisual);
    }
}
