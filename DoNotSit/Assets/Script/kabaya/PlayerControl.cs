using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Normal,
    Charge,
    Attack,
    PowerAttack
}
public class PlayerControl : MonoBehaviour
{
    public float jumpForce = 2.0f;//ジャンプの力
    public float attackForce = 2.0f;//アタックの力
    public float attackTime;//チャージ
    public float speed = 2.0f;//地上での移動速度
    Rigidbody rb;
    public bool jumpFlag;//ジャンプフラグ
    public bool attackFlag;//アタックフラグ
    public int hp;//Hp
    public int attackCount;
    public int chargeCount;
    public PlayerState currentPlayerState; //現在の状態
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumpFlag = false;
        attackFlag = false;
        hp = 3;
        currentPlayerState = PlayerState.Normal;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        Attack();
        PowerAttack();
    }
    void Move()//移動系
    {
        //左スティック
        float h = Input.GetAxis("Horizontal");
        if (currentPlayerState == PlayerState.Normal)
        {
            rb.velocity = new Vector2(speed * h, rb.velocity.y);
            if (h > 0)//右
            {
                //  transform.localRotation = new Quaternion(0, 0, 0, 0);
            }
            else if (h < 0)//左
            {
                //  transform.localRotation = new Quaternion(0, 180, 0, 0);
            }
        }
    }
    void Jump()//ジャンプ系
    {
        //Aボタン Input.GetButtonDown("Jump")&&
        if (Input.GetButtonDown("Jump") && jumpFlag == false)
        {
            // rb.AddForce(Vector2.up * jumpForce);
            rb.velocity = Vector2.up * jumpForce;
            jumpFlag = true;
        }
    }
    void Attack()//攻撃
    {
        var h1 = Input.GetAxis("Horizontal");
        var v1 = Input.GetAxis("Vertical");
        //Bボタン
        if (Input.GetButtonUp("Attack"))
        {
            if (h1 != 0 || v1 != 0)
            {
                transform.position += new Vector3(h1 * Time.deltaTime, v1 * Time.deltaTime, 0) * attackForce;
                attackFlag = true;
                currentPlayerState = PlayerState.Attack;
            }
        }
        else
        {
            attackFlag = false;
            currentPlayerState = PlayerState.Normal;
        }
    }
    void PowerAttack()//強攻撃
    {
        var h1 = Input.GetAxis("Horizontal");
        var v1 = Input.GetAxis("Vertical");
        //長押しBボタン
        if (Input.GetButton("Attack") && attackFlag == false)
        {
            chargeCount++;
            currentPlayerState = PlayerState.Charge;
        }
        else if (Input.GetButtonUp("Attack") && attackCount <= chargeCount && attackFlag == false)
        {
            if (h1 != 0 || v1 != 0)
            {
                transform.position += new Vector3(h1 * Time.deltaTime, v1 * Time.deltaTime, 0) * (attackForce * 2);
                attackFlag = true;
            }
            //else
            //{
            //    transform.position += new Vector3(attackForce * Time.deltaTime, 0, 0) * 2 ;
            //    attackFlag = true;
            //}
            currentPlayerState = PlayerState.PowerAttack;
        }
        else
        {
            attackFlag = false;
            chargeCount = 0;
            currentPlayerState = PlayerState.Normal;
        }
    }
    void OnCollisionEnter(Collision col)
    {

        if (col.gameObject.CompareTag("Wall"))
        {
            jumpFlag = false;
        }
    }
}
