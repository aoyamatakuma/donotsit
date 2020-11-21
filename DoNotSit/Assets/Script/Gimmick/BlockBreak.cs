using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBreak : MonoBehaviour
{
    public GameObject effect;
    
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "ChaseEnemy")
        { 
            Death();
        }
    }

    void Death()
    {
        Instantiate(effect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
