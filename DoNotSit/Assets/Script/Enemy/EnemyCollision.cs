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

    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShakeScript>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        damageUI.GetComponent<ScoreAddUI>().SetDamage(score);
        damageUI.SetActive(false);
    }
    
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {     
            if (player.currentPlayerState == PlayerState.Attack)
            {
                player.Score(score);
                Damage(col.ClosestPointOnBounds(transform.position));
                Death();
            }
            else
            {
                player.Damage(damage);
            }
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Bomb" || col.gameObject.tag =="BlowAway")
        {
            player.Score(score);
             Damage(col.ClosestPointOnBounds(transform.position));
            Death();
        }
    }

    void Death()
    {
        int num = Random.Range(0, effects.Count);
        camera.Shake(camera.durations, camera.magnitudes);
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
