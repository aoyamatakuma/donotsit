using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;


public enum PlayerState
{
    Normal,
    Attack,
    Stop
}
public class PlayerControl : MonoBehaviour
{
    Rigidbody playerRig;
    public bool jumpFlag;//ジャンプフラグ
    public bool restratFlag;//障害物の当たり判定のフラグ
    public bool rayFlag;//壁云々
    public bool matFlag;//沼フラグ
    public bool revFlag;//スティック反転フラグ
    public float jumpSpeed = 20.0f;//ジャンプの力
    public float jumpSpeedUp = 1.2f;//ジャンプアップの力
    public float jumpDefalut;//※ジャンプデフォルト
    public float MaxjumpSpeed;//ジャンプ最大値
    public float maxAngle = 44.0f; // 最大回転角度
    public float minAngle = -44.0f; // 最小回転角度
    public float roateSpeed = 1.0f;//回るスピード
    public float angleZ;//こいつ大事回転制御
    public static float timer;//タイマー
    public float starttimer = 60f;
    //  public float comboTimer = 0f;//コンボタイマー
    public float combo;//コンボ
    public float rayline;//レイ長さ
    public int level = 1;//レベル
    public static int scoreNumber;
                         //  public int speedCount;//連続用
    public int exp;//経験値
    public int hp;
    public Text comboText;
    public Text levelText;//レベルテキスト
    public Text expText;
    public Text hpText;
    public Text scoreText;
    public GameObject ob;//矢印
    public Transform basePosition;//支点
    public PlayerState currentPlayerState; //現在の状態
    Vector3 playerVec;
    WallAbility wa;
    Vector3 Scale;
    public GameObject jumpEffect;
    AudioSource audio;
    public AudioClip jumpSE;
    public GameObject effectPos;
    private float maxAngleSet;
    private float minAngleSet;

    //追加
    Vector3 hitPoint;
    Vector3 playerRot = Vector3.zero;
    GameObject hitObject;
    int wallNum;
    bool colFlag;
    Fade fade;

    //
    public float angleSpeed = 1;
    public float angleLimit=40;
    float playerAngle = 0;
    public bool select = false;
    GameObject moveWall;
    public GameObject carsol;

    public int ReflectCount;
    private int refCount = 0;
   

    // Start is called before the first frame update
    void Start()
    {
        playerRig = GetComponent<Rigidbody>();
        //フラグ達
        select = false;
        jumpFlag = false;//ジャンプ
        rayFlag = false;//レイ
        restratFlag = false;//トゲ
        revFlag = false;//スティック
        MaxjumpSpeed = jumpDefalut * 2;
        currentPlayerState = PlayerState.Normal;
        Scale = gameObject.transform.lossyScale;
        timer = starttimer;
        scoreNumber = 0;
        fade = GetComponent<Fade>();
        audio = GetComponent<AudioSource>();            
        SetAngle();
        refCount = ReflectCount;
        
    }

