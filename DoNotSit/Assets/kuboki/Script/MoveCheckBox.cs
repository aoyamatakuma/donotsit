using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCheckBox : MonoBehaviour
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
            Debug.Log("接触した");
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("離れました");
            if (playerPos.x<col.gameObject.transform.position.x)
            {
                cameraCS.RightMoveCamera();
            }
            else if(playerPos.x>col.gameObject.transform.position.x)
            {
                cameraCS.LeftMoveCamera();
            }
            else if(playerPos.y<col.gameObject.transform.position.y)
            {
                cameraCS.UpMoveCamera();
            }
            else if (playerPos.y > col.gameObject.transform.position.y)
            {
                cameraCS.DownMoveCamera();
            }
        }
    }
}
