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
    bool jump;

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
        if(!jump)
        {
            if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
            {
                if (jumpForce < maxJumpForce)
                {
                    jumpForce += 10;
                    gameObject.transform.localScale -= new Vector3(0.05f, 0.05f, 0.05f);
                }
            }
            //ボタンの動き
            if (Input.GetKeyUp(KeyCode.Space))
            {
                playerRig.velocity += jumpAngle * jumpForce;
                gameObject.transform.localScale = Scale;
                jumpForce = 0;
                jump = true;
            }
            //マウスでの動き
            if (Input.GetMouseButtonUp(0))
            {
                //マウスの位置取得
                Vector3 mousePos = Input.mousePosition;
                //メインカメラのZを０にしたい
                mousePos.z = 311;
                //マウスの位置をワールド座標に変換
                Vector3 objPos = Camera.main.ScreenToWorldPoint(mousePos) - gameObject.transform.position;
                Vector3 a = objPos.normalized;
                playerRig.velocity = a * jumpForce;
                gameObject.transform.localScale = Scale;
                jumpForce = 0;
                jump = true;
            }
        }
        
    }
    void testMove()
    {
        
        if (Input.GetKey(KeyCode.W))
        {
            playerRig.velocity += Vector3.up*10;
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
                    //めり込んだ時の処理を書く
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
                    jump = false;
                    //ジャンプしたらmaxJumpForceをもとに戻す；
                    break;
                case 3://反射
                    ReflectAction(col.gameObject);
                    break;
                case 4://滑る床
                    SripAction(col.gameObject);
                    jump = false;
                    break;
                case 5://とげ

                    break;
                case 6://斜めの反射
                    SkewRefrect(col.gameObject);
                    break;
                default:
                    break;
            }
        }
    }
    //平面反射
    private void ReflectAction(GameObject col)
    {
        //Z回転軸取得
        float a = col.gameObject.transform.localEulerAngles.z;
        if (wa.Height(true).y - (Scale.y / 2) >= gameObject.transform.position.y - (Scale.y / 2))
        {
            if (wa.Height(false).y + (Scale.y / 2) <= gameObject.transform.position.y + (Scale.y / 2))
            {
                if(gameObject.transform.position.x>col.gameObject.transform.position.x)
                {
                    Debug.Log("right");
                    gameObject.transform.Rotate(Vector3.forward * (-90-a));
                }
                else
                {
                    Debug.Log("left");
                    gameObject.transform.Rotate(Vector3.forward * (90 - a));
                }
            }
            else
            {
                Debug.Log("Down");
                gameObject.transform.Rotate(Vector3.forward * (-180 - a));
            }
        }
        else
        {
            if (wa.Width(true).x - (Scale.x / 2) <= gameObject.transform.position.x - (Scale.x / 2) || wa.Width(false).x + (Scale.x / 2) >= gameObject.transform.position.x + (Scale.x / 2))
            {
                if (gameObject.transform.position.x > col.gameObject.transform.position.x)
                {
                    Debug.Log("right");
                    gameObject.transform.Rotate(Vector3.forward * (-90 - a));
                }
                else
                {
                    Debug.Log("left");
                    gameObject.transform.Rotate(Vector3.forward * (90 - a));
                }
            }
            else
            {
                Debug.Log("UP");
                gameObject.transform.Rotate(Vector3.forward * (-a));
            }
        }
        //当たったオブジェの向き取得
        Vector3 n = gameObject.transform.up;
        //内積
        float h = Mathf.Abs(Vector3.Dot(playerRig.velocity, n));
        //反射ベクトル
        Vector3 r = playerRig.velocity + 2 * n * h;
        //代入
        playerRig.velocity = r;
        //
        Debug.Log("平面呼んだ");
        gameObject.transform.localRotation=Quaternion.Euler(0,0,0);
    }
    //斜め４５度の反射
    private void SkewRefrect(GameObject col)
    {
        //Z回転軸取得
        float a = col.gameObject.transform.localEulerAngles.z;
        gameObject.transform.Rotate(Vector3.forward * (45+a));
        //当たったオブジェの向き取得
        Vector3 n = gameObject.transform.up;
        //内積
        float h = Mathf.Abs(Vector3.Dot(playerRig.velocity, n));
        //反射ベクトル
        Vector3 r = playerRig.velocity + 2 * n * h;
        //代入
        playerRig.velocity = r;
        //
        Debug.Log("斜め呼んだ");
        gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
    //滑る床
    private void SripAction(GameObject col)
    {
        if (wa.Height(true).y - (Scale.y / 2) >= gameObject.transform.position.y - (Scale.y / 2))
        {
            if (wa.Height(false).y + (Scale.y / 2) <= gameObject.transform.position.y + (Scale.y / 2))
            {
                if (gameObject.transform.position.x > col.gameObject.transform.position.x)
                {
                    Debug.Log("right");
                    playerRig.velocity = new Vector3(0, playerRig.velocity.y, 0);
                }
                else
                {
                    Debug.Log("left");
                    playerRig.velocity = new Vector3(0, playerRig.velocity.y, 0);
                }
            }
            else
            {
                Debug.Log("Down");
                playerRig.velocity = new Vector3(playerRig.velocity.x,0, 0);
            }
        }
        else
        {
            if (wa.Width(true).x - (Scale.x / 2) <= gameObject.transform.position.x - (Scale.x / 2) || wa.Width(false).x + (Scale.x / 2) >= gameObject.transform.position.x + (Scale.x / 2))
            {
                if (gameObject.transform.position.x > col.gameObject.transform.position.x)
                {
                    Debug.Log("right");
                    playerRig.velocity = new Vector3(0, playerRig.velocity.y, 0);
                }
                else
                {
                    Debug.Log("left");
                    playerRig.velocity = new Vector3(0, playerRig.velocity.y, 0);
                }
            }
            else
            {
                Debug.Log("UP");
                playerRig.velocity = new Vector3(playerRig.velocity.x,0, 0);
            }
        }
    }

 }

