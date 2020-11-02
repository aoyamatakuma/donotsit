using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSample : MonoBehaviour
{
    public float speed;
    public GameObject player;
    public float delay;
    float cnt;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(player.transform.position.x,
            transform.position.y,
            transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        cnt += Time.deltaTime;
        if(cnt > delay)
        {
            Vector3 move = transform.position;
            move.x += speed * Time.deltaTime;
            transform.position = move;
        }
      
    }
}
