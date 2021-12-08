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

    public void Interact(Transform init)
    {
        if (state == NPCStates.Idle)
        {
            state = NPCStates.Conversation;
            characterBehaviour.LookAtPlayer(init.position);
            
            StartCoroutine(DialogManager.Instance.ShowDialog(dialog, () => {
                idleTimer = 0f;
                state = NPCStates.Idle;
            }));
        }
    }

    private void Update()
    {
        if (DialogManager.Instance.IsInConverse) return;

        if (state == NPCStates.Idle)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer > moveInterval)
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

        var prePos = transform.position;

        yield return characterBehaviour.Move(movements[currentMove]);
        
        if (transform.position != prePos)
            currentMove = (currentMove + 1) % movements.Count;
        
        state = NPCStates.Idle;
    }
}
