﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum PlayerState
{
    Normal,
    Attack
}
public class PlayerControl : MonoBehaviour
{
    Rigidbody playerRig;
    public bool jumpFlag;//ジャンプフラグ
    public bool restratFlag;//障害物の当たり判定のフラグ
    public bool rayFlag;//壁云々
    public bool matFlag;//沼フラグ
    public float jumpSpeed = 20.0f;//ジャンプの力
    public float jumpSpeedUp = 1.2f;//ジャンプアップの力
    public float jumpDefalut;//※ジャンプデフォルト
    public float MaxjumpSpeed;//ジャンプ最大値
    public float maxAngle = 44.0f; // 最大回転角度
    public float minAngle = -44.0f; // 最小回転角度
    public float roateSpeed = 1.0f;//回るスピード
    public float angleZ;//こいつ大事回転制御
    public float timer = 60.0f;//タイマー
  //  public float comboTimer = 0f;//コンボタイマー
    public float combo;//コンボ
    public float rayline;//レイ長さ
    public int level;//レベル
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
    // Start is called before the first frame update
    void Start()
    {
        playerRig = GetComponent<Rigidbody>();
        jumpFlag = false;
        rayFlag =　false;
        currentPlayerState = PlayerState.Normal;
    }

    // Update is called once per frame
    void Update()
    {
        //ノーマルステート
        if (currentPlayerState == PlayerState.Normal)
        {
            Move();
            Jump();
            RayObject();
        }
        //タイマー
        timer -= 1.0f * Time.deltaTime;
        timerText.text = timer.ToString("f2") + "秒";//制限時間
        comboText.text = combo.ToString();//コンボ
        Combo();//コンボ関連
        // levelText.text = level.ToString();
        if (timer <= 0)
        {
            Destroy(this.gameObject);
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
    void Jump()//ジャンプ系
    {
        if (Input.GetButtonUp("Jump") && jumpFlag == false && rayFlag　==　true)
        {
            jumpFlag = true;
            angleZ = 0;
            currentPlayerState = PlayerState.Attack;
            playerRig.AddForce(transform.up * jumpSpeed, ForceMode.Impulse);//ジャンプする
            playerVec = transform.up * jumpSpeed;
        }
    }
    //壁があるない
    void RayObject()
    {
        Ray ray = new Ray(transform.position, transform.up);
        RaycastHit hit;
        //レイの判定(飛ばすレイ、レイが当たったものの情報、レイの長さ)
        if (Physics.Raycast(ray, out hit, rayline)) //壁がある時
        {
            if (hit.collider.tag == "Wall") 
            {
                rayFlag = true;
                ob.SetActive(true);
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
            jumpFlag = false;
            playerRig.velocity = Vector3.zero;
            //↓こいつでくっついて反転！！
            this.transform.Rotate(Vector3.forward, this.transform.rotation.z + 180);
            combo = 0;
            Debug.Log("当たった");
            //transform.rotation = Quaternion.FromToRotation(Vector3.forward, Vector3.back);
            // ReflectionUp();
            wa = col.gameObject.GetComponent<WallAbility>();
            switch (wa.abilityNumber)
            {
                case 0://着地
                    currentPlayerState = PlayerState.Normal;
                    //めり込んだ時の処理を書く
                    break;
                case 1://沼の床
                    playerRig.velocity = Vector3.zero;
                    jumpSpeed = jumpDefalut;
                    currentPlayerState = PlayerState.Normal;
                    //ジャンプしたらmaxJumpForceをもとに戻す；
                    break;
                case 2://反射
                    ReflectAction(col.gameObject);
                    break;
                default:
                    break;
            }
        }
        //EnemyCollision場合
        //アタック(移動中に当たると)タイマー増加
        if (col.gameObject.CompareTag("Enemy") && currentPlayerState == PlayerState.Attack)
        {
            combo++;//コンボ増加
                    // timer += combo;//コンボ時間に反映
                    //  Destroy(col.gameObject);
                    //EnemeyUp();//敵を倒すパターン
        }
    }
    //リスタート用
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Wall"))
        {
            wa = col.gameObject.GetComponent<WallAbility>();
            switch (wa.abilityNumber)
            {
                case 3://滑る床
                    SripAction(col.gameObject);
                    break;
                case 4://とげ
                    timer -= 10.0f;
                    //Destroy(this.gameObject);
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
            combo++;//コンボ増加
           // timer += combo;//コンボ時間に反映
            //  Destroy(col.gameObject);
            //EnemeyUp();//敵を倒すパターン
        }
    }
    public void Damage(float damage)
    {
        timer -= damage;
    }
    //コンボ系
    public void Combo()
    {
        if (combo>=1)
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
        if (level == 0 & exp >= 1)
        {
            level += 1;
            jumpSpeed *= jumpSpeedUp;
        }
        if (level == 1 && exp >= 5)
        {
            level += 1;
            jumpSpeed *= jumpSpeedUp;
        }
    }
    private void ReflectAction(GameObject col)
    {
        //Z回転軸取得
        float a = col.gameObject.transform.localEulerAngles.z;
        gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
        if (wa.Height(true).y - (Scale.y / 2) >= gameObject.transform.position.y - (Scale.y / 2))
        {
            if (wa.Height(false).y + (Scale.y / 2) <= gameObject.transform.position.y + (Scale.y / 2))
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
        float h = Mathf.Abs(Vector3.Dot(playerVec, n));
        //反射ベクトル
        Vector3 r = playerVec + 2 * n * h;
        //代入
        playerRig.velocity = r;
        playerVec = playerRig.velocity;
        //
        Debug.Log("平面呼んだ");
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
}
