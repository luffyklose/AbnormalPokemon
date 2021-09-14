using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    [SerializeField] private GameObject health;
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
}
