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
   [SerializeField] private int expYield;

   [SerializeField] private List<LearnableMove> learnableMoves;

   public static int MAXMOVESNUM { get; set; } = 4;

   public int GetExpForLevel(int level)
   {
      return 4 * (level * level * level) / 5;
   }
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

   public MonsterType Type1
   {
      get { return type1; }
   }

   public MonsterType Type2
   {
      get { return type2; }
   }

   public int ExpYield
   {
      get { return expYield; }
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
   [SerializeField] private int level;

   public MoveBase Base
   {
      get { return movebase; }
   }

   public int Level
   {
      get { return level; }
   }
}

public enum Stat
{
   Attack,
   Defence,
   SpAttack,
   SpDefence,
   Speed
}

public enum MonsterType
{
   None,
   Normal,
   Fire,
   Water,
   Electric,
   Grass,
   Ice,
   Fighting,
   Poison,
   Ground,
   Flying,
   Psychic,
   Bug,
   Rock,
   Ghost,
   Dragon,
   Dark,
   Steel,
   Fairy
}

public class TypeChart
{
    static float[][] chart =
    {
         //                       Nor   Fir   Wat   Ele   Gra   Ice   Fig   Poi   Gro   Fly   Psy   Bug   Roc   Gho   Dra   Dar  Ste    Fai
        /*Normal*/  new float[] {1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   0.5f, 0,    1f,   1f,   0.5f, 1f},
        /*Fire*/    new float[] {1f,   0.5f, 0.5f, 1f,   2f,   2f,   1f,   1f,   1f,   1f,   1f,   2f,   0.5f, 1f,   0.5f, 1f,   2f,   1f},
        /*Water*/   new float[] {1f,   2f,   0.5f, 1f,   0.5f, 1f,   1f,   1f,   2f,   1f,   1f,   1f,   2f,   1f,   0.5f, 1f,   1f,   1f},
        /*Electric*/new float[] {1f,   1f,   2f,   0.5f, 0.5f, 1f,   1f,   1f,   0f,   2f,   1f,   1f,   1f,   1f,   0.5f, 1f,   1f,   1f},
        /*Grass*/   new float[] {1f,   0.5f, 2f,   1f,   0.5f, 1f,   1f,   0.5f, 2f,   0.5f, 1f,   0.5f, 2f,   1f,   0.5f, 1f,   0.5f, 1f},
        /*Ice*/     new float[] {1f,   0.5f, 0.5f, 1f,   2f,   0.5f, 1f,   1f,   2f,   2f,   1f,   1f,   1f,   1f,   2f,   1f,   0.5f, 1f},
        /*Fighting*/new float[] {2f,   1f,   1f,   1f,   1f,   2f,   1f,   0.5f, 1f,   0.5f, 0.5f, 0.5f, 2f,   0f,   1f,   2f,   2f,   0.5f},
        /*Poison*/  new float[] {1f,   1f,   1f,   1f,   2f,   1f,   1f,   0.5f, 0.5f, 1f,   1f,   1f,   0.5f, 0.5f, 1f,   1f,   0f,   2f},
        /*Ground*/  new float[] {1f,   2f,   1f,   2f,   0.5f, 1f,   1f,   2f,   1f,   0f,   1f,   0.5f, 2f,   1f,   1f,   1f,   2f,   1f},
        /*Flying*/  new float[] {1f,   1f,   1f,   0.5f, 2f,   1f,   2f,   1f,   1f,   1f,   1f,   2f,   0.5f, 1f,   1f,   1f,   0.5f, 1f},
        /*Psychic*/ new float[] {1f,   1f,   1f,   1f,   1f,   1f,   2f,   2f,   1f,   1f,   0.5f, 1f,   1f,   1f,   1f,   0f,   0.5f, 1f},
        /*Bug*/     new float[] {1f,   0.5f, 1f,   1f,   2f,   1f,   0.5f, 0.5f, 1f,   0.5f, 2f,   1f,   1f,   0.5f, 1f,   2f,   0.5f, 0.5f},
        /*Rock*/    new float[] {1f,   2f,   1f,   1f,   1f,   2f,   0.5f, 1f,   0.5f, 2f,   1f,   2f,   1f,   1f,   1f,   1f,   0.5f, 1f},
        /*Ghost*/   new float[] {0f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   0.5f, 1f,   1f,   2f,   1f,   0.5f, 1f,   1f},
        /*Dragon*/  new float[] {1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   2f,   1f,   0.5f, 0f},
        /*Dark*/    new float[] {1f,   1f,   1f,   1f,   1f,   1f,   0.5f, 1f,   1f,   1f,   2f,   1f,   1f,   2f,   1f,   0.5f, 1f,   0.5f},
        /*Steel*/   new float[] {1f,   0.5f, 0.5f, 0.5f, 1f,   2f,   1f,   1f,   1f,   1f,   1f,   2f,   0.5f, 1f,   1f,   1f,   0.5f, 2f},
        /*Fairy*/   new float[] {1f,   0.5f, 1f,   1f,   1f,   1f,   2f,   0.5f, 1f,   1f,   1f,   1f,   1f,   1f,   2f,   2f,   0.5f, 1f}
    };

    public static float GetEffectiveness(MonsterType attackType, MonsterType defendType)
    {
        if (attackType == MonsterType.None || defendType == MonsterType.None)
            return 1f;

        int row = (int)attackType - 1;
        int col = (int)defendType - 1;

        return chart[row][col];
    }
}