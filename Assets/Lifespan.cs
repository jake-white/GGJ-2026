using UnityEngine;

public class Lifespan : MonoBehaviour
{
    public float seconds;
    float spawnTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - spawnTime > seconds)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
