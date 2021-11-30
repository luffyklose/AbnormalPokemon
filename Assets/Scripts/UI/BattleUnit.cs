using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] private MonsterBase _base;
    [SerializeField] private int level;
    [SerializeField] private bool isPlayerUnit;
    [SerializeField] private BattleHUD hud;
    
    public Monster Monster { get; set; }

    private Image image;
    private Vector3 originalPos;
    private Color originalColor;

    public bool IsPlayerUnit
    {
        get { return isPlayerUnit; }
    }

    public BattleHUD HUD
    {
        get { return hud; }
    }
    
    private void Awake()
    {
        image = GetComponent<Image>();
        originalPos = image.transform.localPosition;
        originalColor = image.color;
    }

    public void SetMonster(MonsterBase monsterBase)
    {
        _base = monsterBase;
    }

    public void Setup(Monster monster)
    {
        Monster = monster;
        if (isPlayerUnit)
        {
            image.sprite = Monster.Base.BackSprite;
        }
        else
        {
            image.sprite = Monster.Base.FrontSprite;
        }

        hud.gameObject.SetActive(true);
        hud.SetData(monster);
        image.color = originalColor;
        PlayerEnterAnimation();
    }

    public void DisableHUD()
    {
        hud.gameObject.SetActive(false);  
    }

    public void PlayerEnterAnimation()
    {
        if (isPlayerUnit)
        {
            image.transform.localPosition = new Vector3(-500.0f, originalPos.y);
        }
        else
        {
            image.transform.localPosition = new Vector3(500.0f, originalPos.y);
        }

        image.transform.DOLocalMoveX(originalPos.x, 1f);
    }

    public void PlayAttackAnimation()
    {
        var sequence = DOTween.Sequence();
        if (isPlayerUnit)
        {
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x + 50.0f, 0.25f));
        }
        else
        {
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x - 50.0f, 0.25f));
        }

        sequence.Append(image.transform.DOLocalMoveX(originalPos.x, 0.25f));
    }
    
    public void PlayHitAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.gray,0.1f));
        sequence.Append(image.DOColor(originalColor,0.1f));
    }

    public void PlayDefeatAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.transform.DOLocalMoveY(originalPos.y - 150.0f, 0.5f));
        sequence.Join(image.DOFade(0.0f, 0.5f));
    }
}
