using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLevelList : MonoBehaviour
{
    public static EnemyLevelList enemyLevelSingleton = null;


    
    //List of all enemies per level
    public List<List<int>> m_EnemyTeams = new List<List<int>>();


    public List<int> getEnemyTeamID(int level)
    {
        return m_EnemyTeams[level];
    }

    public void Awake()
    {
        if (enemyLevelSingleton == null)
        {
            enemyLevelSingleton = this;
            return;
        }
        Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Example enemy team
        List<int> list = new List<int>() { 0,0,0 }; //Team of three of the first character
        m_EnemyTeams.Add(list); 


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
