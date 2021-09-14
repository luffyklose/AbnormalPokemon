using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] private Text nameText;

    [SerializeField] private Text levelText;

    [SerializeField] private HPBar hpBar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetData(Monster monster)
    {
        nameText.text = monster.Base.Name;
        levelText.text = "Lvl " + monster.Level;
        hpBar.setHP((float)monster.HP/monster.MaxHP);
    }
}
