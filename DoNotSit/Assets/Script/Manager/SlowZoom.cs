using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowZoom : MonoBehaviour
{
    private CameraShakeScript shake;
    private SlowTime slow;
    // Start is called before the first frame update
    void Start()
    {
        shake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShakeScript>();
        slow = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SlowTime>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            slow.slowMotion = true;
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")//ボム外した場合
        {
            slow.slowMotion = false;
        }
    }

}
