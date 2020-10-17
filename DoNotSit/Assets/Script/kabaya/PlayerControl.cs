using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerState
{
    Normal,
    Attack
}
public class PlayerControl : MonoBehaviour
{
    public float jumpSpeed = 2.0f;//ジャンプの力
    public float roateSpeed = 2.0f;//回るスピード
    Rigidbody rb;
    public bool jumpFlag;//ジャンプフラグ
    public PlayerState currentPlayerState; //現在の状態
    public float maxAngle = 70f; // 最大回転角度
    public float minAngle = -70f; // 最小回転角度
    public float timer = 60f;//タイマー
    public int combo;//コンボ
    public Vector3 localGravity;
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
        setLocalGravity();
        //ノーマルステート
        if (currentPlayerState == PlayerState.Normal)
        {
            Move();
            Jump();
        }
        //アタックステート
        if (currentPlayerState == PlayerState.Attack)
        {
            rb.AddForce(transform.up * jumpSpeed, ForceMode.Impulse);
        }
        //タイマー
        timer -= 1.0f * Time.deltaTime;
        if (timer == 0)
        {
            Destroy(gameObject);
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
        if (Input.GetButtonDown("Jump")&&jumpFlag==false)
        {
            jumpFlag = true;
            currentPlayerState = PlayerState.Attack;
        }
    }
    void setLocalGravity()
    {
        rb.AddForce(localGravity, ForceMode.Acceleration);
    }
    void OnCollisionEnter(Collision col)
    {

        if (col.gameObject.CompareTag("Wall"))
        {
            jumpFlag = false;
            currentPlayerState = PlayerState.Normal;
            //rb.useGravity = false;
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
        }
    }
    //反射
    public void ReflectionUp()
    {

    }
    //レベルアップ
    public void LevelUp()
    {

    }
}
