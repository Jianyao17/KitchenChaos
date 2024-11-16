using System;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private float footstepTimerMax = .5f;

    public static event EventHandler OnAnyPlayerMoved;
    
    private Player _player;
    private float _footstepTimer;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        _footstepTimer -= Time.deltaTime;
        if (_footstepTimer < 0f)
        {
            _footstepTimer = footstepTimerMax;
            if (_player.IsWalking)
                OnAnyPlayerMoved?.Invoke(this, EventArgs.Empty);
        }
    }
}
