using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bondTest : MonoBehaviour
{

    Rigidbody playerRig;
    Vector3 playerVec,refVec;
    public Transform[] PosAction = new Transform[4];

    float playerRot;
    // Start is called before the first frame update
    void Start()
    {
        playerRig = GetComponent<Rigidbody>();
        SetVecPos();
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
        test();
        Debug.Log(PosAction[0].position);
    }
    private void OnCollisionEnter(Collision col)
    {
        //refVec.x = -playerVec.y;
        //refVec.y = -playerVec.x;

        //playerRig.velocity = refVec*100;
        //playerVec = refVec;
        //Debug.Log(refVec);
    }
   
    public void SetVecPos()
    {
        PosAction[0].position = gameObject.transform.position +
            new Vector3(gameObject.transform.localScale.x * 0.5f,
            gameObject.transform.localScale.y * 0.5f, 0);
        PosAction[1].position = gameObject.transform.position +
            new Vector3(gameObject.transform.localScale.x * 0.5f,
            -gameObject.transform.localScale.y * 0.5f, 0);
        PosAction[2].position = gameObject.transform.position +
            new Vector3(-gameObject.transform.localScale.x * 0.5f,
            -gameObject.transform.localScale.y * 0.5f, 0);
        PosAction[3].position = gameObject.transform.position +
            new Vector3(-gameObject.transform.localScale.x * 0.5f,
            gameObject.transform.localScale.y * 0.5f, 0);
    }

    public void test()
    {
        float a = gameObject.transform.localEulerAngles.z;
        PosAction[0].RotateAround(gameObject.transform.position, transform.up, a);
    }
}
