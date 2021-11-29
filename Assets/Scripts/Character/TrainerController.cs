using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TrainerController : MonoBehaviour
{
    [SerializeField] Dialog dialog;
    [SerializeField] GameObject DetectRange;

    CharacterBehaviour character;
    private void Awake()
    {
        character = GetComponent<CharacterBehaviour>();
    }

    private void Start()
    {
        SetTrainerRotation(character.Animator.DefaultDirection);
    }

    public IEnumerator StartBattle(PlayerController player)
    {
        var diff = player.transform.position - transform.position;
        var moveVec = diff - diff.normalized;
        moveVec = new Vector2(Mathf.Round(moveVec.x), Mathf.Round(moveVec.y));

        yield return character.Move(moveVec);
        
        StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
    }

    public void SetTrainerRotation(FacingDirection dir)
    {
        float dirAngle = 0f;
        if (dir == FacingDirection.Right)
            dirAngle = 90f;
        else if (dir == FacingDirection.Up)
            dirAngle = 180f;
        else if (dir == FacingDirection.Left)
            dirAngle = 270f;

        DetectRange.transform.eulerAngles = new Vector3(0f, 0f, dirAngle);
    }
}
