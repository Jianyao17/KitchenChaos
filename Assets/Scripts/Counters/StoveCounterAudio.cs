using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterAudio : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    
    private AudioSource _audioSource;
    private float _soundTimer;
    private bool _playWarningSound;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        stoveCounter.OnStateChanged += PlayAudioLoop;
    }

    private void Update()
    {
        if (_playWarningSound)
        {
            _soundTimer -= Time.deltaTime;
            if (_soundTimer <= 0)
            {
                _soundTimer = .2f;
                AudioManager.Instance.PlayWarningSound(transform.position);
            }
        }
    }

    private void PlayAudioLoop(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool isFrying = e.State is StoveCounter.FryingState.Frying or StoveCounter.FryingState.Fried;

        if (isFrying)
        {
            _audioSource.Play();
            if (e.State == StoveCounter.FryingState.Fried)
                _playWarningSound = true;
        }
        else
        {
            _audioSource.Pause();
            _playWarningSound = false;
        }
    }
}
