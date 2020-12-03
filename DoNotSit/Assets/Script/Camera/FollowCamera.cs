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
        if(offset.z>=Maxzoom.z)
        {
            offset.z = Maxzoom.z;
        }
        if (offset.z < 1)
        {
            offset.z = 1;
        }
        if (zoom == false)
        {
            offset.z--;
            //transform.position = new Vector3(player.transform.position.x,
            //    player.transform.position.y,
            //    player.transform.position.z + offset.z);
        }
        if (zoom == true)//スロー演出させてる奴
        {
            offset.z++;
            //transform.position = new Vector3(player.transform.position.x,
            //    player.transform.position.y,
            //    player.transform.position.z + offset.z);
        }
    }
}
