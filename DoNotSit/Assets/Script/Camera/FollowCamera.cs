using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    GameObject player;
    public GameObject target;
    public Vector3 offset;
    public Vector3 Maxzoom;
    public bool zoom;
    Vector3 posi;
    private float speed=0.1f;
    private bool trans;
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
        if (zoom == false )//元の画面に戻す処理
        {
            if (offset.z > 0)
            {
                offset.z--;
                transform.position -= offset * speed;
                trans = true;
            }
            if (offset.z < 1)
            {
                if (trans == true)
                {
                    var pos = transform.localPosition;
                    transform.localPosition = new Vector3(pos.x, posi.y, posi.z);
                    trans = false;
                }
            }
        }
        if (zoom == true)//スロー演出させてる処理
        {
            if (offset.z < Maxzoom.z)
            {
                offset.z++;
                transform.position += offset * speed;
            }
            if (offset.z >= Maxzoom.z)
            {
                offset.z = Maxzoom.z;
            }
        }
    }
   
}
