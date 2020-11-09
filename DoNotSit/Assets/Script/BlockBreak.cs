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
            Debug.Log("当たったあ");
            Death();
        }
    }

    void Death()
    {
        Instantiate(effect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
