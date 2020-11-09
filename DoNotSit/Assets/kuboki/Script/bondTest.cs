using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bondTest : MonoBehaviour
{

    Rigidbody playerRig;
    Vector3 playerVec,refVec;

    float playerRot;
    // Start is called before the first frame update
    void Start()
    {
        playerRig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            playerRig.velocity = transform.up*100;
            playerVec = (transform.up * 100).normalized;
            Debug.Log(playerVec);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerRot++;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            playerRot--;
        }
        gameObject.transform.Rotate(0, 0, playerRot);
        playerRot = 0;
    }
    private void OnCollisionEnter(Collision col)
    {
        refVec.x = -playerVec.y;
        refVec.y = -playerVec.x;

        playerRig.velocity = refVec*100;
        playerVec = refVec;
        Debug.Log(refVec);
    }
}
