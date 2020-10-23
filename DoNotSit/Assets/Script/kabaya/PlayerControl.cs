using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;


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


    //プレイヤーのベクトル
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
            playerRig.AddForce(transform.up * jumpSpeed, ForceMode.Impulse);
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
            currentPlayerState = PlayerState.Normal;
            //↓こいつでくっついて反転！！
            //this.transform.Rotate(Vector3.forward, this.transform.rotation.z + 180);
            combo = 0;
            Debug.Log("当たった");
            //transform.rotation = Quaternion.FromToRotation(Vector3.forward, Vector3.back);
            // ReflectionUp();
            wa = col.gameObject.GetComponent<WallAbility>();
            switch (wa.abilityNumber)
            {
                case 2://沼の床
                    playerRig.velocity = Vector3.zero;
                    jumpSpeed = jumpSpeed / 2;
                    //ジャンプしたらmaxJumpForceをもとに戻す；
                    break;
                case 3://反射
                    ReflectAction(col.gameObject);
                    break;
                default:
                    break;
            }
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
                case 4://滑る床
                    SripAction(col.gameObject);
                    break;
                case 5://とげ
                    timer -= 10.0f;
                    //Destroy(this.gameObject);
                    break;
                case 6://斜めの反射
                    SkewRefrect(col.gameObject);
                    break;
                default:
                    break;
            }
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
    //反射挙動
    private void ReflectAction(GameObject col)
    {
        Debug.Log(playerVec);
        //Z回転軸取得
        float a = col.gameObject.transform.localEulerAngles.z;
        //プレイヤーの回転軸を0にする
        gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
        //当たったオブジェクトのどこにあたったかで向きを変える
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
                    gameObject.transform.Rotate(Vector3.forward *90);
                }
            }
            else
            {
                Debug.Log("UP");
                gameObject.transform.Rotate(Vector3.forward*0);
            }
        }
        //playerの向き取得
        Vector3 n = gameObject.transform.up;
        //内積
        float h = Mathf.Abs(Vector3.Dot(playerVec, n));
        //反射ベクトル
        Vector3 r = playerVec+2* n * h;
        //代入
        playerRig.velocity = r;
        //値変更
        playerVec = playerRig.velocity;

    }
    //斜めの床反射
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
        playerVec = playerRig.velocity;
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
