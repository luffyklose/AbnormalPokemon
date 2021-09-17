using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] private Text nameText;
    [SerializeField] private Text levelText;
    [SerializeField] private HPBar hpBar;

    private Monster _monster;
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
        _monster = monster;
        nameText.text = monster.Base.Name;
        levelText.text = "Lvl " + monster.Level;
        hpBar.setHP((float)monster.HP/monster.MaxHP);
        hpBar.setHPNumber(monster.HP,monster.MaxHP);
    }

    public IEnumerator UpdateHP()
    {
        yield return hpBar.SetHPSmooth((float)_monster.HP / _monster.MaxHP);
        hpBar.setHPNumber(_monster.HP,_monster.MaxHP);
    }
}
