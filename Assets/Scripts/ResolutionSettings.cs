using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionSettings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullScreenToggle;

    private List<(int width, int height)> _listResolutions;

    private void Start()
    {
        _listResolutions = Screen.resolutions
            .Select(res => (res.width, res.height))
            .Distinct()
            .Reverse()
            .ToList();
            
        resolutionDropdown.ClearOptions();
        
        List<string> resolutionOptions = new List<string>();
        int currentResolutionIndex = 0;

        for (var i = 0; i < _listResolutions.Count; i++)
        {
            var resolution = _listResolutions[i];
            var option = resolution.width + "x" + resolution.height;
            resolutionOptions.Add(option);

            if (resolution.width == Screen.currentResolution.width && 
                resolution.height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        fullScreenToggle.isOn = Screen.fullScreen;
        
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        fullScreenToggle.onValueChanged.AddListener(SetFullScreen);
    }

    public void SetResolution(int resolutionIndex)
    {
        var resolution = _listResolutions[resolutionIndex];
        var isFullScreen = Screen.fullScreen;
        Screen.SetResolution(resolution.width, resolution.height, isFullScreen);
        resolutionDropdown.value = resolutionIndex;
    }
    
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        fullScreenToggle.isOn = isFullScreen;
    }
}
