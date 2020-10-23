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
    public bool rayFlag;//壁云々
    public float jumpSpeed = 20.0f;//ジャンプの力
    public float jumpSpeedUp = 1.2f;//ジャンプアップの力
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
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
        //アタックステート
        if (currentPlayerState == PlayerState.Attack)
        {
            rb.AddForce(transform.up * jumpSpeed, ForceMode.Impulse);
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
        if (Input.GetButtonDown("Jump") && jumpFlag == false && rayFlag　==　true)
        {
            jumpFlag = true;
            angleZ = 0;
            currentPlayerState = PlayerState.Attack;
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
            rb.velocity = Vector3.zero;
            currentPlayerState = PlayerState.Normal;
            //↓こいつでくっついて反転！！
             this.transform.Rotate(Vector3.forward, this.transform.rotation.z + 180);
            combo = 0;
            Debug.Log("当たった");
            //transform.rotation = Quaternion.FromToRotation(Vector3.forward, Vector3.back);
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
            combo++;//コンボ増加
           // timer += combo;//コンボ時間に反映
            //  Destroy(col.gameObject);
            //EnemeyUp();//敵を倒すパターン
        }
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
}
