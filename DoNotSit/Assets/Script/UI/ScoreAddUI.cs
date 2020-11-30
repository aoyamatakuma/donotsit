using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreAddUI : MonoBehaviour
{
    private Transform targetObject;
    public TextMeshProUGUI comboText;
    public bool isActive;
    [SerializeField]
    private float lifeTime;
    float cnt;

   
    void Start()
    {
        targetObject = transform.parent;
    }

    void Update()
    {
        if (isActive)
        {
            cnt+=Time.deltaTime;
            if(lifeTime < cnt)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetCombo(float combo)
    {
        comboText.text = combo.ToString() + "Combo!";
    }
}
