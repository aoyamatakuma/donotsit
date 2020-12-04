using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    GameObject player;
    public Vector3 offset;
    public Vector3 Maxzoom;
    public bool zoom;
    Vector3 posi;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        posi = gameObject.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x + offset.x,
               transform.position.y,
               transform.position.z);
        
        if (zoom == false)
        {
            offset.z--;
            if (offset.z > 1)
            {
                transform.position -= offset;
            }
            if (offset.z < 1)
            {
                offset.z = 1;
                transform.position = transform.position;
            }
        }
        if (zoom == true)//スロー演出させてる奴
        {
            offset.z++;
            if (offset.z < Maxzoom.z)
            {
                transform.position += offset;
            }
            if (offset.z >= Maxzoom.z)
            {
                offset.z = Maxzoom.z;
                transform.position = transform.position;
            }
        }
    }
}
