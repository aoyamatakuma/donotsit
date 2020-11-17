using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ChaseState
{
    Normal,
    Stop,
    LEVLE1,
    LEVLE2,
    LEVLE3,
    LEVLE4,
    LEVLE5,
    Clear
};
public class ChaseEnemy : MonoBehaviour
{
    public int enemyHitLimit;
    int hitCnt;
    public float speed;
    public ScrollState currentScrollState; //現在の状態
    public float delay;
    public float speedDefalut;
    float cnt;
    //レベル系
    int level = 1;//レベル
     float exp;//経験値
    public Text levelText;//レベルテキスト
    public int ChaseType;
    PlayerControl player;
    public GameObject bello;
    float attackCount;//カウント
    public float attackLimit;//限界
    public Transform point;
    public bool belooFlag;
    public GameObject carsol;
  public ChaseState currentChaseState;
    public float wait;
    Animator animator;
    public float MagnificationLev2;
    public float MagnificationLev3;
    public float MagnificationLev4;
    public float MagnificationLev5;
    // Start is called before the first frame update
    void Start()
    {
        belooFlag = false;
        transform.position = new Vector3(transform.position.x,
             transform.position.y,
             transform.position.z);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        animator = GetComponent<Animator>();
        currentChaseState = ChaseState.LEVLE1;
    }

    // Update is called once per frame
    void Update()
    {
        cnt += Time.deltaTime;
        if (cnt > delay)
        {
            Animation();
            if (currentChaseState != ChaseState.Stop)
            {
                LevelUp();
                if (hitCnt >= enemyHitLimit)
                {
                    //下がる処理ここに
                    LevelDown();
                    hitCnt = 0;
                }
                switch (ChaseType)
                {
                    case 1://右
                        Right();
                        Attack();
                        break;
                    case 2://左
                        Left();
                        break;
                    case 3://上
                        Up();
                        break;
                    case 4://下
                        Down();
                        break;
                }

            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "BlowAway")
        {
            hitCnt++;
        }
    }
    void Animation()
    {
        if (currentChaseState == ChaseState.Stop)
        {
            animator.SetBool("Sutan", true);   
        }
        else
        {
            animator.SetBool("Sutan", false);
        }
    }
    //右
    void Right()
    {
        Vector3 move = transform.position;
        move.x += speed * Time.deltaTime;//右
        transform.position = move;
    }
    //左
    void Left()
    {
        Vector3 move = transform.position;
        move.x -= speed * Time.deltaTime;//左
        transform.position = move;
    }
    //上
    void Up()
    {
        Vector3 move = transform.position;
        move.y += speed * Time.deltaTime;//上
        transform.position = move;
    }
    //下
    void Down()
    {
        Vector3 move = transform.position;
        move.y -= speed * Time.deltaTime;//下
        transform.position = move;
    }
    //ベロ攻撃
     void Attack()
    {
        if (player.currentPlayerState == PlayerState.Normal)//ノーマルの時
        {      
            attackCount+=Time.deltaTime;
            if (attackCount >= attackLimit)
            {
                belooFlag = false;
            }
        }
        else
        {
            attackCount = 0;           
        }

        if (attackCount >= attackLimit)//アタックする
        {
            if (!belooFlag)
            {
                belooFlag = true;
                carsol.SetActive(true);
                var parent = this.transform;
                Instantiate(bello, point.transform.position, bello.transform.rotation,parent);
                attackCount = 0;
            }
        }
    }
    //レベルアップ
    public void LevelUp()
    {
        //経験値UP
        exp += 1.0f * Time.deltaTime;
        // レベル系
        levelText.text = "SPEEDLEVRL:" + level.ToString();
        if (level == 0)//レベル1
        {
            level = 1;
            if(level==1)
            {
                currentChaseState = ChaseState.LEVLE1;
            }
        }
        if (exp >= 5.0f)//レベル2
        {
            level = 2;
            if (level == 2)
            {
                currentChaseState = ChaseState.LEVLE2;
                speed = speedDefalut;
                if (speed == speedDefalut)
                {
                    speed *= MagnificationLev2;
                }
            }
        }

        if (exp >= 10.0f)//レベル3
        {
            level = 3;
            if (level == 3)
            {
                currentChaseState = ChaseState.LEVLE3;
                speed = speedDefalut;
                if (speed == speedDefalut)
                {
                    speed *= MagnificationLev3;
                }
            }
        }

        if (exp >= 15.0f)//レベル4
        {
            level = 4;
            if (level == 4)
            {
                currentChaseState = ChaseState.LEVLE4;
                speed = speedDefalut;
                if (speed == speedDefalut)
                {
                    speed *= MagnificationLev4;
                }
            }
        }

        if (exp >= 20.0f)//レベル5
        {
            level = 5;
            if (level == 5)
            {
                currentChaseState = ChaseState.LEVLE5;
                speed = speedDefalut;
                if (speed == speedDefalut)
                {
                    speed *= MagnificationLev5;
                }
            }
        }
    }
    //レベルダウン
    public void LevelDown()
    {
        //exp -= 7.0f;
        //level -= 1;
        //speed = speedDefalut;
       StartCoroutine("SutanTime");
    }
    IEnumerator SutanTime()
    {
        currentChaseState = ChaseState.Stop;
        yield return new WaitForSeconds(wait);
        currentChaseState = ChaseState.Normal;
        yield break;
    }
}
