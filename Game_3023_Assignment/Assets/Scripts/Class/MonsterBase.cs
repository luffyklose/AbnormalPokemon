using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster",menuName = "Monster/Monster")]
public class MonsterBase : ScriptableObject
{
   [SerializeField] private string name;
   [TextArea] [SerializeField] private string description;
   [SerializeField] private Sprite frontSprite;
   [SerializeField] private Sprite backSprite;
   [SerializeField] private MonsterType type1;
   [SerializeField] private MonsterType type2;
   
   //Base attribute
   [SerializeField] private int maxHP;
   [SerializeField] private int attack;
   [SerializeField] private int defence;
   [SerializeField] private int spAttack;
   [SerializeField] private int spDefence;
   [SerializeField] private int speed;

   [SerializeField] private List<LearnableMove> learnableMoves;

   public string Name
   {
      get { return name; }
   }
   
   public string Description
   {
      get { return description; }
   }
   
   public Sprite FrontSprite
   {
      get { return frontSprite; }
   }
   
   public Sprite BackSprite
   {
      get { return backSprite; }
   }

   public int MaxHP
   {
      get { return maxHP; }
   }
   
   public int Attack
   {
      get { return attack; }
   }
   
   public int Defence
   {
      get { return defence; }
   }
   
   public int SpAttack
   {
      get { return spAttack; }
   }
   
   public int SpDefence
   {
      get { return spDefence; }
   }
   
   public int Speed
   {
      get { return speed; }
   }

   public List<LearnableMove> LearnableMoves
   {
      get { return learnableMoves; }
   }
}

[System.Serializable]
public class LearnableMove
{
   [SerializeField] private MoveBase movebase;

   public MoveBase Base
   {
      get { return movebase; }
   }
}

public enum MonsterType
{
   None,
   Normal,
   Fire,
   Grass,
   Water,
}
