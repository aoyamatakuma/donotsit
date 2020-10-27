using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    public float damage;
    private CameraShakeScript camera;
    public List<GameObject> effects;

    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShakeScript>();
      
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
                player.Damage(damage);
            }
        }
    }

    void Death()
    {
        int num = Random.Range(0, effects.Count);
        Debug.Log(num);
        camera.Shake(camera.durations, camera.magnitudes);
        Instantiate(effects[num], transform.position, transform.rotation);
        effects[num].GetComponent<AudioSource>().Play();
        Destroy(gameObject);
    }


}
