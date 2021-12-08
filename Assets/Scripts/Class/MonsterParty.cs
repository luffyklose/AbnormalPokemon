using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterParty : MonoBehaviour
{
    [SerializeField]private List<Monster> monsters;

    public List<Monster> Monsters
    {
        get { return monsters; }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (monsters.Count > 6)
        {
            for (int i = 6; i < monsters.Count; i++)
            {
                monsters.Remove(monsters[i]);
            }
        }
        
        foreach (Monster monster in monsters)
        {
            monster.Init();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Monster GetFirstHealthyMonster()
    {
        return monsters.Where(x => x.HP > 0).FirstOrDefault();
    }
}
