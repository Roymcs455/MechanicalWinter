using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudio : MonoBehaviour
{
    [SerializeField]
    private AudioClip stepClip;
    [SerializeField]
    private AudioClip jumpClip;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayStep()
    {
        audioSource.PlayOneShot(stepClip);
    }
    public void PlayJump()
    {
        audioSource.PlayOneShot(jumpClip);
    }
}
