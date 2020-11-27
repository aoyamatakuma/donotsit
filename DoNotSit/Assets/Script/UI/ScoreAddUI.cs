using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreAddUI : MonoBehaviour
{
    private Transform targetObject;
    public Text damageText;
    public Text comboText;
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

    //public void SetPosition(Vector3 point)
    //{
    //    damageText.rectTransform.localPosition
    //       = RectTransformUtility.WorldToScreenPoint(Camera.main, point);
    //    comboText.rectTransform.localPosition
    //       = RectTransformUtility.WorldToScreenPoint(Camera.main, point+ new Vector3(0,5,0));
    //}

    public void SetDamage(float damage)
    {
        damageText.text ="+"+ damage.ToString();
    }

    public void SetCombo(float combo)
    {
        comboText.text = combo.ToString() + "コンボ!";
    }
}
