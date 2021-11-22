using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public enum BattleState
{
    Start,
    PlayerActionSelection,
    PlayerMoveSelection,
    PlayerMove,
    EnemyMove,
    Busy
}
public class BattleSystem : MonoBehaviour
{

    [SerializeField] private BattleUnit playerUnit;
    [SerializeField] private BattleHUD playerHUD;
    [SerializeField] private BattleUnit enemyUnit;
    [SerializeField] private BattleHUD enemyHUD;
    [SerializeField] private DialogBox dialogBox;
    [SerializeField] private List<Monster> monsterList;
    
    private BattleState state;
    private int currentAction;
    private int currentMove;
    
    //return true when enemy is defeated, return flase when player is defeated
    public event Action<bool> OnBattleOver;
    
    // Start is called before the first frame update
    public void StartBattle()
    {
        int temp = UnityEngine.Random.Range(0, monsterList.Count - 1);
        enemyUnit.SetMonster(monsterList[temp].Base);
        StartCoroutine(SetupBattle());
    }

    // Update is called once per frame
    public void HandleUpdate()
    {
        if (state == BattleState.PlayerActionSelection)
        {
            HandleActionSelection();
        }
        else if (state == BattleState.PlayerMoveSelection)
        {
            HandleMoveSelection();
        }
    }

    public IEnumerator SetupBattle()
    {
        playerUnit.Setup();
        playerHUD.SetData(playerUnit.Monster);
        
        enemyUnit.Setup();
        enemyHUD.SetData(enemyUnit.Monster);
        
        dialogBox.SetMoveNames(playerUnit.Monster.Moves);
        
        yield return dialogBox.TypeDialog($"A wild {enemyUnit.Monster.Base.Name} appears!");

        PlayerAction();
    }

    void PlayerAction()
    {
        state = BattleState.PlayerActionSelection;
        StartCoroutine(dialogBox.TypeDialog("Choose an action"));
        dialogBox.EnableActionSelector(true);
    }

    void PlayerMove()
    {
        state = BattleState.PlayerMoveSelection;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
    }

    IEnumerator PerfomPlayerMove()
    {
        state = BattleState.PlayerMove;
        var move = playerUnit.Monster.Moves[currentMove];
        move.PP--;
        yield return dialogBox.TypeDialog($"{playerUnit.Monster.Base.Name} used {move.Base.Name}");
        playerUnit.PlayAttackAnimation();
        enemyUnit.PlayHitAnimation();
        DamageDetails damageDetails = enemyUnit.Monster.TakeDamage(move, playerUnit.Monster);
        yield return enemyHUD.UpdateHP();
        yield return ShowDamageDetails(damageDetails);
        
        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{enemyUnit.Monster.Base.Name} is defeated!");

            yield return new WaitForSeconds(2.0f);
            enemyUnit.PlayDefeatAnimation();
            yield return new WaitForSeconds(0.5f);
            OnBattleOver(true);
        }
        else
        {
            StartCoroutine(PerformEnemyMove());
        }
    }

    IEnumerator PerformEnemyMove()
    {
        state = BattleState.EnemyMove;
        var move = enemyUnit.Monster.GetRandomMove();
        move.PP--;
        yield return dialogBox.TypeDialog($"{enemyUnit.Monster.Base.Name} used {move.Base.Name}");
        enemyUnit.PlayAttackAnimation();
        playerUnit.PlayHitAnimation();
        DamageDetails damageDetails = playerUnit.Monster.TakeDamage(move, enemyUnit.Monster);
        yield return playerHUD.UpdateHP();
        yield return ShowDamageDetails(damageDetails);
        
        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{playerUnit.Monster.Base.Name} is defeated!");
            
            yield return new WaitForSeconds(2.0f);
            playerUnit.PlayDefeatAnimation();
            yield return new WaitForSeconds(0.5f);
            OnBattleOver(false);
        }
        else
        {
            PlayerAction();
        }
    }
    
    IEnumerator Escape()
    {
        state = BattleState.Busy;
        Debug.Log("Paolema");
        yield return dialogBox.TypeDialog("Run Away!");
        OnBattleOver(true);
        Debug.Log("Paole");
    }

    void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentAction < dialogBox.getActionNumber() - 1)
            {
                ++currentAction;
            }
            else
            {
                currentAction = 0;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentAction > 0)
            {
                --currentAction;
            }
            else
            {
                currentAction = dialogBox.getActionNumber() - 1;
            }
        }
        
        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentAction == 0)
            {
                //Fight
                Debug.Log("Choose Action");
                PlayerMove();
                dialogBox.EnableActionSelector(false);
            }
            else if (currentAction == 1)
            {
                //Run
                //Debug.Log("Paolema");
                OnBattleOver(true);
                //Escape();
            }
        }
    }

   void HandleMoveSelection()
   {
       if (Input.GetKeyDown(KeyCode.RightArrow))
       {
           if (currentMove < playerUnit.Monster.Moves.Count - 1)
           {
               ++currentMove;
           }
           else
           {
               currentMove = 0;
           }
       }
       else if (Input.GetKeyDown(KeyCode.LeftArrow))
       {
           if (currentMove > 0)
           {
               --currentMove;
           }
           else
           {
               currentMove = playerUnit.Monster.Moves.Count - 1;
           }
       }
        
       dialogBox.UpdateMoveSelection(currentMove,playerUnit.Monster.Moves[currentMove]);

       if (Input.GetKeyDown(KeyCode.Space))
       {
           Debug.Log("Choose Move");
           dialogBox.EnableMoveSelector(false);
           dialogBox.EnableDialogText(true);
           StartCoroutine(PerfomPlayerMove());
       }
   }

   IEnumerator ShowDamageDetails(DamageDetails damageDetails)
   {
       if (damageDetails.TypeEffectiveness > 1f)
       {
           yield return dialogBox.TypeDialog("It's super effective");
       }
       else
       {
           yield return dialogBox.TypeDialog("It's not effective");
       }
   }
}
