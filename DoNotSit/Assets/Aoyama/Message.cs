using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Message : MonoBehaviour
{

    [SerializeField]
    private GameObject MessagePrefab;
    private GameObject MessageInstance;
    public bool isChaseEnemy;
    bool isActive;

    // Start is called before the first frame update
    void Start()
    {
      //  Destroy(MessagePrefab);
        
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    void ActiveText()
    {
        //explanationObj.SetActive(true);
        //text.text = explanationText;
        MessageInstance = GameObject.Instantiate(MessagePrefab) as GameObject;
        isActive = true;
    }



    void OnTriggerEnter(Collider col)
    {
        if (!isChaseEnemy)
        {
            if (col.gameObject.tag == "Player" && !isActive)
            {
                ActiveText();
            }
        }
        else
        {
            if(col.gameObject.tag == "ChaseEnemy" && !isActive)
            {
                ActiveText();
            }
        }
      
    }
}
