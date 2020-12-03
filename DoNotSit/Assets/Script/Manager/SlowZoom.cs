using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowZoom : MonoBehaviour
{
    private FollowCamera camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowCamera>();
    }

    // Update is called once per frame
    void Update()
    {
     
    }
    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            camera.zoom = true;
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            camera.zoom = false;
        }
    }

}
