using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipesDeliveredText;
    [SerializeField] private Button playAgainButton;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += UpdateUI;
        playAgainButton.onClick.AddListener(RestartGame);
        HideUI();
    }

    private void RestartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void UpdateUI(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsGameOver)
        {
            ShowUI();
            recipesDeliveredText.text = DeliveryManager.Instance.SuccessfulDeliveryCount.ToString();
        }
        else HideUI();
    }

    private void HideUI() 
        => gameObject.SetActive(false);

    private void ShowUI() 
        => gameObject.SetActive(true);
}
