using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClipReferenceSO audioClipReference;
    [SerializeField] private AudioSource musicAudioSource;

    public static AudioManager Instance { get; private set; }
    
    public float MusicVolume
    {
        get => musicAudioSource.volume;
        set
        {
            musicAudioSource.volume = value;
            PlayerPrefs.SetFloat("MusicVolume", value);
            PlayerPrefs.Save();
        }
    }

    public float SFXVolume
    {
        get => _sfxVolume;
        set
        {
            _sfxVolume = value;
            PlayerPrefs.SetFloat("SFXVolume", value);
            PlayerPrefs.Save();
        }
    }

    private const float SFX_VOL_MULTIPLIER = 40f;
    private float _sfxVolume = .6f;

    private void Awake()
    {
        Instance = this;
        musicAudioSource.volume = PlayerPrefs.GetFloat("MusicVolume", 0.4f);
        _sfxVolume = PlayerPrefs.GetFloat("SFXVolume", _sfxVolume);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += (_, e) 
            => { PlayAudio(audioClipReference.deliverySuccess, e.Position, _sfxVolume); };
        
        DeliveryManager.Instance.OnRecipeFailed += (_, e) 
            => { PlayAudio(audioClipReference.deliveryFail, e.Position, _sfxVolume); };

        CuttingCounter.OnAnyCutting += (sender, args) =>
        {
            var audioPos = ((CuttingCounter)sender).transform.position;
            PlayAudio(audioClipReference.chop, audioPos, _sfxVolume);
        };

        Player.OnPlayerPickUp += (sender, args) =>
        {
            var audioPos = ((Player)sender).transform.position;
            PlayAudio(audioClipReference.objectPickup, audioPos, _sfxVolume);
        };

        BaseCounter.OnAnyDrop += (sender, args) =>
        {
            var audioPos = ((BaseCounter)sender).transform.position;
            PlayAudio(audioClipReference.objectDrop, audioPos, _sfxVolume);
        };

        TrashCounter.OnAnyObjectTrash += (sender, args) =>
        {
            var audioPos = ((TrashCounter)sender).transform.position;
            PlayAudio(audioClipReference.trash, audioPos, _sfxVolume);
        };

        PlayerAudio.OnAnyPlayerMoved += (sender, args) =>
        {
            var audioPos = ((PlayerAudio)sender).transform.position;
            PlayAudio(audioClipReference.footstep, audioPos, _sfxVolume);
        };
    }
    
    public void PlayWarningSound(Vector3 position)
        => PlayAudio(audioClipReference.warning, position, _sfxVolume);

    private void PlayAudio(AudioClip audioClip, float volume = 1.0f) 
        => PlayAudio(audioClip, Camera.main!.transform.position, volume * SFX_VOL_MULTIPLIER);

    private void PlayAudio(AudioClip audioClip, Vector3 pos, float volume = 1.0f) 
        => AudioSource.PlayClipAtPoint(audioClip, pos, volume * SFX_VOL_MULTIPLIER);

    private void PlayAudio(List<AudioClip> audioClips, float volume = 1.0f)
        => PlayAudio(audioClips, Camera.main!.transform.position, volume * SFX_VOL_MULTIPLIER);

    private void PlayAudio(List<AudioClip> audioClips, Vector3 pos, float volume = 1.0f) 
        => PlayAudio(audioClips[Random.Range(0, audioClips.Count)], pos, volume * SFX_VOL_MULTIPLIER);
}