using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField] private GameObject health;
    [SerializeField] private Text HpText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setHP(float HPPercentage)
    {
        health.transform.localScale = new Vector3(HPPercentage, 1.0f);
    }

    public void setHPNumber(int Hp, int MaxHp)
    {
        HpText.text = Hp + "/" + MaxHp;
    }
    
    public IEnumerator SetHPSmooth(float newHpPercent)
    {
        float CurHPPercent = health.transform.localScale.x;
        float changeAmt = CurHPPercent - newHpPercent;

        while (CurHPPercent - newHpPercent > Mathf.Epsilon)
        {
            CurHPPercent -= changeAmt * Time.deltaTime;
            health.transform.localScale = new Vector3(CurHPPercent, 1f);
            //Debug.Log(CurHPPercent);
            yield return null;
        }
        health.transform.localScale = new Vector3(newHpPercent, 1f);
    }
}
