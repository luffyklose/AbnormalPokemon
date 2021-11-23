using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = Unity.Mathematics.Random;

[System.Serializable]
public class Monster
{
    [SerializeField] private MonsterBase _base;
    [SerializeField] private int level;

    public MonsterBase Base
    {
        get
        {
            return _base;
        }
    }

    public int Level
    {
        get
        {
            return level;
        }
    }
    public int HP { get; set; }
    public List<Move> Moves { get; set; }

    public void Init()
    {
        HP = MaxHP;
        Exp = Base.GetExpForLevel(level);

        Moves = new List<Move>();
        foreach (LearnableMove move in Base.LearnableMoves)
        {
            if (move.Level <= level)
            {
                Moves.Add(new Move(move.Base));
            }

            if (Moves.Count >= MonsterBase.MAXMOVESNUM)
            {
                break;
            }
        }
    }

    public bool CheckForLevelUp()
    {
        if (Exp > Base.GetExpForLevel(level + 1))
        {
            ++level;
            return true;
        }
        else
        {
            return false;
        }
    }

    public LearnableMove GetLearnableMoveAtCurrentLevel()
    {
        return Base.LearnableMoves.Where(x => x.Level == level).FirstOrDefault();
    }

    public void LearnMove(LearnableMove move)
    {
        if (Moves.Count >= MonsterBase.MAXMOVESNUM)
        {
            return;
        }
        Moves.Add(new Move(move.Base));
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

    public int Exp
    {
        get;
        set;
    }

    public DamageDetails TakeDamage(Move move, Monster attacker)
    {
        float typeEffectiveness = TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type1) *
                                  TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type2);
        DamageDetails damageDetails = new DamageDetails()
        {
            TypeEffectiveness = typeEffectiveness,
            Defeated = false
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
            damageDetails.Defeated = true;
        }
        else
        {
            damageDetails.Defeated = false;
        }

        return damageDetails;
    }

    public Move GetRandomMove()
    {
        int r = UnityEngine.Random.Range(0, Moves.Count);
        return Moves[r];
    }

    public Move GetWeakMove(Monster targetMonster)
    {
        List<Move> tempList = new List<Move>();
        foreach (Move move in Moves)
        {
            if (TypeChart.GetEffectiveness(move.Base.Type, targetMonster.Base.Type1) > 1.0f ||
                TypeChart.GetEffectiveness(move.Base.Type, targetMonster.Base.Type2) > 1.0f)
            {
                tempList.Add(move);
            }   
        }

        if (tempList.Count == 0)
        {
            foreach (Move move in Moves)
            {
                if (!(TypeChart.GetEffectiveness(move.Base.Type, targetMonster.Base.Type1) < 1.0f) ||
                    !(TypeChart.GetEffectiveness(move.Base.Type, targetMonster.Base.Type2) < 1.0f))
                {
                    tempList.Add(move);
                }
            }
        }
        
        if (tempList.Count == 0)
        {
            foreach (Move move in Moves)
            {
                tempList.Add(move);
            }
        }

        return tempList[UnityEngine.Random.Range(0, tempList.Count - 1)];
    }
}

public class DamageDetails
{
    public bool Defeated { get; set; }
    public float Critical{get;set; }
    public float TypeEffectiveness{get;set; }
}
