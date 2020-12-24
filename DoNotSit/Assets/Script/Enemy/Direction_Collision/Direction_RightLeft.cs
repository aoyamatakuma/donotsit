using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Direction_RightLeft : MonoBehaviour
{
    public GameObject damageUI;
    bool isBurst;
    public float maxBurstSpeed;
    public float minBurstSpeed;
    private float burstSpeed;
    private CameraShakeScript camera;
    public List<GameObject> effects;
    Vector3 dir;
    GameObject playerObj;
    PlayerControl player;
    public float score;
    bool isTouch;
    Goaltape goal;
    void Start()
    {
        isBurst = false;
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShakeScript>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        goal = GameObject.FindGameObjectWithTag("Goal").GetComponent<Goaltape>();
        burstSpeed = Random.Range(minBurstSpeed, maxBurstSpeed);
        RandomDirection();
      
       damageUI.SetActive(false);
      
    }

    void Update()
    {
        if (goal.isGoal)
        {
            for(int i = 0; i < effects.Count; i++)
            {
                effects[i].GetComponent<AudioSource>().enabled = false;
            }
            Death();
        }
        Move();
    }

 
    void Move()
    {
        if (!isBurst)
        {
            return;
        }
        //transform.position += (transform.right+dir)*burstSpeed * Time.deltaTime;
        transform.position += new Vector3(-burstSpeed * Time.deltaTime,1,0);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" && !isBurst)
        {
            if (player.currentPlayerState == PlayerState.Attack)
            {
                isBurst = true;
                player.Score(score);
                Damage(col.ClosestPointOnBounds(transform.position));
                camera.Shake(camera.durations, camera.magnitudes);
            }
            else
            {
                Death();
            }
        }

        if (col.gameObject.tag == "Bomb" || col.gameObject.tag == "BlowAway")
        {
            isBurst = true;
            Damage(col.ClosestPointOnBounds(transform.position));
            camera.Shake(camera.durations, camera.magnitudes);
        }

        if ((col.gameObject.tag == "Wall" || col.gameObject.tag == "Enemy" || col.gameObject.tag == "ChaseEnemy") && isBurst && !isTouch)
        {   
            Death();
            isTouch = true;
        }

        if(col.gameObject.tag == "ChaseEnemy")
        {
            Death();
        }
    }

    void Death()
    {
      
        int num = Random.Range(0, effects.Count);
        Instantiate(effects[num], transform.position, transform.rotation);
        effects[num].GetComponent<AudioSource>().Play();
        Destroy(gameObject);
    }

    void RandomDirection()
    {
        float num = Random.Range(0f, 1f);
        dir = new Vector3(0f, num, 0f);
    }

    void Damage(Vector3 point)
    {
        damageUI.SetActive(true);
        damageUI.GetComponent<ScoreAddUI>().isActive = true;
        damageUI.GetComponent<ScoreAddUI>().SetCombo(player.combo);
        damageUI.transform.SetParent(null);
        damageUI.transform.position = point + new Vector3(0,0,-6);
        damageUI.transform.localEulerAngles = Vector3.zero;
    }


}
