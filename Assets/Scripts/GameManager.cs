using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private enum GameState
    {
        WaitingToStart,
        CountdownToStart,
        GameRunning,
        GameOver,
    }
    
    public static GameManager Instance;
    
    [SerializeField] private float gameRunningDuration = 30f;
    
    public bool IsGamePaused { get; private set; }
    public bool IsCountdownToStart => _gameState == GameState.CountdownToStart;
    public bool IsGameRunning => _gameState == GameState.GameRunning;
    public bool IsGameOver => _gameState == GameState.GameOver;
    
    public float CountdownToStart => _countDownToStartTimer;
    public float GameRunningTimeNormalized => 1 - (_gameRunningTimer / gameRunningDuration);

    public event EventHandler OnGameStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;
    
    private GameState _gameState;
    private float _waitingToStartTimer = 1f;
    private float _countDownToStartTimer = 3f;
    private float _gameRunningTimer;

    private void Awake()
    {
        Instance = this;
        _gameState = GameState.WaitingToStart;
    }

    private void Start()
    {
        IsGamePaused = false;
        PlayerInput.Instance.OnPauseAction += ToggleGamePause;
    }

    public void ToggleGamePause(object sender, EventArgs e)
    {
        IsGamePaused = !IsGamePaused;
        if (IsGamePaused)
        {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Update()
    {
        switch (_gameState)
        {
            case GameState.WaitingToStart:
                _waitingToStartTimer -= Time.deltaTime;
                if (_waitingToStartTimer < 0)
                {
                    _gameState = GameState.CountdownToStart;
                    OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            
            case GameState.CountdownToStart:
                _countDownToStartTimer -= Time.deltaTime;
                if (_countDownToStartTimer < 0)
                {
                    _gameState = GameState.GameRunning;
                    _gameRunningTimer = gameRunningDuration;
                    OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case GameState.GameRunning:
                _gameRunningTimer -= Time.deltaTime;
                if (_gameRunningTimer < 0)
                {
                    _gameState = GameState.GameOver;
                    OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case GameState.GameOver:
                break;
        }
    }
}
