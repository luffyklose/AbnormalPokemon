using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = System.Random;

public enum BattleState
{
    Start,
    PlayerActionSelection,
    PlayerMoveSelection,
    PerformMove,
    EnemyMove,
    PartyScreen,
    BattleOver,
    Busy,
    Notbusy,
}
public class BattleSystem : MonoBehaviour
{

    [SerializeField] private BattleUnit playerUnit;
    [SerializeField] private BattleUnit enemyUnit;
    [SerializeField] private DialogBox dialogBox;
    [SerializeField] private PartyScreen partyScreen;

    private BattleState state;
    private int currentAction;
    private int currentMove;
    private int currentMember;
    private bool isTrainerBattle = false;
    
    private MonsterParty playerParty;
    private Monster wildMonster;
    
    //return true when enemy is defeated, return flase when player is defeated
    public event Action<bool> OnBattleOver;
    
    // Start is called before the first frame update
    public void StartBattle(MonsterParty playerParty,Monster wild)
    {
        this.playerParty = playerParty;
        wildMonster = wild;
        StartCoroutine(SetupBattle());
    }

    // Update is called once per frame
    public void HandleUpdate()
    {
        switch (state)
        {
            case BattleState.PlayerActionSelection:
            {
                HandleActionSelection();
                break;
            }
            case BattleState.PlayerMoveSelection:
            {
                HandleMoveSelection();
                break;
            }
            case BattleState.PartyScreen:
            {
                HandlePartyScreenSelection();
                break;
            }
            default: break;
        }
    }

    public IEnumerator SetupBattle()
    {
        currentAction = 0;
        currentMove = 0;
        
        playerUnit.Setup(playerParty.GetFirstHealthyMonster());
        enemyUnit.Setup(wildMonster);
        partyScreen.Init();
        
        dialogBox.SetMoveNames(playerUnit.Monster.Moves);
        
        yield return dialogBox.TypeDialog($"A wild {enemyUnit.Monster.Base.Name} appears!");

        PlayerActionSelection();
    }

    void PlayerActionSelection()
    {
        state = BattleState.PlayerActionSelection;
        dialogBox.SetDialog("Choose an action");
        dialogBox.EnableActionSelector(true);
    }

    void PlayerMoveSelection()
    {
        state = BattleState.PlayerMoveSelection;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
    }

    IEnumerator PlayerMove()
    {
        state = BattleState.PerformMove;
        Move move = playerUnit.Monster.Moves[currentMove];
        yield return RunMove(playerUnit, enemyUnit, move);
        
        if(state==BattleState.PerformMove)
            StartCoroutine(EnemyMove());
    }

    IEnumerator EnemyMove()
    {
        state = BattleState.PerformMove;
        Move move = enemyUnit.Monster.GetWeakMove(playerUnit.Monster);
        yield return RunMove(enemyUnit, playerUnit, move);
       
        if(state==BattleState.PerformMove)
            PlayerActionSelection();
    }

    IEnumerator RunMove(BattleUnit sourceUnit, BattleUnit targetUnit, Move move)
    {
        move.PP--;
        yield return dialogBox.TypeDialog($"{sourceUnit.Monster.Base.Name} used {move.Base.Name}");
        sourceUnit.PlayAttackAnimation();
        targetUnit.PlayHitAnimation();
        DamageDetails damageDetails = targetUnit.Monster.TakeDamage(move, sourceUnit.Monster);

        yield return targetUnit.HUD.UpdateHP();
        yield return ShowDamageDetails(damageDetails);
        
        if (damageDetails.Defeated)
        {
            yield return HandleMonsterDefeated(targetUnit);
        }
    }

    void CheckForBattleOver(BattleUnit faintedUnit)
    {
        if (faintedUnit.IsPlayerUnit)
        {
            Monster nextMonster = playerParty.GetFirstHealthyMonster();
            if (nextMonster != null)
            {
                OpenPartyScreen();
            }
            else
            {
                BattleOver(false);
            }
        }
        else
        {
            BattleOver(true);
        }
    }

