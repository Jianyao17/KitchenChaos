using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveWarningUI : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;

    private void Start()
    {
        gameObject.SetActive(false);
        stoveCounter.OnStateChanged += ToggleWarningUI;
    }

    private void ToggleWarningUI(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        if (e.State == StoveCounter.FryingState.Fried) gameObject.SetActive(true);
        else gameObject.SetActive(false);
    }
}
