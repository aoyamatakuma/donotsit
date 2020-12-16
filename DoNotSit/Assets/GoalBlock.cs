using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBlock : MonoBehaviour
{
    public GameObject effect;
    private Goal goal;
    void Start()
    {
        goal = GameObject.Find("Goal").GetComponent<Goal>();
    }
        void Update()
    {
       if(goal.isbreak==true)
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
