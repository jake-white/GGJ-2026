using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonBehavior<AudioManager>
{
    public AudioSource commander;
    public List<AudioClip> commanderVoiceLines;
    private AudioSource music;
    int nextVoice = -1;
    void Start()
    {
        music = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(nextVoice > -1 && !commander.isPlaying)
        {
            PlayCommanderVoice(nextVoice);
            nextVoice = -1;
        }
    }

    public void QueueCommanderVoice(int index)
    {
        if (commander.isPlaying)
        {
            nextVoice = index;
        }
        else
        {
            PlayCommanderVoice(index);
        }
    }

    public void PlayCommanderVoice(int index)
    {
        commander.Stop();
        commander.clip = commanderVoiceLines[index];
        commander.Play();
    }
}
