using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] private MonsterBase _base;
    [SerializeField] private int level;
    [SerializeField] private bool isPlayerUnit;
    
    public Monster Monster { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup()
    {
        Monster = new Monster(_base, level);
        if (isPlayerUnit)
        {
            GetComponent<Image>().sprite = Monster.Base.BackSprite;
        }
        else
        {
            GetComponent<Image>().sprite = Monster.Base.FrontSprite;
        }
    }
}
