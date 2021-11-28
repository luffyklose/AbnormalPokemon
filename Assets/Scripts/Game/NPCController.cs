using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] private Dialog dialog;
    [SerializeField] private List<Vector2> movements;

    [SerializeField] private float moveInterval;
    
    private NPCStates state;
    private float idleTimer;
    private int currentMove;
    
    private CharacterBehaviour characterBehaviour;

    private void Awake()
    {
        characterBehaviour = GetComponent<CharacterBehaviour>();
    }

    public void Interact()
    {
        if (state == NPCStates.Idle) 
            StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
        //StartCoroutine(characterBehaviour.Move(new Vector2(0, 2)));
    }

    private void Update()
    {
        if (DialogManager.Instance.IsInConverse)
            return;

        if (state == NPCStates.Idle)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= moveInterval)
            {
                idleTimer = 0f;
                if (movements.Count > 0)
                    StartCoroutine(Walk());
            }
        }
        characterBehaviour.HandleUpdate();
    }

    IEnumerator Walk()
    {
        state = NPCStates.Walking;

        yield return characterBehaviour.Move(movements[currentMove]);
        currentMove = (currentMove + 1) % movements.Count;
        
        state = NPCStates.Idle;
    }
}
