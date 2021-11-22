using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "Monster/Move", order = 0)]
public class MoveBase : ScriptableObject
{
    [SerializeField] private string name;
    [TextArea] [SerializeField] private string description;

    [SerializeField] private MonsterType type;
    [SerializeField] private int power;
    [SerializeField] private int accuracy;
    //PP is the number of times a move can be performed
    [SerializeField] private int pp;

    public string Name
    {
        get { return name; }
    }
    
    public string Description
    {
        get { return description; }
    }
    
    public MonsterType Type
    {
        get { return type; }
    }
    
    public int Power
    {
        get { return power; }
    }
    
    public int Accuracy
    {
        get { return accuracy; }
    }
    
    public int Pp
    {
        get { return pp; }
    }

    public bool IsSpecial
    {
        get
        {
            if (type == MonsterType.Fire || type == MonsterType.Water || type == MonsterType.Grass ||
                type == MonsterType.Ice || type == MonsterType.Electric || type == MonsterType.Dragon)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

