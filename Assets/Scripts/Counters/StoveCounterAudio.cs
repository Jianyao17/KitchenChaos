using ImprovedTimers;
using UnityEngine;

public class StoveCounterAudio : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField, Range(0f, 1f)] private float playInterval;
    
    private AudioSource _audioSource;
    private CountdownTimer _soundTimer;
    private bool _playWarningSound;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _soundTimer = new CountdownTimer(playInterval);
    }

    private void Start()
    {
        stoveCounter.OnStateChanged += PlayAudioLoop;
        _soundTimer.Start();
    }

    private void Update()
    {
        if (_playWarningSound && _soundTimer.IsFinished)
        {
            _soundTimer.Reset();
            _soundTimer.Start();
            AudioManager.Instance.PlayWarningSound(transform.position);
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
    
    private void OnDestroy()
    {
        _soundTimer.Stop();
        _soundTimer.Dispose();
    }
}
