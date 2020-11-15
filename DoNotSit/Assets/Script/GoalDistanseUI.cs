using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDistanseUI : MonoBehaviour
{
    public GameObject playerUI;
    public GameObject goalUI;
    GameObject player;
    GameObject goal;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        goal = GameObject.FindGameObjectWithTag("Goal");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
