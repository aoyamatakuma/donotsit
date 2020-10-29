using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    public List<GameObject> count;
    public PlayerControl player;
    public GameObject hpObj;
    public GameObject scObj;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CountCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CountCoroutine()
    {
        player.enabled = false;
        hpObj.SetActive(false);
        scObj.SetActive(false);
        for (int i = 0; i < count.Count; i++) 
        {
            count[i].SetActive(true);
            yield return new WaitForSeconds(1.0f);
            count[i].SetActive(false);
        }
        player.enabled = true;
        hpObj.SetActive(true);
        scObj.SetActive(true);
    }
}