    void BattleOver(bool isPlayerWin)
    {
        state = BattleState.BattleOver;
        OnBattleOver(isPlayerWin);
    }

    IEnumerator HandleMonsterDefeated(BattleUnit defeatedUnit)
    {
        yield return dialogBox.TypeDialog($"{defeatedUnit.Monster.Base.Name} is defeated!");
        defeatedUnit.PlayDefeatAnimation();
        yield return new WaitForSeconds(2.0f);

        if (!defeatedUnit.IsPlayerUnit)
        {
            int expYield = defeatedUnit.Monster.Base.ExpYield;
            int enemyLevel = defeatedUnit.Monster.Level;
            float trainerBonus = isTrainerBattle ? 1.5f : 1.0f;
            int expGain = Mathf.FloorToInt((expYield * enemyLevel * trainerBonus) / 7);
            //Debug.Log($"{expYield}*{enemyLevel}*{trainerBonus}");
            //Debug.Log($"{playerUnit.Monster.Exp} + {expGain} =");
            playerUnit.Monster.Exp += expGain;
            //Debug.Log($"{playerUnit.Monster.Exp}");
            yield return dialogBox.TypeDialog($"{playerUnit.Monster.Base.name} gained {expGain} exp");
            yield return playerUnit.HUD.SetExpSmoothly();

            while (playerUnit.Monster.CheckForLevelUp())
            {
                playerUnit.HUD.SetLevel();
                yield return dialogBox.TypeDialog($"{playerUnit.Monster.Base.name} grew to {playerUnit.Monster.Level}");
                LearnableMove newMove = playerUnit.Monster.GetLearnableMoveAtCurrentLevel();
                if (newMove != null)
                {
                    if (playerUnit.Monster.Moves.Count < MonsterBase.MAXMOVESNUM)
                    {
                        playerUnit.Monster.LearnMove(newMove);
                        yield return
                            dialogBox.TypeDialog($"{playerUnit.Monster.Base.name} learned {newMove.Base.name}");
                        dialogBox.SetMoveNames(playerUnit.Monster.Moves);
                    }
                    else
                    {
                        
                    }
                }
                
                yield return playerUnit.HUD.SetExpSmoothly();
            }
        }
        
        CheckForBattleOver(defeatedUnit);
    }

    private void OpenPartyScreen()
    {
        state = BattleState.PartyScreen;
        partyScreen.SetPartyData(playerParty.Monsters);
        partyScreen.gameObject.SetActive(true);
    }

