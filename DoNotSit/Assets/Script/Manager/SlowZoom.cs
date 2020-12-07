using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowZoom : MonoBehaviour
{
    private CameraShakeScript shake;
    // Start is called before the first frame update
    void Start()
    {
        shake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShakeScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {

        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")//ボム外した場合
        {
         
        }
    }

}
