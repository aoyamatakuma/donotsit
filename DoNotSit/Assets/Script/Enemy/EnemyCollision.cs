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
        damageUI.GetComponent<ScoreAddUI>().SetScore(score);
        damageUI.SetActive(false);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
           
            if (player.currentPlayerState == PlayerState.Attack)
            {
                Damage(col);
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
            Damage(col);
            Death();
        }
    }

    void Death()
    {
        player.Score(score);
        int num = Random.Range(0, effects.Count);
        camera.Shake(camera.durations, camera.magnitudes);
        Instantiate(effects[num], transform.position, transform.rotation);
        effects[num].GetComponent<AudioSource>().Play();
        Destroy(gameObject);
    }

    void Damage(Collider col)
    {
        damageUI.SetActive(true);
        damageUI.GetComponent<ScoreAddUI>().SetCombo(player.combo + 1);
        damageUI.transform.position = col.bounds.center - Camera.main.transform.forward * 1f;
        damageUI.transform.SetParent(null);
        
    }


}
