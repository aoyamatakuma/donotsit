using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    public GameObject damageUI;
    public int damage;
    private CameraShakeScript camera;
    public List<GameObject> effects;
    public float score;
    PlayerControl player;
    Goaltape goal;
    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShakeScript>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        goal = GameObject.FindGameObjectWithTag("Goal").GetComponent<Goaltape>();
        damageUI.SetActive(false);
    }

    void Update()
    {
        if (goal.isGoal)
        {
            for (int i = 0; i < effects.Count; i++)
            {
                effects[i].GetComponent<AudioSource>().enabled = false;
            }
            Death();
        }
    }
    
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {     
            if (player.currentPlayerState == PlayerState.Attack)
            {
                player.Score(score);
                Damage(col.ClosestPointOnBounds(transform.position));
                camera.Shake(camera.durations, camera.magnitudes);
                Death();
            }
            else
            {
                player.Damage(damage);
                Death();
            }
        }
        if (col.gameObject.tag == "ChaseEnemy" )
        {
            Death();
        }
        }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Bomb" || col.gameObject.tag =="BlowAway")
        {
            player.Score(score);
             Damage(col.ClosestPointOnBounds(transform.position));
            camera.Shake(camera.durations, camera.magnitudes);
            Death();
        }
    }

    void Death()
    {
        int num = Random.Range(0, effects.Count);
       // camera.Shake(camera.durations, camera.magnitudes);
        Instantiate(effects[num], transform.position, transform.rotation);
        effects[num].GetComponent<AudioSource>().Play();
        Destroy(gameObject);
    }

    void Damage(Vector3 point)
    {
        damageUI.SetActive(true);
        damageUI.GetComponent<ScoreAddUI>().isActive = true;
        damageUI.GetComponent<ScoreAddUI>().SetCombo(player.combo);
        damageUI.transform.SetParent(null);
        damageUI.transform.position = point + new Vector3(0,0,-6);   
    }


}
