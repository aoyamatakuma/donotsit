using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Direction_Forward : MonoBehaviour
{
    bool isBurst;
    public float burstSpeed;
    private CameraShakeScript camera;
    public List<GameObject> effects;
    Renderer render;

    void Start()
    {
        isBurst = false;
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShakeScript>();
        render = GetComponent<Renderer>();
    }

    void Update()
    {
        Move();
        if (!render.isVisible && isBurst)
        {
            Death();
        }
    }

    void Move()
    {
        if (!isBurst)
        {
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(camera.transform.position.x,
            camera.transform.position.y,
            camera.transform.position.z), burstSpeed);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            PlayerControl player = col.gameObject.GetComponent<PlayerControl>();
            if (player.currentPlayerState == PlayerState.Attack)
            {
                isBurst = true;
                camera.Shake(camera.durations, camera.magnitudes);
            }
        }

        if (col.gameObject.tag == "Bomb")
        {
            isBurst = true;
            camera.Shake(camera.durations, camera.magnitudes);
        }

    }
   

    void Death()
    {
        //int num = Random.Range(0, effects.Count);
        //Instantiate(effects[num], transform.position, transform.rotation);
        //effects[num].GetComponent<AudioSource>().Play();
        Destroy(gameObject);
    }
}
