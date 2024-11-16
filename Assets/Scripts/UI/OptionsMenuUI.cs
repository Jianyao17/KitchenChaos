using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class OptionsMenuUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private GameObject rebindUI;
    [SerializeField] private List<KeyBindingButton> keyBindingButtons;
    
    public event EventHandler OnCloseEvent;
    
    
    private void Start()
    {
        HideRebindUI();
        musicVolumeSlider.value = AudioManager.Instance.MusicVolume;
        sfxVolumeSlider.value = AudioManager.Instance.SFXVolume;
        
        musicVolumeSlider.onValueChanged.AddListener(volume=> AudioManager.Instance.MusicVolume = volume);
        sfxVolumeSlider.onValueChanged.AddListener(volume=> AudioManager.Instance.SFXVolume = volume);
        
        OnCloseEvent += CloseOptionsUI;
        closeButton.onClick.AddListener(() => OnCloseEvent?.Invoke(this, EventArgs.Empty));

        // Display current binding settings to keyBindingButtons
        foreach (var bindingButton in keyBindingButtons)
        {
            var keyType = bindingButton.keyBindType;
            bindingButton.labelText.text = PlayerInput.Instance.GetKeyBindText(keyType);
        }
        
        // Add listener for rebinding action
        foreach (var bindingButton in keyBindingButtons)
        {
            bindingButton.bindButton.onClick.AddListener(
                () => OnRebindAction(bindingButton));
        }
    }

    private void OnRebindAction(KeyBindingButton bindingButton)
    {
        ShowRebindUI();
        PlayerInput.Instance.RebindKeyBinding(bindingButton.keyBindType, () =>
        {
            bindingButton.labelText.text = PlayerInput.Instance.GetKeyBindText(bindingButton.keyBindType);
            HideRebindUI();
        });
    }
    
    public void CloseOptionsUI(object sender, EventArgs eventArgs)
    {
        gameObject.SetActive(false);
    }
    
    private void ShowRebindUI() => rebindUI.SetActive(true);
    private void HideRebindUI() => rebindUI.SetActive(false);
}

[Serializable]
public struct KeyBindingButton
{
    public KeyBindType keyBindType;
    public Button bindButton;
    public TextMeshProUGUI labelText;
}