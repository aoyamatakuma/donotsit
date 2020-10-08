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
    public float jumpForce = 2.0f;//ジャンプの力
    public float attackForce = 10.0f;//ジャンプの力
    public float attackTime;
    public float speed = 2.0f;//地上での移動速度
    Rigidbody rb;
    public bool jumpFlag;
    public bool attackFlag;
    public int hp;
    public int attackCount;
    // Start is called before the first frame update
    void Start()
    {
        rb =  GetComponent<Rigidbody>();
        jumpFlag = false;
        attackFlag = false;
        hp = 3;
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
        rb.velocity = new Vector2(speed * h,rb.velocity.y);
        if (h > 0)//右
        {
            transform.localRotation = new Quaternion(0, 0, 0, 0);
        }
        else if (h < 0)//左
        {
            transform.localRotation = new Quaternion(0, 180, 0, 0);
        }
    }
    void Jump()//ジャンプ系
    {
        //Aボタン
        if (Input.GetButtonDown("Jump")&& jumpFlag == false)
        {
            rb.AddForce(Vector2.up * jumpForce);
            jumpFlag = true;
        }
    }
    void Attack()//攻撃
    {
        var h1 = Input.GetAxis("Horizontal");
        var v1 = Input.GetAxis("Vertical");
        //Bボタン
        if (Input.GetButtonDown("Attack"))
        {
          //  transform.position += new Vector3(50.0f * Time.deltaTime, 0, 0);
            if (h1 != 0 || v1 != 0)
            {
                transform.position +=  new Vector3(h1, v1, 0);
                attackTime = 0;
                attackFlag = true;
            }
            else
            {
                transform.position += new Vector3(50.0f * Time.deltaTime, 0, 0);
                attackTime = 0;
            }
        }
        else
        {
            attackFlag = false;
        }
    }
    void PowerAttack()//強攻撃
    {
        var h1 = Input.GetAxis("Horizontal");
        var v1 = Input.GetAxis("Vertical");
        //長押しBボタン
        if (Input.GetButton("Attack"))
        {
            attackTime += Time.deltaTime;
            
        }
        else if (Input.GetButtonUp("Attack")&& attackTime >=3.0f)
        {
          // transform.position += new Vector3((50.0f*attackTime) * Time.deltaTime, 0, 0);
               if (h1 != 0 || v1 != 0)
            {
                transform.position +=  new Vector3(h1, v1, 0);
                attackTime = 0;
            }
               else
            {
                    transform.position += new Vector3(50.0f * Time.deltaTime, 0, 0);
                attackTime = 0;
            }
        }
    }
    void OnCollisionEnter(Collision col)
    {

        if (col.gameObject.CompareTag("Untagged"))
        {
            jumpFlag = false;
        }
    }
}
