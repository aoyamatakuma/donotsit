using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeGauge : MonoBehaviour
{
    //　ライフゲージプレハブ
    [SerializeField]
    private GameObject lifeObj;

    //　ライフゲージ全削除＆HP分作成
    public void SetLifeGauge(int life)
    {
        
        //　現在の体力数分のライフゲージを作成
        for (int i = 0; i < life-1; i++)
        {
            Instantiate<GameObject>(lifeObj, transform);
        }
    }
    //　ダメージ分だけ削除
    public void SetLifeGauge2(int damage)
    {
        for (int i = 0; i < damage; i++)
        {
            //　最後のライフゲージを削除
            Destroy(transform.GetChild(i).gameObject);
            //Destroy(transform.GetChild(transform.childCount - 1 - i).gameObject);
        }
    }
}
