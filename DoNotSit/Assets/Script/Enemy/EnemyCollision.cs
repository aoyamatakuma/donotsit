using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    public float damage;
    public GameObject effect;
 

    void Start()
    {
    }

   

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            PlayerControl player = col.gameObject.GetComponent<PlayerControl>();
            if (player.currentPlayerState == PlayerState.Attack)
            {
                Death();
            }
            else
            {
                //player.Damage(damage);
            }
        }
    }

    void Death()
    {
        
        Instantiate(effect, transform.position, transform.rotation);
        effect.GetComponent<AudioSource>().Play();
        Destroy(gameObject);
    }


}
