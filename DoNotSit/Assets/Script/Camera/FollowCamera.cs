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
    private float speed=0.1f;
    private CameraShakeScript camera;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        camera = GetComponent<CameraShakeScript>();
        posi = gameObject.transform.position;
    }
    
    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x + offset.x,
               transform.position.y,
               transform.position.z);
        //if (zoom == false)
        //{
        //    if (offset.z > 0)
        //    {
        //        offset.z--;
        //        transform.position -= offset * speed;
        //    }
        //    if (offset.z < 1)
        //    {
        //        offset.z = 0;
        //        var pos = transform.localPosition;
        //        transform.localPosition = new Vector3(pos.x, posi.y, posi.z);
        //    }
        //}
        //if (zoom == true)//スロー演出させてる奴
        //{
        //    if (offset.z < Maxzoom.z)
        //    {
        //        offset.z++;
        //        transform.position += offset * speed;
        //        camera.slowFlag = true;
        //    }
        //    if (offset.z >= Maxzoom.z)
        //    {
        //        offset.z = Maxzoom.z;
        //    }
        //}
    }
   
}
