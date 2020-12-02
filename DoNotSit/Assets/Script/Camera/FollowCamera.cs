using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    GameObject player;
    public Vector3 offset;
    public bool zoom;
    Vector3 posi;
    public  bool shack;//揺れ判定
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
        if (zoom == false && shack == true)
        {
            var pos = transform.localPosition;
            transform.localPosition = new Vector3(pos.x, posi.y, posi.z);
            shack = false;
        }
        if (zoom == true && shack==false)
        {
            transform.position = new Vector3(player.transform.position.x + offset.x,
                player.transform.position.y,
                player.transform.position.z - offset.z);
        }
    }
}
