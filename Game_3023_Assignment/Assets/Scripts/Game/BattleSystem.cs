using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{

    [SerializeField] private BattleUnit playerUnit;
    [SerializeField] private BattleHUD playerHUD;

    [SerializeField] private BattleUnit enemyUnit;

    [SerializeField] private BattleHUD enemyHUD;
    // Start is called before the first frame update
    void Start()
    {
        SetupBattle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupBattle()
    {
        playerUnit.Setup();
        playerHUD.SetData(playerUnit.Monster);
        
        enemyUnit.Setup();
        enemyHUD.SetData(playerUnit.Monster);
    }
}
