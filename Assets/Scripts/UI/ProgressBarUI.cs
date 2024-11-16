using System;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject kitchenCounter;
    [SerializeField] private Image progressBar;

    private IHasProgressBar _objectHasProgress;
    
    private void Start()
    {
        _objectHasProgress = kitchenCounter.GetComponent<IHasProgressBar>();
        if (_objectHasProgress == null) 
            throw new NullReferenceException("Game object has no IHasProgressBar implementation component");
        
        _objectHasProgress.OnProgressChanged += OnProgressChanged;
        progressBar.fillAmount = 0f;
        Hide();
    }

    private void OnProgressChanged(object sender, ProgressEventArgs e)
    {
        progressBar.fillAmount = e.ProgressNormalized;
        
        // Hide if value zero or one
        if (e.ProgressNormalized is 0f or 1f) Hide();
        else Show();
    }

    private void Show() => gameObject.SetActive(true);
    
    private void Hide() => gameObject.SetActive(false);
}