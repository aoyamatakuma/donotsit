using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject effect;
    public float bombRange;
    private CameraShakeScript camera;
    // Start is called before the first frame update
    void Start()
    {
        effect.GetComponent<SphereCollider>().radius = bombRange;
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShakeScript>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player" || col.gameObject.tag == "Bomb")
        {
            camera.Shake(camera.durations, camera.magnitudes);
            Instantiate(effect, transform.position, transform.rotation);
            effect.GetComponent<AudioSource>().Play();
            Destroy(gameObject);
        }
    }

  
}
