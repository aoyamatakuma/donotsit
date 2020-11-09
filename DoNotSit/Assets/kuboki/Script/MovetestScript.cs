using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum PlayerSkill
{
    Normal,
    Grand,
    Water,
    Window
}
public enum PlayerMode
{
    Default,
    Skill
}
public enum Player
{
    Ground,
    Jump
}
public class MovetestScript : MonoBehaviour
{
    //コンポーネント取得
    Rigidbody playerRig;
    WallAbility wa;

    
    float jumpForce = 700;//ジャンプの力
    float maxJumpForce = 1000;//ジャンプの最大
    Vector3 playerVec;//プレイヤーの飛んだベクトルを保存
    Vector3 oldPlayerPos;//飛んだ場所を保存
    Vector3 ClickPos;//クリックしたポジション保存用
    Vector3 Scale;//プレイヤーの大きさ格納
    public bool jump;//飛んでいるか否か

    //HP
    public float HP = 100;
    float maxHP = 100;
    //力
    public float ForcePower = 100;
    float maxForcePower = 100;


    //当たったオブジェクトを取得（複数取らないよう）
    public GameObject ColObject;

    public PlayerMode playerMode;
    public PlayerSkill playerSkill;
    public Player isPlayer;

    bool reflect = false;



    // Start is called before the first frame update
    void Start()
    {
        playerMode = PlayerMode.Default;
        playerSkill = PlayerSkill.Normal;
        isPlayer = Player.Ground;
        playerRig = gameObject.GetComponent<Rigidbody>();
        Scale = gameObject.transform.localScale;

    }
    // Update is called once per frame
    void Update()
    {
        JumpMove();
        if(ForcePower<maxForcePower)
        {
            ForcePower += Time.deltaTime*3;
        }
    }
    void JumpMove()
    {
        if(!jump)
        {
            //左クリック
            if (Input.GetMouseButtonUp(0))
            {
                //マウスの位置取得
                Vector3 mousePos = Input.mousePosition;
                //メインカメラのZを０にしたい
                mousePos.z = 311;
                //マウスの位置をワールド座標に変換した後に向きを取得
                Vector3 m = Camera.main.ScreenToWorldPoint(mousePos) - gameObject.transform.position;
                //正規化
                Vector3 a = m.normalized;
                //クリックした方向に力を加える
                playerRig.velocity = a * jumpForce;
                playerMode = PlayerMode.Default;

                //値の保存
                oldPlayerPos = gameObject.transform.position;//飛んだ位置を保存
                ClickPos = Camera.main.ScreenToWorldPoint(mousePos);//クリックした場所を保存
                playerVec = a * jumpForce;//飛ぶベクトルを保存
                jump = true;
            }
            //右クリック
            if (Input.GetMouseButtonUp(1))
            {
                //スキル使用状態

                if (playerSkill == PlayerSkill.Normal)
                {
                    if (ForcePower > 10)
                    {
                        //マウスの位置取得
                        Vector3 mousePos = Input.mousePosition;
                        //メインカメラのZを０にしたい
                        mousePos.z = 311;
                        //マウスの位置をワールド座標に変換
                        Vector3 m = Camera.main.ScreenToWorldPoint(mousePos) - gameObject.transform.position;
                        //正規化
                        Vector3 a = m.normalized;
                        //クリックした方向に力を加える
                        playerRig.velocity = a * jumpForce;
                        //
                        playerMode = PlayerMode.Skill;
                        ForcePower -= 10;
                        //値の保存
                        oldPlayerPos = gameObject.transform.position;//飛んだ位置を保存
                        ClickPos = Camera.main.ScreenToWorldPoint(mousePos);//クリックした場所を保存
                        playerVec = a * jumpForce;//飛ぶベクトルを保存
                        jump = true;
                        Debug.Log("→押した");
                    }

                }
            }
        }
       
            

    }
    void testMove()
    {

        if (Input.GetKey(KeyCode.W))
        {
            playerRig.velocity += Vector3.up * 10;
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

    #region 当たり判定

    private void OnCollisionEnter(Collision col)
    {
        Vector3 hitpoint;
        foreach(ContactPoint point in col.contacts)
        {
            hitpoint = point.point;
            Vector3 hei = col.gameObject.transform.position - hitpoint;
            Debug.Log(hitpoint);
            Debug.Log(gameObject.transform.position);
        }
        //はじめに触れたオブジェクトをとる
        if(ColObject == null)
        {
            Debug.Log("入れた");
            ColObject = col.gameObject;
            wa = col.gameObject.GetComponent<WallAbility>();
        }
        if (ColObject.tag == "wall")
        {
            if(jump)
            {
                if (playerMode == PlayerMode.Skill && playerSkill == PlayerSkill.Normal)
                {
                    if (!reflect)
                    {
                        ReflectAction(ColObject);
                        reflect = true;
                        ColObject = null;
                    }
                    else
                    {
                        Debug.Log("とまるよ");
                        playerRig.velocity = Vector3.zero;
                        jump = false;
                        isPlayer = Player.Ground;
                        playerMode = PlayerMode.Default;
                        reflect = false;
                    }
                }
                else
                {
                    Debug.Log("とまるよ");
                    playerRig.velocity = Vector3.zero;
                    jump = false;
                    isPlayer = Player.Ground;
                }
            }
           
        }
    }
    private void OnCollisionExit(Collision col)
    {
        if(ColObject == col.gameObject)
        {
            ColObject = null;
            isPlayer = Player.Jump;
        }
    }


    void StopPos()
    {
        if (isPlayer == Player.Ground)
        {
            Debug.Log("地面にいる");
            if (jump)
            {
                Debug.Log("ジャッジ");
                //クリックした位置と現在の場所を確認
                if (ClickPos.x >= gameObject.transform.position.x)
                {
                    if (ClickPos.x <= oldPlayerPos.x)
                    {
                        Debug.Log("止めるよん");
                        playerRig.velocity = Vector3.zero;
                    }
                }
                else if (ClickPos.x <= gameObject.transform.position.x)
                {
                    if (ClickPos.x >= oldPlayerPos.x)
                    {
                        Debug.Log("止めるよね");
                        playerRig.velocity = Vector3.zero;

                    }
                }
                else if (ClickPos.y >= gameObject.transform.position.y)
                {
                    if (ClickPos.y <= oldPlayerPos.y)
                    {
                        Debug.Log("止めるよな");
                        playerRig.velocity = Vector3.zero;

                    }
                }
                else if (ClickPos.y <= gameObject.transform.position.y)
                {
                    if (ClickPos.y >= oldPlayerPos.y)
                    {
                        Debug.Log("止めるよの");
                        playerRig.velocity = Vector3.zero;
                    }
                }

            }
        }
    }
    #endregion
    //平面反射
    private void ReflectAction(GameObject col)
    {      
        if(!reflect)
        {
            //どこにあたったかで角度を決める
            if (wa.Height(true).y - (Scale.y / 2) >= gameObject.transform.position.y - (Scale.y / 2))
            {
                if (wa.Height(false).y + (Scale.y / 2) <= gameObject.transform.position.y + (Scale.y / 2))
                {
                    if (gameObject.transform.position.x > col.gameObject.transform.position.x)
                    {
                        Debug.Log("right");
                        gameObject.transform.Rotate(Vector3.forward * -90);
                    }
                    else
                    {
                        Debug.Log("left");
                        gameObject.transform.Rotate(Vector3.forward * 90);
                    }
                }
                else
                {
                    Debug.Log("Down");
                    gameObject.transform.Rotate(Vector3.forward * 180);
                }
            }
            else
            {
                if (wa.Width(true).x - (Scale.x / 2) <= gameObject.transform.position.x - (Scale.x / 2) || wa.Width(false).x + (Scale.x / 2) >= gameObject.transform.position.x + (Scale.x / 2))
                {
                    if (gameObject.transform.position.x > col.gameObject.transform.position.x)
                    {
                        Debug.Log("right");
                        gameObject.transform.Rotate(Vector3.forward * -90);
                    }
                    else
                    {
                        Debug.Log("left");
                        gameObject.transform.Rotate(Vector3.forward * 90);
                    }
                }
                else
                {
                    Debug.Log("UP");
                    gameObject.transform.Rotate(Vector3.forward * 0);
                }
            }
            //当たったオブジェの向き取得
            Vector3 n = gameObject.transform.up;
            //内積
            float h = Mathf.Abs(Vector3.Dot(playerVec, n));
            //反射ベクトル
            Vector3 r = playerVec + 2 * n * h;
            //代入
            playerRig.velocity = r;
            //回転を戻す
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            //飛ぶベクトルを保存
            playerVec = r;

        }        
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

