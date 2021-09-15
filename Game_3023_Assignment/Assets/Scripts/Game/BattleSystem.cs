using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState
{
    Start,
    PlayerAction,
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
    
    private BattleState state;
    private int currentAction;
    private int currentMove;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetupBattle());
    }

    // Update is called once per frame
    void Update()
    {
        if (state == BattleState.PlayerAction)
        {
            HandleActionSelection();
        }
        else if (state == BattleState.PlayerMove)
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
        yield return new WaitForSeconds(1.0f);

        PlayerAction();
    }

    void PlayerAction()
    {
        state = BattleState.PlayerAction;
        StartCoroutine(dialogBox.TypeDialog("Choose an action"));
        dialogBox.EnableActionSelector(true);
    }

    void PlayerMove()
    {
        state = BattleState.PlayerMove;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
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
                PlayerMove();
            }
            else if (currentAction == 1)
            {
                //Run
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
           if (currentMove == 0)
           {
               
           }
           else if (currentMove == 1)
           {
               
           }
       }
   }
}
