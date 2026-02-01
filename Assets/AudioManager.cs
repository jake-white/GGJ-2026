using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource music;
    void Start()
    {
        music = GetComponent<AudioSource>();
    }
}
