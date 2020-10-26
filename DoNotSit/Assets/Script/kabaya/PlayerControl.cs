﻿using System.Collections;
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
                         //  public int speedCount;//連続用
    public int exp;//経験値
    public Text comboText;
    public Text timerText;//タイマーテキスト
    public Text levelText;//レベルテキスト
    public GameObject ob;//矢印
    public Transform basePosition;//支点
    public PlayerState currentPlayerState; //現在の状態
    Vector3 playerVec;
    WallAbility wa;
    Vector3 Scale;

    //追加
    Vector3 hitPoint;
    Vector3 playerRot;
    GameObject hitObject;
    int wallNum;
    bool colFlag;
    // Start is called before the first frame update
    void Start()
    {
        playerRig = GetComponent<Rigidbody>();
        //フラグ達
        jumpFlag = false;//ジャンプ
        rayFlag = false;//レイ
        restratFlag = false;//トゲ
        revFlag = false;//スティック
        MaxjumpSpeed = jumpDefalut * 2;
        currentPlayerState = PlayerState.Normal;
        Scale = gameObject.transform.localScale;
        timer = starttimer;
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
        timer -= 1.0f * Time.deltaTime;
        timerText.text = timer.ToString("f2") + "秒";//制限時間
        comboText.text = combo.ToString();//コンボ
        Combo();//コンボ関連
        ReverseMove();//反転スティック
        //if (SceneManager.GetActiveScene().name == "Stage1")
        //{
        // levelText.text = level.ToString();
        // }
        if (timer <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
        //最大スピード
        if (jumpSpeed >= MaxjumpSpeed)
        {
            jumpSpeed = MaxjumpSpeed;
        }
        //if(jumpSpeed<=MaxjumpSpeed)
        //{
        //    jumpSpeed = MaxjumpSpeed;
        //}
    }
    void Move()//移動系
    {
        //左スティック
        float turn = Input.GetAxis("Horizontal");
        if (revFlag == false)
        {
            if (turn >= 0.5f)
            {
                if (angleZ >= minAngle)
                {
                    angleZ--;
                    transform.RotateAround(basePosition.transform.position, transform.forward, -roateSpeed);
                }
            }
            if (turn <= -0.5f)
            {
                if (angleZ <= maxAngle)
                {
                    angleZ++;
                    transform.RotateAround(basePosition.transform.position, transform.forward, roateSpeed);
                }
            }
        }
        else if (revFlag == true)
        {
            if (turn >= 0.5f)
            {
                if (angleZ <= maxAngle)
                {
                    angleZ++;
                    transform.RotateAround(basePosition.transform.position, transform.forward, roateSpeed);
                }
            }
            if (turn <= -0.5f)
            {
                if (angleZ >= minAngle)
                {
                    angleZ--;
                    transform.RotateAround(basePosition.transform.position, transform.forward, -roateSpeed);
                }
            }
        }
    }
    void Jump()//ジャンプ系
    {
        if (Input.GetButtonUp("Jump") && jumpFlag == false && rayFlag == true)
        {
            jumpFlag = true;
            revFlag = false;
            angleZ = 0;
            currentPlayerState = PlayerState.Attack;
            playerRig.AddForce(transform.up * jumpSpeed, ForceMode.Impulse);//ジャンプする
            playerVec = transform.up * jumpSpeed;
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
    //壁があるない
    void RayObject()
    {
        Ray ray = new Ray(transform.position, transform.up);
        RaycastHit hit;
        //レイの判定(飛ばすレイ、レイが当たったものの情報、レイの長さ)
        if (Physics.Raycast(ray, out hit, rayline)) //壁がある時
        {
            Physics.queriesHitTriggers = false;//こいつでトリガーのやつは無視するぽい
            if (hit.collider.tag == "Wall")
            {
                rayFlag = true;
                ob.SetActive(true);
                //ヒットしてる位置を取得
                hitPoint = hit.point;
                //オブジェクトを取得
                hitObject = hit.collider.gameObject;
                //   Debug.Log(hit.collider.gameObject.name);
                test();
            }
        }
        else //壁がない時
        {
            rayFlag = false;
            ob.SetActive(false);
        }
        Debug.DrawRay(transform.position, transform.up * rayline, Color.red);
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
                        currentPlayerState = PlayerState.Normal;
                        //①パターン
                        //if (SceneManager.GetActiveScene().name == "Stage1")
                        //{
                        //    ReflectionUp();
                        //}
                        //NormalBlock(col.gameObject);
                        break;
                    case 1://沼の床
                        gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
                        gameObject.transform.Rotate(playerRot);
                        jumpSpeed = jumpDefalut;
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
                    //②パターン
                    //if (SceneManager.GetActiveScene().name == "Stage1")
                    //{
                    //    EnemeyUp();//敵を倒すパターン
                    //}
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
                    Debug.Log("togeHit");
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
            //②パターン
            //if (SceneManager.GetActiveScene().name == "Stage1")
            //{
            //    EnemeyUp();//敵を倒すパターン
            //}
            //③パターン
            //if (SceneManager.GetActiveScene().name == "Stage1")
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
    public void Damage(float damage)
    {
        timer -= damage;
    }
    IEnumerator ThornTime()
    {
        yield return new WaitForSeconds(0.1f);
        if (restratFlag == true)
        {
            timer -= 5.0f;
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
    //反射パターン ①
    public void ReflectionUp()
    {
        jumpSpeed *= jumpSpeedUp;
    }
    //敵を倒すパターン　②
    public void EnemeyUp()
    {
        jumpSpeed *= jumpSpeedUp;
    }
    //レベルアップパターン　③
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
        if (level == 2 & exp >= 10)//レベル3
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
        if (level == 4 & exp >= 20)//レベル5
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
                playerRig.velocity = new Vector3(playerRig.velocity.x, 0, 0);
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
            if (wa.Height(false).y < hitPoint.y)
            {
                if (hitPoint.x > hitObject.transform.position.x)
                {
                    Debug.Log("right");
                    playerRot = Vector3.forward * -90;
                }
                else
                {
                    Debug.Log("left");
                    playerRot = Vector3.forward * 90;
                }
            }
            else
            {
                Debug.Log("Down");
                playerRot = Vector3.forward * 180;
            }
        }
        else
        {
            if (wa.Width(true).x < hitPoint.x || wa.Width(false).x > hitPoint.x)
            {
                if (hitPoint.x > hitObject.transform.position.x)
                {
                    Debug.Log("right");
                    playerRot = Vector3.forward * -90;
                }
                else
                {
                    Debug.Log("left");
                    playerRot = Vector3.forward * 90;
                }
            }
            else
            {
                Debug.Log("UP");
                playerRot = Vector3.forward * 0;
            }
        }
        wallNum = wa.abilityNumber;
    }
}
