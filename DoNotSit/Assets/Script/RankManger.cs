using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankManger : MonoBehaviour
{
    public float S;
    public float A;
    public float B;
    public static string rank;
    private float playerScore;
    // Update is called once per frame
    void Update()
    {
        playerScore = PlayerControl.scoreNumber;
        if (playerScore >= S)//Sランク
        {
            rank = "S";
        }
        if (playerScore < S)//Aランク
        {
            rank = "A";
        }
        if (playerScore <= A)//Bランク
        {
            rank = "B";
        }
        if (playerScore <= B)//Cランク
        {
            rank = "C";
        }
       // Debug.Log(rank);
    }
    public static string ClearRank()
    {
        return rank;
    }
}
