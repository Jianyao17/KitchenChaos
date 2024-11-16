using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameCountdownClockUI : MonoBehaviour
{
    [SerializeField] private Image pieTimerImage;
    [SerializeField] private Color startColor = new Color(165, 255, 45);
    [SerializeField] private Color endColor = new Color(255, 106, 45);
    
    private float _percentage = 0f;
    
    private void Awake()
    {
        pieTimerImage.fillAmount = _percentage;
        pieTimerImage.color = startColor;
    }

    private void Update()
    {
        _percentage = GameManager.Instance.IsGameRunning ? 
            GameManager.Instance.GameRunningTimeNormalized : 0f;
        
        pieTimerImage.fillAmount = _percentage;
        pieTimerImage.color = Color.Lerp(startColor, endColor, _percentage);
    }
}
