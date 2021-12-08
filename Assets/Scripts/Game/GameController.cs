using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    World,
    Battle,
    Dialog
}
public class GameController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private BattleSystem battleSystem;
    [SerializeField] private Camera worldCamera;

    private TrainerController trainer;
    private GameState state;
    public static GameController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerController.onEncountered += StartBattle;
        battleSystem.OnBattleOver += EndBattle;

        playerController.onTrainerEncounter += (Collider2D collider) =>
        {
            var trainer = collider.GetComponentInParent<TrainerController>();

            if (trainer != null)
            {
                state = GameState.Battle;
                StartCoroutine(trainer.StartBattle(playerController));
            }
        };

        DialogManager.Instance.OnShowDialog += () =>
        {
            state = GameState.Dialog;
        };
        
        DialogManager.Instance.OnCloseDialog += () =>
        {
            if (state == GameState.Dialog)
            {
                state = GameState.World;
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (state == GameState.World)
        {
            playerController.HandleUpdate();
        }
        else if (state == GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
        else if (state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }
    }

    void StartBattle()
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);
        MonsterParty playerParty = playerController.GetComponent<MonsterParty>();
        Monster wildMonster = FindObjectOfType<MapArea>().GetComponent<MapArea>().GetRandomWildMonster();
        battleSystem.StartBattle(playerParty, wildMonster);
    }

    public void StartTrainerBattle(TrainerController trainer)
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        this.trainer = trainer;
        
        MonsterParty playerParty = playerController.GetComponent<MonsterParty>();
        MonsterParty trainerParty = trainer.GetComponent<MonsterParty>();
        
        battleSystem.StartTrainerBattle(playerParty, trainerParty);
    }
    
    void EndBattle(bool isEnemyDefeated)
    {
        if (trainer != null && isEnemyDefeated == true)
        {
            trainer.BattleLost();
            trainer = null;
        }

        state = GameState.World;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
        if (!isEnemyDefeated)
        {
            FindObjectOfType<GameMenu>().Load();
        }
    }
}
