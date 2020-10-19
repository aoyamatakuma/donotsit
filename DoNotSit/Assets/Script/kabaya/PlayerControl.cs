using System.Collections;
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

    Rigidbody rb;
    public bool jumpFlag;//ジャンプフラグ
    public bool restratFlag;//障害物の当たり判定のフラグ
    public float jumpSpeed = 2.0f;//ジャンプの力
    public float jumpSpeedUp = 1.2f;//ジャンプアップの力
    public float roateSpeed = 2.0f;//回るスピード
    public PlayerState currentPlayerState; //現在の状態
    public float maxAngle = 45f; // 最大回転角度
    public float minAngle = -45f; // 最小回転角度
    public float RemaxAngle = 135f; // 最大回転角度
    public float ReminAngle = -135f; // 最小回転角度
    public float timer = 60f;//タイマー
    public float comboTimer = 1.0f;//コンボタイマー
    public int combo;//コンボ
    public int level;//レベル
    public int speedCount;//連続用
    public int exp;//経験値
    public Text timerText;//タイマーテキスト
    public Text levelText;//レベルテキスト
    public GameObject ob;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumpFlag = false;
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
            ob.SetActive(true);
        }
        //アタックステート
        if (currentPlayerState == PlayerState.Attack)
        {//, ForceMode.Impulse
            rb.AddForce(transform.up * jumpSpeed);
            ob.SetActive(false);
        }
        //タイマー
        timer -= 1.0f * Time.deltaTime;
        timerText.text = timer.ToString("f1") + "秒";
        if (timer <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    void Move()//移動系
    {
        //左スティック
        float turn = Input.GetAxis("Horizontal");
        // 現在の回転角度を0～360から-180～180に変換
        float rotateZ = (transform.eulerAngles.z > 180) ? transform.eulerAngles.z - 360 : transform.eulerAngles.z;
        // 現在の回転角度に入力(turn)を加味した回転角度をMathf.Clamp()を使いminAngleからMaxAngle内に収まるようにする
        float angleZ = Mathf.Clamp(rotateZ - turn * roateSpeed, minAngle, maxAngle);
        // 回転角度を-180～180から0～360に変換
        angleZ = (angleZ < 0) ? angleZ + 360 : angleZ;
        // 回転角度をオブジェクトに適用
        transform.rotation = Quaternion.Euler(0, 0, angleZ);
    }
    void Jump()//ジャンプ系
    {
        if (Input.GetButtonDown("Jump") && jumpFlag == false)
        {
            jumpFlag = true;
            currentPlayerState = PlayerState.Attack;
        }
    }
    //壁との当たり判定
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Wall"))
        {
            jumpFlag = false;
            currentPlayerState = PlayerState.Normal;
            //this.transform.Rotate(Vector3.forward,this.transform.rotation.z +180);
            rb.useGravity = false;
        }
    }
    //リスタート用
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Wall"))
        {
        }
        //当たるとタイマー減少
        if (col.gameObject.CompareTag("Enemy") && currentPlayerState == PlayerState.Normal)
        {
            timer -= 10.0f;
        }
        //アタック(移動中に当たると)タイマー増加
        if (col.gameObject.CompareTag("Enemy") && currentPlayerState == PlayerState.Attack)
        {
            timer += 2.0f * Time.deltaTime;
            combo++;
            //Destroy(col.gameObject);
            //敵を倒すパターン
            //jumpSpeed *= jumpSpeedUp;
        }
    }
    //コンボ系
    public void Combo()
    {
     
    }
    //反射パターン
    public void ReflectionUp()
    {
        jumpSpeed *= jumpSpeedUp;
    }
    //レベルアップパターン
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
}
