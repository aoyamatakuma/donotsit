using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplanationText : MonoBehaviour
{
    public GameObject explanationObj;
    Text text;
    [Multiline]
    public string explanationText;
    bool isActive;
    public float activeCnt;
    float cnt;
    // Start is called before the first frame update
    void Start()
    {
        cnt = 0;
        text = explanationObj.transform.GetChild(0).GetComponent<Text>();
        text.text = explanationText;
        explanationObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            cnt += Time.deltaTime;
            if(cnt >= activeCnt)
            {
                explanationObj.SetActive(false);
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player" && !isActive)
        {
            explanationObj.SetActive(true);
            isActive = true;
        }
    }
}
