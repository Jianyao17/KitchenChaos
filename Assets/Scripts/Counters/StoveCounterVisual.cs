using System;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private GameObject stoveOnGameObject;
    [SerializeField] private GameObject particleGameObject;

    private void Start()
    {
        stoveCounter.OnStateChanged += OnStateChanged;
    }

    private void OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool showVisual = e.State is StoveCounter.FryingState.Frying or StoveCounter.FryingState.Fried;
        stoveOnGameObject.SetActive(showVisual);
        particleGameObject.SetActive(showVisual);
    }
}
