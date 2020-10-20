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
    public float jumpSpeed = 20.0f;//ジャンプの力
    public float jumpSpeedUp = 1.2f;//ジャンプアップの力
    public float roateSpeed = 2.0f;//回るスピード
    public PlayerState currentPlayerState; //現在の状態
    public float maxAngle = 45.0f; // 最大回転角度
    public float minAngle = -45.0f; // 最小回転角度
    public float roateAngle=1.0f;
    public float timer = 60.0f;//タイマー
    public float comboTimer = 1.0f;//コンボタイマー
    public int combo;//コンボ
    public int level;//レベル
    public int speedCount;//連続用
    public int exp;//経験値
    public Text timerText;//タイマーテキスト
    public Text levelText;//レベルテキスト
    public GameObject ob;
    Vector3 angle;
    public Transform basePosition;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumpFlag = false;
        currentPlayerState = PlayerState.Normal;
        angle = transform.localEulerAngles;
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
        timerText.text = timer.ToString("f2") + "秒";
        if (timer <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    void Move()//移動系
    {
        //////左スティック
        float turn = Input.GetAxis("Horizontal");
        //// 現在の回転角度を0～360から-180～180に変換
        //float rotateZ = (transform.eulerAngles.z > 180) ? transform.eulerAngles.z - 360 : transform.eulerAngles.z;
        //// 現在の回転角度に入力(turn)を加味した回転角度をMathf.Clamp()を使いminAngleからMaxAngle内に収まるようにする
        //float angleZ = Mathf.Clamp(rotateZ - turn * roateSpeed, minAngle, maxAngle);
        //// 回転角度を-180～180から0～360に変換
        //angleZ = (angleZ < 0) ? angleZ + 360 : angleZ;
        //// 回転角度をオブジェクトに適用
        //transform.rotation = Quaternion.Euler(0, 0, angleZ);
        if (turn >= 1)
        {
          //   transform.Rotate(0f, 0f, -roateAngle);
            transform.RotateAround(basePosition.transform.position,transform.forward,-roateAngle );
            if(roateAngle < minAngle)
            {
                transform.RotateAround(basePosition.transform.position, transform.forward, roateAngle);
            }
        }
        if (turn <= -1)
        {
         //  transform.Rotate(0f, 0f, roateAngle);
            transform.RotateAround(basePosition.transform.position, transform.forward,roateAngle);
            if (roateAngle > maxAngle)
            {
                transform.RotateAround(basePosition.transform.position, transform.forward, -roateAngle);
            }
        }
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
        if (col.gameObject.CompareTag("Wall") && currentPlayerState == PlayerState.Attack)
        {
            jumpFlag = false;
            currentPlayerState = PlayerState.Normal;
            //↓こいつでくっついて反転！！
            this.transform.Rotate(Vector3.forward, this.transform.rotation.z + 180);
            rb.velocity = Vector3.zero;
            // ReflectionUp();
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
            //EnemeyUp();
        }
    }
    //コンボ系
    public void Combo()
    {

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
}
