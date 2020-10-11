using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovetestScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            gameObject.transform.position += Vector3.up*10;
        }
        if (Input.GetKey(KeyCode.S))
        {
            gameObject.transform.position += Vector3.down*10;
        }
        if (Input.GetKey(KeyCode.A))
        {
            gameObject.transform.position += Vector3.left*10;
        }
        if (Input.GetKey(KeyCode.D))
        {
            gameObject.transform.position += Vector3.right*10;
        }
    }
}
