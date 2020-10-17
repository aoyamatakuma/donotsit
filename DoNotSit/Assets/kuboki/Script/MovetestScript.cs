using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovetestScript : MonoBehaviour
{
    Rigidbody playerRig;
    WallAbility wa;

    float jumpForce = 0;
    float maxJumpForce = 1000;
    Vector3 jumpAngle = new Vector3(1,1,0);

    // Start is called before the first frame update
    void Start()
    {
        playerRig = gameObject.GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        JumpMove();
    }
    void JumpMove()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (jumpForce < maxJumpForce)
            {
                jumpForce+=10;
                gameObject.transform.localScale -= new Vector3(0.05f, 0.05f, 0.05f);
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            playerRig.velocity += jumpAngle * jumpForce;
            gameObject.transform.localScale = new Vector3(10, 10, 5);
            jumpForce = 0;
        }
    }
    void testMove()
    {

        if (Input.GetKey(KeyCode.W))
        {
            playerRig.velocity += Vector3.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            playerRig.velocity += Vector3.down * 10;
        }
        if (Input.GetKey(KeyCode.A))
        {
            playerRig.velocity += Vector3.left * 10;
        }
        if (Input.GetKey(KeyCode.D))
        {
            playerRig.velocity += Vector3.right * 10;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "wall")
        {
            Debug.Log("触れた");
            wa = col.gameObject.GetComponent<WallAbility>();
            switch (wa.abilityNumber)
            {
                
                case 0://着地
                    playerRig.velocity = Vector3.zero;
                    break;
                    
                case 1://反射
                    //進行方向
                    float playerVec = playerRig.velocity.x*playerRig.velocity.y;
                    //上にいるとき
                    if (wa.Height(true).y < gameObject.transform.position.y)
                    {
                        if (wa.Width(true).x <gameObject.transform.position.x)//右上
                        {
                            playerRig.velocity = new Vector3(-playerRig.velocity.x, -playerRig.velocity.y, 0);
                        }
                        else if(wa.Width(false).x<gameObject.transform.position.x)//真ん中
                        {
                            playerRig.velocity = new Vector3(playerRig.velocity.x, -playerRig.velocity.y, 0);
                        }
                        else//左上
                        {
                            playerRig.velocity = new Vector3(-playerRig.velocity.x, -playerRig.velocity.y, 0);
                        }
                    }
                    //真ん中
                    else if(wa.Height(false).y<gameObject.transform.position.y)
                    {
                        playerRig.velocity = new Vector3(-playerRig.velocity.x, playerRig.velocity.y, 0);
                    }
                    //下にいるとき
                    else
                    {
                        if (wa.Width(true).x < gameObject.transform.position.x)//右下
                        {
                            playerRig.velocity = new Vector3(-playerRig.velocity.x, -playerRig.velocity.y, 0);
                        }
                        else if (wa.Width(false).x < gameObject.transform.position.x)//真ん中
                        {
                            playerRig.velocity = new Vector3(playerRig.velocity.x, -playerRig.velocity.y, 0);
                        }
                        else//左下
                        {
                            playerRig.velocity = new Vector3(-playerRig.velocity.x, -playerRig.velocity.y, 0);
                        }
                    }
                    break;
                case 2://沼の床
                    playerRig.velocity = Vector3.zero;
                    maxJumpForce = maxJumpForce / 2;
                    //ジャンプしたらmaxJumpForceをもとに戻す；

                    break;
                default:
                    break;
            }
        }
    }
 }

