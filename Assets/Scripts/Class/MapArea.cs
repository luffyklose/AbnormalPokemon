using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class MapArea : MonoBehaviour
{
    [SerializeField] private List<Monster> wildMonsters;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Monster GetRandomWildMonster()
    {
        Monster wild= wildMonsters[UnityEngine.Random.Range(0, wildMonsters.Count - 1)];
        wild.Init();
        return wild;
    }
}
