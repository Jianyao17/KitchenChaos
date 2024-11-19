using System;
using ImprovedTimers;
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
    
    [SerializeField] private float waitingToStartDuration = 1f;
    [SerializeField] private float startCountdownDuration = 3f;
    [SerializeField] private float gameRunningDuration = 30f;
    
    public bool IsGamePaused { get; private set; }
    public bool IsCountdownToStart => _gameState == GameState.CountdownToStart;
    public bool IsGameRunning => _gameState == GameState.GameRunning;
    public bool IsGameOver => _gameState == GameState.GameOver;
    
    public float CountdownToStart => _gameState == GameState.CountdownToStart ? _timer.CurrentTime : -1f;
    public float GameRunningTimeNormalized => 1 - (_timer.CurrentTime / gameRunningDuration);

    public event EventHandler OnGameStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;
    
    private GameState _gameState;
    private CountdownTimer _timer;

    private void Awake()
    {
        Instance = this;
        _timer = new CountdownTimer(0);
        SetGameState(GameState.WaitingToStart, waitingToStartDuration);
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
        if (!_timer.IsFinished) return;
        
        switch (_gameState)
        {
            case GameState.WaitingToStart:
                SetGameState(GameState.CountdownToStart, startCountdownDuration);
                break;
            
            case GameState.CountdownToStart:
                SetGameState(GameState.GameRunning, gameRunningDuration);
                break;
            
            case GameState.GameRunning:
                SetGameState(GameState.GameOver, 0f);
                break;
            
            case GameState.GameOver:
                break;
        }
    }

    // Set game state with running duration
    private void SetGameState(GameState newState, float durationSeconds = 0)
    {
        _gameState = newState;
        _timer.Reset(durationSeconds);
        _timer.Start();
        OnGameStateChanged?.Invoke(this, EventArgs.Empty);
    }
    
    private void OnDestroy()
    {
        _timer.Stop();
        _timer.Dispose();
    }
}