    // Update is called once per frame
    void Update()
    {

        
        //ノーマルステート
        if (currentPlayerState == PlayerState.Normal)//ノーマル
        {
            Invoke("Move", 0.0001f);//プレイに支障はないはず
            Jump();
            RayObject();
            PlayerWallMove();
            playerRig.velocity = Vector3.zero;
        }
        if (currentPlayerState == PlayerState.Attack)//アタック中
        {
            ob.SetActive(false);
        }
        if (currentPlayerState == PlayerState.Stop)//ストップ中
        {
            ob.SetActive(false);
            playerRig.velocity = Vector3.zero;
        }
        //タイマー
        timer += 1.0f * Time.deltaTime;
        comboText.text = combo.ToString();//コンボ
        hpText.text = "HP:" + hp.ToString();
        scoreText.text =  scoreNumber.ToString();
        //if (SceneManager.GetActiveScene().name == "Stage2")
        //{ 
        //levelText.text = "Level:" + level.ToString();
        //expText.text = "Exp:" + exp.ToString();
        //}
        Combo();//コンボ関連
        ReverseMove();//反転スティック

        if (hp <= 0)
        {
            fade.StartFadeIn("GameOver", false);
        }
        //最大スピード
        if (jumpSpeed >= MaxjumpSpeed)
        {
            jumpSpeed = MaxjumpSpeed;
        }

        if (Input.GetButtonDown("Select"))
        {
            if (select)
            {
                select = false;
            }
            else
            {
                select = true;
            }
        }
        Carsolmove();
    }
    void Move()//移動系
    {
        //左スティック
        float turn = Input.GetAxis("Horizontal");
        float up = Input.GetAxis("Vertical");
        Vector3 Rot = transform.localEulerAngles;
        if(!select)
        {
            switch (playerAngle)
            {
                case 0:
                    if (Rot.z <= maxAngleSet || Rot.z >= minAngleSet)
                    {
                        if (turn > 0)
                        { transform.Rotate(Vector3.back * angleSpeed); }
                        else if (turn < 0)
                        { transform.Rotate(Vector3.forward * angleSpeed); }
                    }
                    else
                    {
                        if (maxAngleSet <= Rot.z && 90 >= Rot.z)
                        {
                            transform.localEulerAngles = new Vector3(0, 0, maxAngleSet - 1);
                        }
                        if (Rot.z <= minAngleSet && 270 <= Rot.z)
                        {
                            transform.localEulerAngles = new Vector3(0, 0, minAngleSet + 1);
                        }
                    }
                    break;
                case 90:
                    if (Rot.z <= maxAngleSet && Rot.z >= minAngleSet)
                    {
                        if (up > 0)
                        { transform.Rotate(Vector3.back * angleSpeed); }
                        else if (up < 0)
                        { transform.Rotate(Vector3.forward * angleSpeed); }
                    }
                    else
                    {
                        if (maxAngleSet <= Rot.z)
                        {
                            transform.localEulerAngles = new Vector3(0, 0, maxAngleSet - 1);
                        }
                        if (minAngleSet >= Rot.z)
                        {
                            transform.localEulerAngles = new Vector3(0, 0, minAngleSet + 1);
                        }
                    }
                    break;
                case 180:
                    if (Rot.z <= maxAngleSet && Rot.z >= minAngleSet)
                    {
                        if (turn > 0)
                        { transform.Rotate(Vector3.forward * angleSpeed); }
                        else if (turn < 0)
                        { transform.Rotate(Vector3.back * angleSpeed); }
                    }
                    else
                    {
                        if (maxAngleSet <= Rot.z)
                        {
                            transform.localEulerAngles = new Vector3(0, 0, maxAngleSet - 1);
                        }
                        if (minAngleSet >= Rot.z)
                        {
                            transform.localEulerAngles = new Vector3(0, 0, minAngleSet + 1);
                        }
                    }
                    break;
                case 270:
                    if (Rot.z <= maxAngleSet && Rot.z >= minAngleSet)
                    {
                        if (up > 0)
                        { transform.Rotate(Vector3.forward * angleSpeed); }
                        else if (up < 0)
                        { transform.Rotate(Vector3.back * angleSpeed); }
                    }
                    else
                    {
                        if (maxAngleSet < Rot.z)
                        {
                            transform.localEulerAngles = new Vector3(0, 0, maxAngleSet - 1);
                        }
                        if (minAngleSet > Rot.z)
                        {
                            transform.localEulerAngles = new Vector3(0, 0, minAngleSet + 1);
                        }
                    }
                    break;
            }
        }
        
    }

    void SetAngle()
    {
        playerAngle = playerRot.z;
        if (playerAngle == 0)
        {
            maxAngleSet = angleLimit;
            minAngleSet = 360 - angleLimit;
        }
        else
        {
            maxAngleSet = playerAngle + angleLimit;
            minAngleSet = playerAngle - angleLimit;
        }
    }
    void Jump()//ジャンプ系
    {
        if (Input.GetButtonUp("Jump") && jumpFlag == false && rayFlag == true)
        {
            jumpFlag = true;
            audio.PlayOneShot(jumpSE);
            Instantiate(jumpEffect, effectPos.transform.position, transform.rotation);
            revFlag = false;
            angleZ = 0;
            currentPlayerState = PlayerState.Attack;
            playerRig.AddForce(transform.up * jumpSpeed, ForceMode.Impulse);//ジャンプする
            playerVec = transform.up * jumpSpeed;
            moveWall = null;
            gameObject.transform.localScale = Scale;
            //carsol.SetActive(false);
        }
    }
    void ReverseMove()
    {
        if (this.gameObject.transform.localRotation == Quaternion.Euler(0, 0, 180))
        {
            revFlag = true;
        }
        if (this.gameObject.transform.localRotation == Quaternion.Euler(0, 0, 0))
        {
            revFlag = false;
        }
    }
    public static float TimeScore()
    {
        return timer;
    }
    public static int ClearScore()
    {
        return scoreNumber;
    }
    //壁があるない
    void RayObject()
    {
        Ray ray = new Ray(transform.position, transform.up);
        RaycastHit hit;
        float a = 1000;
        for (int i = 0; i < 3; i++)
        {
            ray = new Ray(transform.position +Vector3.right*(i - 1)*1.6f, transform.up);
            //レイの判定(飛ばすレイ、レイが当たったものの情報、レイの長さ)
            if (Physics.Raycast(ray, out hit, rayline)) //壁がある時
            {
                Physics.queriesHitTriggers = false;//こいつでトリガーのやつは無視するぽい
                if (hit.collider.tag == "Wall")
                {
                    rayFlag = true;
                    ob.SetActive(true);
                    //令の長さを取得
                    float dis = Vector3.Distance(hit.point, transform.position + Vector3.right * (i - 1));
                    if (a > dis)
                    {
                        a = dis;
                        //ヒットしてる位置を取得
                        hitPoint = hit.point;
                        //オブジェクトを取得
                        hitObject = hit.collider.gameObject;
                        vecTest();
                    }
                    Debug.DrawRay(transform.position +(transform.up+ Vector3.right) * 1.6f * (i - 1), transform.up * rayline, Color.red, 0, true);                   
                }
            }
            else //壁がない時
            {
                rayFlag = false;
                ob.SetActive(false);
            }
        }        
        test();
    }

