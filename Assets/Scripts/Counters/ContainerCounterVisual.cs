using System;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    [SerializeField] private ContainerCounter containerCounter;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Animator _animator;

    private static readonly int OpenClose = Animator.StringToHash("OpenClose");
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        containerCounter.OnPlayerInteract += PlayAnimation;
        spriteRenderer.sprite = containerCounter.GetKitchenObjectSprite();
    }

    private void PlayAnimation(object sender, EventArgs e) 
        => _animator.SetTrigger(OpenClose);
}