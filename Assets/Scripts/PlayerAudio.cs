using System;
using ImprovedTimers;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private float footstepTimerMax = .5f;

    public static event EventHandler OnAnyPlayerMoved;
    
    private Player _player;
    private CountdownTimer _footstepTimer;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _footstepTimer = new CountdownTimer(footstepTimerMax);
    }

    private void Update()
    {
        if (_footstepTimer.IsFinished)
        {
            _footstepTimer.Reset(footstepTimerMax);
            _footstepTimer.Start();
            
            if (_player.IsWalking)
                OnAnyPlayerMoved?.Invoke(this, EventArgs.Empty);
        }
    }
    
    private void OnDestroy()
    {
        _footstepTimer.Stop();
        _footstepTimer.Dispose();
    }
}
