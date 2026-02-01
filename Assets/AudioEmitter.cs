using UnityEngine;

public class AudioEmitter : MonoBehaviour
{
    public SFXEmitAndDie sfxPrefab;
    public AudioClip clip;
    public void Emit()
    {
        SFXEmitAndDie newSource = Instantiate(sfxPrefab);
        newSource.Play(clip);
    }
}
