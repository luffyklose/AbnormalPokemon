using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class Monster
{
    public MonsterBase Base { get; set; }
    public int Level { get; set; }
    public int HP { get; set; }
    public List<Move> Moves { get; set; }

    public Monster(MonsterBase pbase, int plevel)
    {
        Base = pbase;
        Level = plevel;
        HP = MaxHP;

        Moves = new List<Move>();
        foreach (var move in Base.LearnableMoves)
        {
            Moves.Add(new Move(move.Base));
        }
    }

    public int Attack
    {
        get { return Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5; }
    }
    
    public int Defence
    {
        get { return Mathf.FloorToInt((Base.Defence * Level) / 100f) + 5; }
    }
    
    public int SpAttack
    {
        get { return Mathf.FloorToInt((Base.SpAttack * Level) / 100f) + 5; }
    }
    
    public int SpDefence
    {
        get { return Mathf.FloorToInt((Base.SpDefence * Level) / 100f) + 5; }
    }
    
    public int Speed
    {
        get { return Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5; }
    }
    
    public int MaxHP
    {
        get { return Mathf.FloorToInt((Base.MaxHP * Level) / 100f) + 10; }
    }

    public bool TakeDamage(Move move, Monster attacker)
    {
        float modifiers = UnityEngine.Random.Range(0.85f, 1f);
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attacker.Attack / Defence) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        //Debug.Log(Base.Name + " HP:" + HP);
        HP -= damage;
        //Debug.Log(attacker.Base.Name+" attack use "+move.Base.Name+", HP "+HP);
        if (HP <= 0)
        {
            HP = 0;
            return true;
        }

        return false;
    }

    public Move GetRandomMove()
    {
        int r = UnityEngine.Random.Range(0, Moves.Count);
        return Moves[r];
    }
}
