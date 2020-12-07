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
    private bool jumpFlag;//ジャンプフラグ
    private bool restratFlag;//障害物の当たり判定のフラグ
    private bool rayFlag;//壁云々
    private bool matFlag;//沼フラグ
    private bool revFlag;//スティック反転フラグ
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
    //コンボ
    public float comboTimer = 0f;//コンボタイマー
    public float comboTimerAdd;//コンボタイマー
    public float comboTimerMax;//コンボタイマーマックス
    public float combo;//コンボ
    public float comboBonus;
    private bool comboFlag;//コンボフラグ
    public float rayline;//レイ長さ
    public int level = 1;//レベル
    public static float scoreNumber;
                         //  public int speedCount;//連続用
    private int exp;//経験値
    public int hp;
    public Text comboText;
    private Text levelText;//レベルテキスト
    private Text expText;
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
    public AudioClip damegeSE;
    public GameObject effectPos;
    public GameObject effectPos2;
    public GameObject sperkEffect;
    private float maxAngleSet;
    private float minAngleSet;

    //追加
    Vector3 hitPoint;
    Vector3 playerRot = Vector3.zero;
    GameObject hitObject;
    int wallNum;
    bool colFlag;
    Fade fade;
    int damage = 1;
    //
    public float angleSpeed = 1;
    public float angleLimit=40;
    float playerAngle = 0;
    public bool select = false;
    GameObject moveWall;
    public List<GameObject> carsol;

    public int ReflectCount;
    private int refCount = 0;
    [SerializeField]
    private LifeGauge life;
    [SerializeField]
    private LifeGauge lifeback;
    public  Animator animator;
    [SerializeField]
    private Renderer[] renderer;
    int timeRender;
    bool ren;

    bool flag = false;
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
        life = GameObject.Find("HPpanel").GetComponent<LifeGauge>();
        lifeback= GameObject.Find("HPpanelBack").GetComponent<LifeGauge>();
        SetAngle();
        refCount = ReflectCount;
        life.SetLifeGauge(hp);
        lifeback.SetLifeGauge(hp);
        }

    // Update is called once per frame
    void Update()
    {

        RayObject();
        Tenmetu();
        //ノーマルステート
        if (currentPlayerState == PlayerState.Normal)//ノーマル
        {
            Invoke("Move", 0.0001f);//プレイに支障はないはず
            Jump();
            PlayerWallMove();
            playerRig.velocity = Vector3.zero;
            animator.SetBool("Jump", false);
        }
        if (currentPlayerState == PlayerState.Attack)//アタック中
        {
            ob.SetActive(false);
            animator.SetBool("Jump", true);
        }
        if (currentPlayerState == PlayerState.Stop)//ストップ中
        {
            ob.SetActive(false);
            playerRig.velocity = Vector3.zero;
        }
        //タイマー
        timer += 1.0f * Time.deltaTime;
        //comboText.text = combo.ToString()+ "COMOBO!";//コンボ
        hpText.text = "HP:";
        scoreText.text =  scoreNumber.ToString();
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
        if (!select)
        {
            switch (playerAngle)
            {
                case 0:
                    if (turn > 0 && Rot.z > minAngleSet || turn > 0 && Rot.z < maxAngleSet+angleSpeed*2)
                    { transform.Rotate(Vector3.back * angleSpeed * (turn * turn)); }
                    else if (turn < 0 && Rot.z > minAngleSet-angleSpeed*2 || turn < 0 && Rot.z < maxAngleSet)
                    { transform.Rotate(Vector3.forward * angleSpeed * (turn * turn)); }
                    break;
                case 90:
                    if (up > 0 && Rot.z > minAngleSet)
                    { transform.Rotate(Vector3.back * angleSpeed * (up*up)); }
                    else if (up < 0 && Rot.z < maxAngleSet)
                    { transform.Rotate(Vector3.forward * angleSpeed * (up * up)); }
                    break;
                case 180:
                    if (turn > 0 && Rot.z < maxAngleSet)
                    { transform.Rotate(Vector3.forward * angleSpeed *(turn*turn)); }
                    else if (turn < 0 && Rot.z > minAngleSet)
                    { transform.Rotate(Vector3.back * angleSpeed * (turn * turn)); }
                    break;
                case 270:
                    if (up > 0&&Rot.z<maxAngleSet)
                    { transform.Rotate(Vector3.forward * angleSpeed * (up * up)); }
                    else if (up < 0&&Rot.z>minAngleSet)
                    { transform.Rotate(Vector3.back * angleSpeed * (up * up)); }
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
    public static float ClearScore()
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
            ray = new Ray(transform.position +Vector3.right*(i - 1)*2.0f, transform.up);
            Debug.DrawRay(transform.position + Vector3.right * 2.0f * (i - 1), transform.up * rayline, Color.red, 0, true);
            //レイの判定(飛ばすレイ、レイが当たったものの情報、レイの長さ)
            if (Physics.Raycast(ray, out hit, rayline)) //壁がある時
            {
                Physics.queriesHitTriggers = false;//こいつでトリガーのやつは無視するぽい
                if (hit.collider.tag == "Wall")
                {
                    rayFlag = true;
                    ob.SetActive(true);
                    //令の長さを取得
                    float dis = Vector3.Distance(hit.point, transform.position + Vector3.right *2.0f* (i - 1));
                    if (a > dis)
                    {
                        a = dis;
                        //ヒットしてる位置を取得
                        hitPoint = hit.point;
                        //オブジェクトを取得
                        hitObject = hit.collider.gameObject;
                        test();
                    }                                    
                }
            }
            else //壁がない時
            {
                rayFlag = false;
                ob.SetActive(false);
            }
        }        
        
    }

    //壁との当たり判定
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Wall") && currentPlayerState == PlayerState.Attack)
        {
            if (!colFlag)
            {
                colFlag = true;
                if (hitObject!=col.gameObject)
                {
                    hitObject = col.gameObject;
                    foreach (ContactPoint point in col.contacts)
                    {
                        hitPoint = point.point;
                        Debug.Log("違うオブジェクト");
                    }
                    test();
                }

                jumpFlag = false;
                playerRig.velocity = Vector3.zero;
                switch (wallNum)
                {
                    case 0://着地
                        ReflectActionCount();
                        break;
                    case 1://沼の床
                        gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
                        gameObject.transform.Rotate(playerRot);
                        jumpSpeed = jumpDefalut;
                        SetAngle();
                        currentPlayerState = PlayerState.Normal;
                        break;
                    case 2://反射
                        ReflectAction();
                        break;
                    case 4://とげ
                        restratFlag = true;
                        Instantiate(sperkEffect,effectPos2.transform.position, transform.rotation);
                        //ReflectActionCount();
                        ReflectAction();
                        StartCoroutine("ThornTime");
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
                        ReflectMoveActionCount(col.gameObject);
                        break;
                    case 10:
                        ReflectActionCount();
                        break;
                    default:
                        break;
                }
            }
        }
        //EnemyCollision場合
        //アタック(移動中に当たると)タイマー増加
        if (col.gameObject.CompareTag("Enemy"))
        {
        //   ComboStart();
        }
        if (col.gameObject.CompareTag("Wall"))
        {
            wa = col.gameObject.GetComponent<WallAbility>();
            switch (wa.abilityNumber)
            {
                case 9://デスエリア
                    StageDate.Instance.SetData(SceneManager.GetActiveScene().name);
                    fade.StartFadeIn("GameOver", false);
                    break;
                default:
                    break;
            }
        }
        if (col.gameObject.CompareTag("ChaseEnemy"))
        {
            StageDate.Instance.SetData(SceneManager.GetActiveScene().name);
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
                    Instantiate(sperkEffect, effectPos2.transform.position, transform.rotation);
                    StartCoroutine("ThornTime");
                    break;
                case 5://斜めの反射
                    SkewRefrect(col.gameObject);
                    break;
                case 9://デスエリア                   
                    StageDate.Instance.SetData(SceneManager.GetActiveScene().name);
                    fade.StartFadeIn("GameOver", false);
                    break;
                default:
                    break;
            }
        }
        //EnemyTrigger場合
        //アタック(移動中に当たると)タイマー増加
        if (col.gameObject.CompareTag("Enemy"))
        {
           // ComboStart();
        }
    }
    #region 使ってない
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
        if (wa.Height(true) - (Scale.y / 2) >= gameObject.transform.position.y - (Scale.y / 2))
        {
            if (wa.Height(false) + (Scale.y / 2) <= gameObject.transform.position.y + (Scale.y / 2))
            {
                if (gameObject.transform.position.x > col.gameObject.transform.position.x)
                {
                    playerRig.velocity = new Vector3(0, playerRig.velocity.y, 0);
                }
                else
                {
                    playerRig.velocity = new Vector3(0, playerRig.velocity.y, 0);
                }
            }
            else
            {
                playerRig.velocity = new Vector3(playerRig.velocity.x, 0, 0);
            }
        }
        else
        {
            if (wa.Width(true) - (Scale.x / 2) <= gameObject.transform.position.x - (Scale.x / 2) || wa.Width(false) + (Scale.x / 2) >= gameObject.transform.position.x + (Scale.x / 2))
            {
                if (gameObject.transform.position.x > col.gameObject.transform.position.x)
                {
                    playerRig.velocity = new Vector3(0, playerRig.velocity.y, 0);
                }
                else
                {
                    playerRig.velocity = new Vector3(0, playerRig.velocity.y, 0);
                }
            }
            else
            {
                playerRig.velocity = new Vector3(playerRig.velocity.x, 0, 0);
            }
        }
    }
    #endregion
    public void Damage(int damage)
    {
        hp -= damage;
        ren = true;
        audio.PlayOneShot(damegeSE);
        if (hp < 0)
        {
            //　ダメージ調整
            damage = Mathf.Abs(hp + damage);
            hp = 0;
        }
        if (damage > 0)
        {
            life.SetLifeGauge2(damage);
        }
    }
    public void Score(float score)
    {
        scoreNumber += score;
        if (comboFlag == true)
        {
            scoreNumber += score*comboBonus;//ボーナス
        }
        ComboStart();
    }
    void Tenmetu()
    {
        foreach (Renderer childRender in renderer)
        {
            if(ren==true)
            {
                timeRender++;
                bool oddeven = Mathf.FloorToInt(Time.time * 5) % 2 == 0;
                childRender.enabled = oddeven;
                if(timeRender>=200)
                {
                    ren = false;
                }
            }
            else
            {
                timeRender = 0;
                childRender.enabled = true;
            }
        }
    }
    IEnumerator ThornTime()
    {
        yield return new WaitForSeconds(0.1f);
        if (restratFlag == true)
        {
           Damage(damage);
        }
        restratFlag = false;
        yield break;
    }
    //コンボ系
    public void Combo()
    {
        if (comboFlag == true)
        {
           // comboText.enabled = true;
            comboTimer -= 1.0f * Time.deltaTime;
            if (comboTimer <= 0)
            {
                comboFlag = false;
            }
            ComboBonus();
        }
        else if (comboFlag == false)
        {
            combo = 0;
            comboBonus = 0;
           // comboText.enabled = false;
        }
    }
    public void ComboStart()
    {
        combo++;
        comboTimer += comboTimerAdd;
        if(comboTimer>=comboTimerMax)
        {
            comboTimer = comboTimerMax;
        }
        comboFlag = true;
    }
    public void ComboBonus()
    {
        if (combo >= 2)
        {
            comboBonus = 1.2f;
        }
        if (combo >= 4)
        {
            comboBonus = 1.4f;
        }
        if (combo >= 6)
        {
            comboBonus = 1.6f;
        }
        if (combo >= 8)
        {
            comboBonus = 1.8f;
        }
        if (combo >= 10)
        {
            comboBonus = 2.0f;
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
    //反射の数をカウントしない
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
       // RayObject();
    }
    //反射の数をカウントする
    private void ReflectActionCount()
    {
        CapsuleCollider col = GetComponent<CapsuleCollider>();
        col.isTrigger = true;
        if (refCount<1)
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
           // RayObject();
            refCount--;
        }
        col.isTrigger = false;
    }
    //動く床の反射用
    private void ReflectMoveActionCount(GameObject colObj)
    {
        CapsuleCollider col = GetComponent<CapsuleCollider>();
        col.isTrigger = true;
        if (refCount < 1)
        {
            moveWall = colObj;
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
           // RayObject();
            refCount--;
        }
        col.isTrigger = false;
    }
   
    //回転の判定
    private void test()
    {
        //コンポーネント取得
        wa = hitObject.GetComponent<WallAbility>();
        float x = hitPoint.x;
        float y = hitPoint.y;

        x *= 10;
        y *= 10;

        x = Mathf.Floor(x) / 10;
        y = Mathf.Floor(y) / 10;

        //どの位置にあたったか判定し回転する
        if (y == wa.Height(true) || y + 0.1f == wa.Height(true) || y - 0.1f == wa.Height(true))
        {
            if(wa.colObjs[0] ==null)
            {
                playerRot = Vector3.forward * 0;
                Debug.Log("Up");
            }           
        }
        else if (y == wa.Height(false) || y + 0.1f == wa.Height(false) || y - 0.1f == wa.Height(false))
        {
            if (wa.colObjs[1] == null)
            {
                playerRot = Vector3.forward * 180;
                Debug.Log("Down");
            }
        }
        else if (x == wa.Width(true) || x + 0.1f == wa.Width(true) || x - 0.1f == wa.Width(true))
        {
            if (wa.colObjs[2] == null)
            {
                playerRot = Vector3.forward * 270;
                Debug.Log("Right");
            }
        }
        else if (x == wa.Width(false) || x + 0.1f == wa.Width(false) || x - 0.1f == wa.Width(false))
        {
            if (wa.colObjs[3] == null)
            {
                playerRot = Vector3.forward * 90;
                Debug.Log("Left");
            }
        }
        wallNum = wa.abilityNumber;
    }
    //動く床の判定
    private void coltest()
    {
        //コンポーネント取得
        wa = hitObject.GetComponent<WallAbility>();
        float x = hitPoint.x;
        float y = hitPoint.y;

        x *= 10;
        y *= 10;

        x = Mathf.Floor(x) / 10;
        y = Mathf.Floor(y) / 10;

        //どの位置にあたったか判定し回転する
        if (y == wa.Height(true) || y + 0.1f == wa.Height(true) || y - 0.1f == wa.Height(true))
        {
            playerRot = Vector3.forward * 0;
        }
        else if (y == wa.Height(false) || y + 0.1f == wa.Height(false) || y - 0.1f == wa.Height(false))
        {
            playerRot = Vector3.forward * 180;
        }
        else if (x == wa.Width(true) || x + 0.1f == wa.Width(true) || x - 0.1f == wa.Width(true))
        {
            playerRot = Vector3.forward * 270;
        }
        else if (x == wa.Width(false) || x + 0.1f == wa.Width(false) || x - 0.1f == wa.Width(false))
        {
            playerRot = Vector3.forward * 90;
        }
        wallNum = wa.abilityNumber;
    }
    //判定を見るよう
    private void vecTest()
    {
        //コンポーネント取得
        wa = hitObject.GetComponent<WallAbility>();
        float x = hitPoint.x;
        float y = hitPoint.y;

        x *= 10;
        y *= 10;

        x = Mathf.Floor(x) / 10;
        y = Mathf.Floor(y) / 10;

        //どの位置にあたったか判定し回転する
        if (y == wa.Height(true) || y + 0.1f == wa.Height(true) || y - 0.1f == wa.Height(true))
        {
            Debug.Log("Up");
        }
        else if (y == wa.Height(false) || y + 0.1f == wa.Height(false) || y - 0.1f == wa.Height(false))
        {
            Debug.Log("Down");
        }
        else if (x == wa.Width(true) || x + 0.1f == wa.Width(true) || x - 0.1f == wa.Width(true))
        {
            Debug.Log("Right");
        }
        else if (x == wa.Width(false) || x + 0.1f == wa.Width(false) || x - 0.1f == wa.Width(false))
        {
            Debug.Log("Left");
        }
        Debug.Log(wa.Width(true));
        Debug.Log(wa.Width(false));
        Debug.Log(wa.Height(true));
        Debug.Log(wa.Height(false));
        Debug.Log(hitPoint);
    }

    //動く床のプレイヤーの動き
    private void PlayerWallMove()
    {
        if(moveWall!=null)
        {
            WallAbility wa = moveWall.GetComponent<WallAbility>();
            if (wa.abilityNumber == 8)
            {
                MoveWall mw = moveWall.GetComponent<MoveWall>();
                mw.Move(gameObject, mw.speedX,mw.speedY);
            }
            else if (wa.abilityNumber == 9)
            {
                StageDate.Instance.SetData(SceneManager.GetActiveScene().name);
                fade.StartFadeIn("GameOver", false);
            }
            else
            { }
        }
    }

    //カーソルの動き
    private void Carsolmove()
    {
        //パターン1
        carsol[0].transform.position = hitPoint;
        Vector3 vec = (hitPoint - gameObject.transform.position).normalized;
        var distance = Vector3.Distance(hitPoint, gameObject.transform.position);
        Vector3 pos = (hitPoint + gameObject.transform.position) / 2;
        pos = (carsol[1].transform.position + gameObject.transform.position) / 2;
        int a = 0;
        for (int i = 1; i < carsol.Count; i++)
        {
            carsol[i].transform.position = pos + new Vector3(vec.x * i * 8, vec.y * i * 8, 0);
            carsol[i].transform.LookAt(hitPoint, Vector3.up);

            if (gameObject.transform.position.x <= hitPoint.x)//右
            {
                if (gameObject.transform.position.y < hitPoint.y)//上
                {
                    if (carsol[i].transform.position.y > hitPoint.y)//上すぎたら
                    {
                        carsol[i].SetActive(false);
                        a++;
                    }
                    else { carsol[i].SetActive(true); }
                }
                else//した
                {
                    if (carsol[i].transform.position.y < hitPoint.y)
                    {
                        carsol[i].SetActive(false);
                        a++;
                    }
                    else { carsol[i].SetActive(true); }
                }
            }
            else//左
            {
                if (gameObject.transform.position.y < hitPoint.y)//上
                {
                    if (carsol[i].transform.position.y > hitPoint.y)//上すぎたら
                    {
                        carsol[i].SetActive(false);
                        a++;
                    }
                    else { carsol[i].SetActive(true); }
                }
                else//した
                {
                    if (carsol[i].transform.position.y < hitPoint.y)
                    {
                        carsol[i].SetActive(false);
                        a++;
                    }
                    else { carsol[i].SetActive(true); }
                }
            }
        }
        
        //飛んだら消える
        if (currentPlayerState == PlayerState.Attack||!rayFlag)
        {
            for (int i = 0; i < carsol.Count; i++)
            {
                carsol[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < carsol.Count-a; i++)
            {
                carsol[i].SetActive(true);
            }
        }

    }
}
