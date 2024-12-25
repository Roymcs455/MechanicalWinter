using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="AudioShoot config", menuName ="Guns/Audio Configuration", order =5)]
public class AudioConfigurationSO : ScriptableObject
{
    [Range(0,1f)]
    public float Volume = 1.0f;
    public AudioClip[] fireClips;
    public void PlayShootingClip(AudioSource audioSource)
    {
        audioSource.PlayOneShot(fireClips[Random.Range(0, fireClips.Length)]);
    }
}
