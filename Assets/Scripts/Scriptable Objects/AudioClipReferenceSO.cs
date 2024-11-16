using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AudioClipReference")]
public class AudioClipReferenceSO : ScriptableObject
{
    public List<AudioClip> chop;
    public List<AudioClip> deliveryFail;
    public List<AudioClip> deliverySuccess;
    public List<AudioClip> footstep;
    public List<AudioClip> objectDrop;
    public List<AudioClip> objectPickup;
    public List<AudioClip> trash;
    public List<AudioClip> warning;
    public AudioClip stoveSizzle;
}