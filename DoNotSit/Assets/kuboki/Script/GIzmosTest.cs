using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GIzmosTest : MonoBehaviour
{
    //スクリプト取得
    [SerializeField]
    private StageScript stageCS;

    private void OnDrawGizmos()
    {
        stageCS.GetDrawMap();
    }
}
