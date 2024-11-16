using TMPro;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsText;
    [SerializeField] private float updateInterval = 0.5f;
    private float _timer;
    private int _frameCount;
    private float _fps;

    private void Update()
    {
        _frameCount++;
        _timer += Time.unscaledDeltaTime;

        if (_timer >= updateInterval)
        {
            _fps = _frameCount / _timer;
            fpsText.text = $"FPS: {_fps:F1}";
            _frameCount = 0;
            _timer = 0;
        }
    }
}