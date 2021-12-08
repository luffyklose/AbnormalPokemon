using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] private Text nameText;
    [SerializeField] private Text levelText;
    [SerializeField] private HPBar hpBar;
    [SerializeField] private GameObject expBar;

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
        SetLevel();
        hpBar.setHP((float)monster.HP/monster.MaxHP);
        hpBar.setHPNumber(monster.HP,monster.MaxHP);
        SetExp();
    }

    public void SetLevel()
    {
        levelText.text = "Lvl " + _monster.Level;
    }

    public void SetExp()
    {
        if (expBar == null)
            return;
        float normalizedExp = GetNormalizedExp();
        expBar.transform.localScale = new Vector3(normalizedExp, 1.0f, 1.0f);
    }
    
    public IEnumerator SetExpSmoothly(bool reset=false)
    {
        if (expBar == null)
            yield break;
        if (reset)
            expBar.transform.localPosition = new Vector3(0.0f, 1.0f, 1.0f);
        float normalizedExp = GetNormalizedExp();
        //Debug.Log($"{normalizedExp}");
        yield return expBar.transform.DOScaleX(normalizedExp, 1.0f).WaitForCompletion();
    }

    float GetNormalizedExp()
    {
        int currLevelExp = _monster.Base.GetExpForLevel(_monster.Level);
        int nextLevelExp = _monster.Base.GetExpForLevel(_monster.Level + 1);
        float normalizedExp = (float)(_monster.Exp - currLevelExp) / (nextLevelExp - currLevelExp);
        //Debug.Log($"{_monster.Exp} {currLevelExp} {nextLevelExp} {normalizedExp}");
        return Mathf.Clamp01(normalizedExp);
    }

    public IEnumerator UpdateHP()
    {
        yield return hpBar.SetHPSmooth((float)_monster.HP / _monster.MaxHP);
        hpBar.setHPNumber(_monster.HP,_monster.MaxHP);
    }
}
