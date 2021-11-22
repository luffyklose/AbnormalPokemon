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

    public DamageDetails TakeDamage(Move move, Monster attacker)
    {
        float typeEffectiveness = TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type1) *
                                  TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type2);
        DamageDetails damageDetails = new DamageDetails()
        {
            TypeEffectiveness = typeEffectiveness,
            Fainted = false
        };
        float attack = (move.Base.IsSpecial) ? attacker.SpAttack : attacker.Attack;
        float defence = (move.Base.IsSpecial) ? SpDefence : Defence;
        
        float modifiers = UnityEngine.Random.Range(0.85f, 1f) * typeEffectiveness;
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attack / defence) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        //Debug.Log(Base.Name + " HP:" + HP);
        HP -= damage;
        //Debug.Log(attacker.Base.Name+" attack use "+move.Base.Name+", HP "+HP);
        if (HP <= 0)
        {
            HP = 0;
            damageDetails.Fainted = true;
        }

        damageDetails.Fainted = false;
        return damageDetails;
    }

    public Move GetRandomMove()
    {
        int r = UnityEngine.Random.Range(0, Moves.Count);
        return Moves[r];
    }
}

public class DamageDetails
{
    public bool Fainted { get; set; }
    public float Critical{get;set; }
    public float TypeEffectiveness{get;set; }
}
