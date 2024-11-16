using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private Color successColor = new Color(92, 217, 9);
    [SerializeField] private Color failedColor = new Color(217, 60, 9);
    [SerializeField] private Sprite successIcon;
    [SerializeField] private Sprite failedIcon;

    private static readonly int Popup = Animator.StringToHash("Popup");
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        gameObject.SetActive(false);
        DeliveryManager.Instance.OnRecipeSuccess += DisplaySuccessUI;
        DeliveryManager.Instance.OnRecipeFailed += DisplayFailedUI;
    }

    private void DisplayFailedUI(object sender, DeliveryManager.AudioPosEventArgs e)
    {
        gameObject.SetActive(true);
        _animator.SetTrigger(Popup);
        resultText.text = "Delivery Failed";
        backgroundImage.color = failedColor;
        iconImage.sprite = failedIcon;
    }

    private void DisplaySuccessUI(object sender, DeliveryManager.AudioPosEventArgs e)
    {
        gameObject.SetActive(true);
        _animator.SetTrigger(Popup);
        resultText.text = "Delivery Success";
        backgroundImage.color = successColor;
        iconImage.sprite = successIcon;
    }
}
