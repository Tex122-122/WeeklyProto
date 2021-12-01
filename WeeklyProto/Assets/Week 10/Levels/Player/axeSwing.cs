using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class axeSwing : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] clips;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Swing()
    {
        AudioClip clip = GetRandomClip();
        audioSource.PlayOneShot(clip);

    }

    private AudioClip GetRandomClip()
    {
        return clips[UnityEngine.Random.Range(0, clips.Length)];
    }
}
