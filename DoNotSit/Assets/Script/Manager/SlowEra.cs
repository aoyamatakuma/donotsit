using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEra : MonoBehaviour
{
    private CameraShakeScript camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShakeScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            camera.durations = 0;
            camera.magnitudes = 0;
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            camera.durations = 0.5f;
            camera.magnitudes = 0.5f;
        }
    }
}
