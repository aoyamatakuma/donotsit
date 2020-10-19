using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleEnemy : MonoBehaviour
{
  void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Player")
        {
            //プレイヤーが死ぬ処理またはプレイヤーの死ぬメソッド
            Destroy(col.gameObject);
        }
    }
}
