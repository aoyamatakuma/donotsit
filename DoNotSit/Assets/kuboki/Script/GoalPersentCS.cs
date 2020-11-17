using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalPersentCS : MonoBehaviour
{
    public GameObject GoalObj;
    public GameObject player;
    private Vector3 startPos,nowPlayPos,goalPos;
    Slider sli;
    float PtoG,GtoS;


    // Start is called before the first frame update
    void Start()
    {
        startPos = player.transform.position;
        goalPos = GoalObj.transform.position;
        sli = GetComponent<Slider>();
        GtoS = Vector3.Distance(startPos, goalPos);
        sli.value = 0;

    }

    // Update is called once per frame
    void Update()
    {
        nowPlayPos = player.transform.position;
        PtoG = Vector3.Distance(nowPlayPos, goalPos);
        float ag =(GtoS - PtoG) / GtoS*100;
        sli.value = ag;
    }



}