    //壁との当たり判定
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Wall") && currentPlayerState == PlayerState.Attack)
        {
            if (!colFlag)
            {
                jumpFlag = false;
                playerRig.velocity = Vector3.zero;
                combo = 0;
                switch (wallNum)
                {
                    case 0://着地
                        gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
                        gameObject.transform.Rotate(playerRot);
                        SetAngle();
                        currentPlayerState = PlayerState.Normal;
                        //NormalBlock(col.gameObject);
                        break;
                    case 1://沼の床
                        gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
                        gameObject.transform.Rotate(playerRot);
                        jumpSpeed = jumpDefalut;
                        SetAngle();
                        currentPlayerState = PlayerState.Normal;
                        // NormalBlock(col.gameObject);
                        break;
                    case 2://反射
                        ReflectAction();
                        break;
                    case 6://斜め着地左
                        currentPlayerState = PlayerState.Normal;
                        SkewBlockLeft(col.gameObject);
                        break;
                    case 7://斜め着地右
                        currentPlayerState = PlayerState.Normal;
                        SkewBlockRight(col.gameObject);
                        break;
                    case 8://動く床
                        moveWall = col.gameObject;
                        Debug.Log(moveWall);
                        coltest();
                        gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
                        gameObject.transform.Rotate(playerRot);
                        SetAngle();
                        currentPlayerState = PlayerState.Normal;
                        break;
                    case 10:
                        ReflectActionCount();
                        break;
                    default:
                        break;
                }
                colFlag = true;
            }
        }
        //EnemyCollision場合
        //アタック(移動中に当たると)タイマー増加
        if (col.gameObject.CompareTag("Enemy") && currentPlayerState == PlayerState.Attack)
        {
            combo++;//コンボ増加
                    // timer += combo;//コンボ時間に反映
                    //  Destroy(col.gameObject);
        }
        if (col.gameObject.CompareTag("Wall"))
        {
            wa = col.gameObject.GetComponent<WallAbility>();
            switch (wa.abilityNumber)
            {
                case 9://デスエリア
                    fade.StartFadeIn("GameOver", false);
                    break;
                default:
                    break;
            }
        }
        if (col.gameObject.CompareTag("ChaseEnemy"))
        {
          fade.StartFadeIn("GameOver", false);
        }
    }
    void OnCollisionExit(Collision col)
    {
        
        if (colFlag)
        {
            colFlag = false;
        }
    }

    //リスタート用
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Wall") && currentPlayerState == PlayerState.Attack)
        {
            wa = col.gameObject.GetComponent<WallAbility>();
            switch (wa.abilityNumber)
            {
                case 3://滑る床
                    SripAction(col.gameObject);
                    break;
                case 4://とげ
                    restratFlag = true;
                    StartCoroutine("ThornTime");
                    break;
                case 5://斜めの反射
                    SkewRefrect(col.gameObject);
                    break;
                default:
                    break;
            }
        }
        //EnemyTrigger場合
        //アタック(移動中に当たると)タイマー増加
        if (col.gameObject.CompareTag("Enemy") && currentPlayerState == PlayerState.Attack)
        {
            combo++;
            //コンボ増加        
            //timer += combo;//コンボ時間に反映
            //④パターン
            //if (SceneManager.GetActiveScene().name == "Stage2")
            //{
            //    LevelUp();
            //}
        }
    }
    //垂直くっつく
    private void NormalBlock(GameObject col)
    {
        //Z回転軸取得
        float a = col.gameObject.transform.localEulerAngles.z;
        gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
        gameObject.transform.Rotate(Vector3.forward + new Vector3(0, 0, a));
    }
    //斜め左
    private void SkewBlockLeft(GameObject col)
    {
        //Z回転軸取得
        float a = col.gameObject.transform.localEulerAngles.z;
        gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
        gameObject.transform.Rotate(Vector3.forward * (45 - a));
    }
    //斜め右
    private void SkewBlockRight(GameObject col)
    {
        //Z回転軸取得
        float a = col.gameObject.transform.localEulerAngles.z;
        gameObject.transform.localRotation = Quaternion.Euler(0, 0, -90);
        gameObject.transform.Rotate(Vector3.forward * (-45 - a));
    }
    public void Damage(int damage)
    {
        hp -= damage;
    }
    public void Score(int score)
    {
        scoreNumber += score;
    }
    IEnumerator ThornTime()
    {
        yield return new WaitForSeconds(0.1f);
        if (restratFlag == true)
        {
           hp--;
        }
        restratFlag = false;
        currentPlayerState = PlayerState.Stop;
        yield return new WaitForSeconds(2.0f);
        currentPlayerState = PlayerState.Normal;
        yield break;
    }
    //コンボ系
    public void Combo()
    {
        if (combo >= 1)
        {
            comboText.enabled = true;
        }
        else
        {
            comboText.enabled = false;
        }
    }
    //レベルアップパターン　②
    public void LevelUp()
    {
        //経験値UP
        exp++;
        // レベル系
        if (level == 1 && exp >= 5)//レベル2
        {
            level += 1;
            jumpSpeed *= 1.2f;
        }
        if (level == 2 && exp >= 10)//レベル3
        {
            level += 1;
            jumpSpeed = jumpDefalut;
            if (jumpSpeed == jumpDefalut)
            {
                jumpSpeed *= 1.4f;
            }
        }
        if (level == 3 && exp >= 15)//レベル4
        {
            level += 1;
            jumpSpeed = jumpDefalut;
            if (jumpSpeed == jumpDefalut)
            {
                jumpSpeed *= 1.6f;
            }
        }
        if (level == 4 && exp >= 20)//レベル5
        {
            level += 1;
            jumpSpeed = jumpDefalut;
            if (jumpSpeed == jumpDefalut)
            {
                jumpSpeed *= 1.8f;
            }
        }
        if (level == 5 && exp >= 25)//レベル6
        {
            level += 1;
            jumpSpeed = jumpDefalut;
            if (jumpSpeed == jumpDefalut)
            {
                jumpSpeed *= 2.0f;
            }
        }
    }
    //
    private void ReflectAction()
    {
        gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
        gameObject.transform.Rotate(playerRot);
        //当たったオブジェの向き取得
        Vector3 n = gameObject.transform.up;
        //内積
        float h = Mathf.Abs(Vector3.Dot(playerVec, n));
        //反射ベクトル
        Vector3 r = playerVec + 2 * n * h;
        //代入
        playerRig.velocity = r;
        playerVec = playerRig.velocity;
        gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
        transform.rotation = Quaternion.FromToRotation(gameObject.transform.up, r);
        RayObject();
    }
    private void ReflectActionCount()
    {
        if(refCount<1)
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            gameObject.transform.Rotate(playerRot);
            SetAngle();
            currentPlayerState = PlayerState.Normal;
            refCount = ReflectCount;
        }
        else
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            gameObject.transform.Rotate(playerRot);
            //当たったオブジェの向き取得
            Vector3 n = gameObject.transform.up;
            //内積
            float h = Mathf.Abs(Vector3.Dot(playerVec, n));
            //反射ベクトル
            Vector3 r = playerVec + 2 * n * h;
            //代入
            playerRig.velocity = r;
            playerVec = playerRig.velocity;
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            transform.rotation = Quaternion.FromToRotation(gameObject.transform.up, r);
            RayObject();
            refCount--;
        }
    }
    private void SkewRefrect(GameObject col)
    {
        //Z回転軸取得
        float a = col.gameObject.transform.localEulerAngles.z;
        gameObject.transform.Rotate(Vector3.forward * (45 + a));
        //当たったオブジェの向き取得
        Vector3 n = gameObject.transform.up;
        //内積
        float h = Mathf.Abs(Vector3.Dot(playerVec, n));
        //反射ベクトル
        Vector3 r = playerVec + 2 * n * h;
        //代入
        playerRig.velocity = r;
        //
        //    Debug.Log("斜め呼んだ");
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
                    // Debug.Log("right");
                    playerRig.velocity = new Vector3(0, playerRig.velocity.y, 0);
                }
                else
                {
                    // Debug.Log("left");
                    playerRig.velocity = new Vector3(0, playerRig.velocity.y, 0);
                }
            }
            else
            {
                // Debug.Log("Down");
                playerRig.velocity = new Vector3(playerRig.velocity.x, 0, 0);
            }
        }
        else
        {
            if (wa.Width(true).x - (Scale.x / 2) <= gameObject.transform.position.x - (Scale.x / 2) || wa.Width(false).x + (Scale.x / 2) >= gameObject.transform.position.x + (Scale.x / 2))
            {
                if (gameObject.transform.position.x > col.gameObject.transform.position.x)
                {
                    // Debug.Log("right");
                    playerRig.velocity = new Vector3(0, playerRig.velocity.y, 0);
                }
                else
                {
                    //  Debug.Log("left");
                    playerRig.velocity = new Vector3(0, playerRig.velocity.y, 0);
                }
            }
            else
            {
                //  Debug.Log("UP");
                playerRig.velocity = new Vector3(playerRig.velocity.x, 0, 0);
            }
        }
    }
    //
    private void test()
    {
        //コンポーネント取得
        wa = hitObject.GetComponent<WallAbility>();
        //どの位置にあたったか判定し回転する
        if (wa.Height(true).y > hitPoint.y)
        {
            if (hitPoint.x > wa.Width(true).x)
            {
                playerRot = Vector3.forward * 270;
            }
            else if (hitPoint.x < wa.Width(false).x)
            {
                playerRot = Vector3.forward * 90;
            }
            else
            {
                playerRot = Vector3.forward * 180;
            }

            //if ((int)wa.Height(false).y<(int)hitPoint.y)
            //{
               
            //}
            //else
            //{
            //    playerRot = Vector3.forward * 180;
            //}
        }
        else
        {
            if (hitPoint.x > wa.Width(true).x)
            {
                playerRot = Vector3.forward * 270;
            }
            else if (hitPoint.x < wa.Width(false).x)
            {
                playerRot = Vector3.forward * 90;
            }
            else
            {
                playerRot = Vector3.forward * 0;
            }
        }
        wallNum = wa.abilityNumber;
    }
    private void coltest()
    {
        //コンポーネント取得
        wa = hitObject.GetComponent<WallAbility>();
        //どの位置にあたったか判定し回転する
        if (wa.Height(true).y > gameObject.transform.position.y)
        {
            if ((int)wa.Height(false).y <= (int)gameObject.transform.position.y)
            {
                if (gameObject.transform.position.x > wa.Width(true).x)
                {
                    playerRot = Vector3.forward * 270;
                }
                else if (gameObject.transform.position.x < wa.Width(false).x)
                {
                    playerRot = Vector3.forward * 90;
                }
                else
                {
                    playerRot = Vector3.forward * 180;
                }
            }
            else
            {
                playerRot = Vector3.forward * 180;
            }
        }
        else
        {
            if (gameObject.transform.position.x > wa.Width(true).x)
            {
                playerRot = Vector3.forward * 270;
            }
            else if (gameObject.transform.position.x < wa.Width(false).x)
            {
                playerRot = Vector3.forward * 90;
            }
            else
            {
                playerRot = Vector3.forward * 0;
            }
        }
        wallNum = wa.abilityNumber;
    }

    private void vecTest()
    {
        wa = hitObject.GetComponent<WallAbility>();
        //どの位置にあたったか判定し回転する
        if (wa.Height(true).y > hitPoint.y)
        {
            if (hitPoint.x > wa.Width(true).x)
            {
                Debug.Log("Right");
            }
            else if (hitPoint.x < wa.Width(false).x)
            {
                Debug.Log("Left");
            }
            else
            {
                Debug.Log("Down");
            }
        }
        else
        {
            if (hitPoint.x > wa.Width(true).x)
            {
                Debug.Log("みぎ");
            }
            else if (hitPoint.x < wa.Width(false).x)
            {
                Debug.Log("ひだり");
            }
            else
            {
                Debug.Log("Up");
            }
        }
    }

    private void PlayerWallMove()
    {
        if(moveWall!=null)
        {
            WallAbility wa = moveWall.GetComponent<WallAbility>();
            if (wa.abilityNumber == 8)
            {
                MoveWall mw = moveWall.GetComponent<MoveWall>();
                mw.Move(gameObject, mw.speed);
            }
            else if (wa.abilityNumber == 9)
            {
                fade.StartFadeIn("GameOver", false);
            }
            else
            { }
        }
    }
    private void Carsolmove()
    {
        carsol.transform.position = hitPoint;
    }
}
