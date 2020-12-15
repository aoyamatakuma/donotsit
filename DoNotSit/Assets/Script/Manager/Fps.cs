using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fps : MonoBehaviour
{
    static int targetFrameRate;
    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = 60; //FPSを60に設定 
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
