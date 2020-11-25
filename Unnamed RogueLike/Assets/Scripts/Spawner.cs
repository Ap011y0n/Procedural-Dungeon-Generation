using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject entity_to_spawn;
    public float spawn_time = 5.0f;
    float time_left;

    // Start is called before the first frame update
    void Start()
    {
        time_left = spawn_time;
    }

    // Update is called once per frame
    void Update()
    {
        time_left -= Time.deltaTime;

        if(time_left <= 0)
        {
            Instantiate(entity_to_spawn, transform);
            time_left = spawn_time;
        }
    }
}
