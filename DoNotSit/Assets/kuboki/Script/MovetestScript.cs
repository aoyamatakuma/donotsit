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
    Vector3 Scale;

    // Start is called before the first frame update
    void Start()
    {
        playerRig = gameObject.GetComponent<Rigidbody>();
        Scale = gameObject.transform.localScale;
    }
    // Update is called once per frame
    void Update()
    {
        JumpMove();
        testMove();
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
            gameObject.transform.localScale = Scale;
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
            wa = col.gameObject.GetComponent<WallAbility>();
            switch (wa.abilityNumber)
            {
                
                case 0://着地
                    playerRig.velocity = Vector3.zero;
                    break;
                    
                case 1://反射
                    //進行方向
                    float playerVec = playerRig.velocity.x*playerRig.velocity.y;

                    //縦の中心
                    if (wa.Height(true).y-(Scale.y/2) >= gameObject.transform.position.y-(Scale.y/2))
                    {
                        if(wa.Height(false).y+(Scale.y/2) <= gameObject.transform.position.y + (Scale.y / 2))
                        {
                            Debug.Log("yoko");
                            playerRig.velocity = new Vector3(-playerRig.velocity.x, playerRig.velocity.y, 0);
                        }
                        else
                        {
                            Debug.Log("tate");
                            playerRig.velocity = new Vector3(playerRig.velocity.x, -playerRig.velocity.y, 0);
                        }
                    }
                    else
                    {                       
                        if(wa.Width(true).x-(Scale.x/2)<=gameObject.transform.position.x-(Scale.x/2)|| wa.Width(false).x + (Scale.x/2) >= gameObject.transform.position.x + (Scale.x / 2))
                        {
                            Debug.Log("横");
                            playerRig.velocity = new Vector3(-playerRig.velocity.x, playerRig.velocity.y, 0);
                        }
                        else
                        {
                            Debug.Log("縦");
                            playerRig.velocity = new Vector3(playerRig.velocity.x, -playerRig.velocity.y, 0);
                        }
                      
                    }
                    
                    break;
                case 2://沼の床
                    playerRig.velocity = Vector3.zero;
                    maxJumpForce = maxJumpForce / 2;
                    //ジャンプしたらmaxJumpForceをもとに戻す；
                    break;
                case 3:
                    ReflectAction(col.gameObject);
                    break;
                default:
                    break;
            }
        }
    }

    private void ReflectAction(GameObject col)
    {
        Vector3 n = col.gameObject.transform.up;
        float h = Mathf.Abs(Vector3.Dot(playerRig.velocity, n));
        Vector3 r = playerRig.velocity + 2 * n * h;
        playerRig.velocity = r;
    }
 }

