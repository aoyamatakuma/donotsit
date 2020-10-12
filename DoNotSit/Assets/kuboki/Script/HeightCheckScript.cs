using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightCheckScript : MonoBehaviour
{
    Vector3 playerPos;
    CameraScript cameraCS;


    // Start is called before the first frame update
    void Start()
    {
        cameraCS = transform.root.gameObject.GetComponent<CameraScript>();
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            playerPos = col.gameObject.transform.position;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if ( col.gameObject.transform.position.y>gameObject.transform.position.y)
            {
                if(playerPos.y<gameObject.transform.position.y)
                {
                    cameraCS.UpMoveCamera();
                }               
            }
            else if (col.gameObject.transform.position.y < gameObject.transform.position.y)
            {
                if (playerPos.y > gameObject.transform.position.y)
                {
                    cameraCS.DownMoveCamera();
                }
            }            
        }
    }
}