    void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentAction == 0 || currentAction == 1)
            {
                currentAction += 2;
            }
            else
            {
                currentAction -= 2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentAction == 0 || currentAction == 1)
            {
                currentAction += 2;
            }
            else
            {
                currentAction -= 2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentAction == 0 || currentAction == 2)
            {
                currentAction += 1;
            }
            else
            {
                currentAction -= 1;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentAction == 0 || currentAction == 2)
            {
                currentAction += 1;
            }
            else
            {
                currentAction -= 1;
            }
        }
        
        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentAction == 0)
            {
                //Fight
                PlayerMoveSelection();
                dialogBox.EnableActionSelector(false);
            }
            else if (currentAction == 1)
            {
                //Win randomly
                StartCoroutine(CheckStruggle());
            }
            else if (currentAction == 2)
            {
                //Change monster
                OpenPartyScreen();
            }
            else if (currentAction == 3)
            {
                //Run
                StartCoroutine(CheckEscape());
            }
        }
    }

   void HandleMoveSelection()
   {
       if (Input.GetKeyDown(KeyCode.RightArrow))
           ++currentMove;
       else if (Input.GetKeyDown(KeyCode.LeftArrow))
           --currentMove;
       else if (Input.GetKeyDown(KeyCode.DownArrow))
           currentMove += 2;
       else if (Input.GetKeyDown(KeyCode.UpArrow))
           currentMove -= 2;

       currentMove = Mathf.Clamp(currentMove, 0, playerUnit.Monster.Moves.Count - 1);
        
       dialogBox.UpdateMoveSelection(currentMove,playerUnit.Monster.Moves[currentMove]);

       if (Input.GetKeyDown(KeyCode.Space))
       {
           dialogBox.EnableMoveSelector(false);
           dialogBox.EnableDialogText(true);
           StartCoroutine(PlayerMove());
       }
       else if (Input.GetKeyDown(KeyCode.B))
       {
           dialogBox.EnableMoveSelector(false);
           dialogBox.EnableDialogText(true);
           PlayerActionSelection();
       }
   }

   void HandlePartyScreenSelection()
   {
       if (Input.GetKeyDown(KeyCode.RightArrow))
           ++currentMember;
       else if (Input.GetKeyDown(KeyCode.LeftArrow))
           --currentMember;
       else if (Input.GetKeyDown(KeyCode.DownArrow))
           currentMember += 2;
       else if (Input.GetKeyDown(KeyCode.UpArrow))
           currentMember -= 2;

       currentMember = Mathf.Clamp(currentMember, 0, playerParty.Monsters.Count - 1);

       partyScreen.UpdateMemberSelection(currentMember);

       if (Input.GetKeyDown(KeyCode.Space))
       {
           Monster selectedMember = playerParty.Monsters[currentMember];
           if (selectedMember.HP <= 0)
           {
               partyScreen.SetMessage("Cannot send a fainted monster");
               return;
           }

           if (selectedMember == playerUnit.Monster)
           {
               partyScreen.SetMessage("Cannot switch to same monster");
               return;
           }

           partyScreen.gameObject.SetActive(false);
           state = BattleState.Busy;
           StartCoroutine(SwitchMonster(selectedMember));

       }
       else if (Input.GetKeyDown(KeyCode.B))
       {
           partyScreen.gameObject.SetActive(false);
           PlayerActionSelection();
       }
   }

   IEnumerator SwitchMonster(Monster newMonster)
   {
       if (playerUnit.Monster.HP > 0)
       {
           yield return dialogBox.TypeDialog($"Come back {playerUnit.Monster.Base.name}!");
          playerUnit.PlayDefeatAnimation();
          yield return new WaitForSeconds(2.0f);
       }
       playerUnit.Setup(newMonster);
       playerUnit.HUD.SetData(newMonster);
       dialogBox.SetMoveNames(newMonster.Moves);
       yield return dialogBox.TypeDialog($"Go {newMonster.Base.name}!");

       StartCoroutine(EnemyMove());
   }

   IEnumerator ShowDamageDetails(DamageDetails damageDetails)
   {
       Debug.Log($"{damageDetails.TypeEffectiveness}");
       if (damageDetails.TypeEffectiveness > 1f)
       {
           yield return dialogBox.TypeDialog("It's super effective");
       }
       else if (damageDetails.TypeEffectiveness < 1f)
       {
           yield return dialogBox.TypeDialog("It's not effective");
       }
   }

   IEnumerator CheckStruggle()
   {
       if (UnityEngine.Random.Range(0.0f, 1.0f) <= 0.1f)
       {
           yield return dialogBox.TypeDialog("Struggle Succeed");
           BattleOver(true);
       }
       else
       {
           yield return dialogBox.TypeDialog("Struggle Failed");
           StartCoroutine(EnemyMove());
       }
   }
   
   IEnumerator CheckEscape()
   {
       if (UnityEngine.Random.Range(0.0f, 1.0f) <= 0.6f)
       {
           yield return dialogBox.TypeDialog("Escape Succeed");
           BattleOver(true);
       }
       else
       {
           yield return dialogBox.TypeDialog("Escape Failed");
           StartCoroutine(EnemyMove());
       }
   }
}
