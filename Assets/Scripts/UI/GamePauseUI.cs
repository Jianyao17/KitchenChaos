using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private OptionsMenuUI optionsMenuUI;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button mainMenuButton;

    private void Start()
    {
        gameObject.SetActive(false);
        optionsMenuUI.gameObject.SetActive(false);
        
        GameManager.Instance.OnGamePaused += ShowUI;
        GameManager.Instance.OnGameUnpaused += HideUI;
        GameManager.Instance.OnGameUnpaused += optionsMenuUI.CloseOptionsUI;

        resumeButton.onClick.AddListener(() 
            => GameManager.Instance.ToggleGamePause(this, EventArgs.Empty));
        mainMenuButton.onClick.AddListener(() 
            => SceneManager.LoadScene("MainMenu"));
        
        optionsButton.onClick.AddListener(() 
            => optionsMenuUI.gameObject.SetActive(true));
    }
    

    private void HideUI(object sender, EventArgs e) 
        => gameObject.SetActive(false);

    private void ShowUI(object sender, EventArgs e) 
        => gameObject.SetActive(true);
}
