using UnityEngine;

public class SFXEmitAndDie : MonoBehaviour
{
    public AudioSource source;
    bool started = false;
    public void Play(AudioClip clip) {
        source.clip = clip;
        source.Play();
        started = true;
    }

    private void Update()
    {
        if(started && !source.isPlaying)
        {
            GameObject.Destroy(gameObject);
        }
    }

}
