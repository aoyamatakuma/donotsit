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
    float a;
    public float permeateSpeed;
   
    void Start()
    {
        targetObject = transform.parent;
        a = comboText.color.a;
    }

    void Update()
    {
        if (isActive)
        {
            cnt+=Time.deltaTime;
            if(lifeTime < cnt)
            {
                Permeate();
            }
        }
    }

    void Permeate()
    {
        comboText.color = new Color(comboText.color.r, comboText.color.g, comboText.color.b, a);
        a -= Time.deltaTime * permeateSpeed;
        if (a < 0)
        {
             Destroy(gameObject);
        }
    }

    public void SetCombo(float combo)
    {
        comboText.text = combo.ToString() + "Combo!";
    }
}
