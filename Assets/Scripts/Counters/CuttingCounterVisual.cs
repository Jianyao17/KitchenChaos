using System;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    [SerializeField] private CuttingCounter containerCounter;
    private Animator _animator;

    private static readonly int Cut = Animator.StringToHash("Cut");
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        containerCounter.OnCutting += PlayAnimation;
    }

    private void PlayAnimation(object sender, EventArgs e) 
        => _animator.SetTrigger(Cut);
}