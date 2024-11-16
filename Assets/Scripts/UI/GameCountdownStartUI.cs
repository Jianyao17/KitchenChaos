using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameCountdownStartUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += UpdateCountdown;
        HideCountdown();
    }

    private void Update()
    {
        countdownText.text = Mathf.Ceil(GameManager.Instance.CountdownToStart).ToString();
    }

    private void UpdateCountdown(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsCountdownToStart) ShowCountdown();
        else HideCountdown();
    }

    private void HideCountdown() 
        => gameObject.SetActive(false);

    private void ShowCountdown() 
        => gameObject.SetActive(true);
}
