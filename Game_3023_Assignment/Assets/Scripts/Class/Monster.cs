using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
