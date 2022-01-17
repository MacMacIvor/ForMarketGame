using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using characterInterface;


public class BattlingSystem : MonoBehaviour
{
    public GameObject m_PlayerManager;
    public GameObject m_CharacterList;

    public GameObject m_PlayerGameObjectHolder;
    public GameObject m_EnemyGameObjectHolder;

    //This is int for now but it will be a list of characters (the class Thaidan be doing)
    private List<CHARACTERBOX> m_PlayerTeam = new List<CHARACTERBOX>();
    private List<CHARACTERBOX> m_EnemyTeam = new List<CHARACTERBOX>();
    //First number will be for which team, second will be for who in the team
    private List<List<int>> m_Order = new List<List<int>>();
    
    private bool m_Done = true;
    private bool m_Played = false;
    private float m_Time;
    public float m_animationDurationTime = 1;
    private bool m_BattleDone = true;
    private bool needsReset = false;

    public int anum = 0;

    //How do I do this?
    //I don't fucking knowww
    //I need an initialize battle which should
    //Get both teams
    //Organize their attacks by speed (so we also need to get the speed)
    //Eventually we will need to get the player team from where we will manage that whole shit

    public Vector3 whereToPlace(int num, int team)
    {
        switch (team)
        {
            case 0:
                switch (num)
                {
                    case 0:
                        return new Vector3(-1, 0, 0);
                    case 1:
                        return new Vector3(-2, 0, 0);
                    case 2:
                        return new Vector3(-3, 0, 0);
                    case 3:
                        return new Vector3(0, 0, 0);
                    case 4:
                        return new Vector3(0, 0, 0);
                    case 5:
                        return new Vector3(0, 0, 0);
                    case 6:
                        return new Vector3(0, 0, 0);
                }
                break;
            case 1:
                switch (num)
                {
                    case 0:
                        return new Vector3(1, 0, 0);
                    case 1:
                        return new Vector3(0, 0, 0);
                    case 2:
                        return new Vector3(0, 0, 0);
                    case 3:
                        return new Vector3(0, 0, 0);
                    case 4:
                        return new Vector3(0, 0, 0);
                    case 5:
                        return new Vector3(0, 0, 0);
                    case 6:
                        return new Vector3(0, 0, 0);
                }
                break;
        }
        return new Vector3(0, 0, 0);
    }
    public GameObject newEnemy(int id, int num, int team) //Not necessarily enemies just adding characters to their holders
    {
        switch (team)
        {
            case 0:
                GameObject newObject = Instantiate(m_CharacterList.transform.GetChild(id).gameObject, m_EnemyGameObjectHolder.transform);
                newObject.transform.position = whereToPlace(num, team);
                return newObject;
            case 1:
                GameObject newObject2 = Instantiate(m_CharacterList.transform.GetChild(id).gameObject, m_PlayerGameObjectHolder.transform);
                newObject2.transform.position = whereToPlace(num, team);
                return newObject2;  
        }
        return null;
    }

    private void InitializeBattle()
    {
       

        //Initializing
        m_PlayerTeam = m_PlayerManager.GetComponent<playerManager>().getPlayerTeam();
        
        List<int> enemyIDs = new List<int>(EnemyLevelList.enemyLevelSingleton.getEnemyTeamID(m_PlayerManager.GetComponent<playerManager>().getGameLevel()));
        
        for (int i = 0; i < enemyIDs.Count; i++)
        {
            m_EnemyTeam.Add(getCharacters(enemyIDs[i]));
            m_EnemyTeam[i].setGameObject(newEnemy(m_EnemyTeam[i].getCharacterID(), i, 0));
        }
        

        //Lets start organising the enemy
        for (int i = 0; i < m_EnemyTeam.Count; i++)
        {
            switch (i)
            {
                case 0:
                    List<int> m_OrderTemp = new List<int>() { 0, i };
                    m_Order.Add(m_OrderTemp);
                    break;
                default:
                    float maxNum = m_Order.Count;
                    for (int j = 0; j < maxNum; j++)
                    {
                        float currentSpeedToLookAt = m_EnemyTeam[i].getSpeed();
        
                        //See if the speed is faster than the current position
                        if (currentSpeedToLookAt < m_EnemyTeam[m_Order[j][1]].getSpeed())
                        {
                            //If so add it to that position pushing the other one back
                            List<int> n_OrderTemp = new List< int>() { 0, i };
                            m_Order.Insert(j, n_OrderTemp);
                        }
                        else if (j == m_Order.Count - 1)
                        {
                            //Went throught the entire list so this speed is the slowest and is added to the end
                            //m_Order.Add(new List<int>(0, m_EnemyTeam[i]), i);
                            List<int> m_OrderTemp2 = new List<int>() { 0, i };
                            m_Order.Add(m_OrderTemp2);
                        }
        
                    }
                    break;
            }
        }

        //Now we can add the player characters
        for (int i = 0; i < m_PlayerTeam.Count; i++)
        {
            m_PlayerTeam[i].setGameObject(newEnemy(m_PlayerTeam[i].getCharacterID(), i, 1));

            float maxNum2 = m_Order.Count;
            for (int j = 0; j < maxNum2; j++)
            {
                float currentSpeedToLookAt = m_PlayerTeam[i].getSpeed();
        
                //See if the speed is faster than the current position
                if (currentSpeedToLookAt < m_EnemyTeam[m_Order[j][1]].getSpeed())
                {
                    //If so add it to that position pushing the other one back
                    List<int> n_OrderTemp = new List<int>() { 1, i };
                    m_Order.Insert(j, n_OrderTemp);
                }
                else if (j == m_Order.Count - 1)
                {
                    //Went throught the entire list so this speed is the slowest and is added to the end
                    List<int> m_OrderTemp = new List<int>() { 1, i };
                    m_Order.Add(m_OrderTemp);
                }
        
            }
        
        }

        m_BattleDone = false;
    }

    //I need the fighting part
        //Attack based on order
        //For now just attack front most person, later maybe characters attack different positions
int i = 0;
    private void BattleSequence()
    {
        
        //Battle is
        //1. Attack
        //2. Play and wait for animation
        //3. Check for death
        switch (i < m_Order.Count)
        {
            case true:
                switch (m_Done)
                {
                    case true:
                        switch (m_Played)
                        {
                            case false:
                                switch (m_Order[i][0])
                                {
                                    case 0:
                                        m_EnemyTeam[m_Order[i][1]].changeAnimation("Attack");
                                        break;
                                    case 1:
                                        m_PlayerTeam[m_Order[i][1]].changeAnimation("Attack");
                                        break;
                                }
                                

                                m_Done = false;
                                m_Played = true;
                                m_Time = m_animationDurationTime;
                                break;
                            case true:
                                switch (m_Order[i][0])
                                {
                                    case 0: //Enemy
                                        m_EnemyTeam[m_Order[i][1]].attack(m_PlayerTeam);
                                        Debug.Log("EnemyAttack");
                                        break;
                                    case 1: //Player
                                        m_PlayerTeam[m_Order[i][1]].attack(m_EnemyTeam);
                                        Debug.Log("PlayerAttack");
                                        break;
                                }

                                for (int y = 0; y < m_Order.Count; y++)
                                {
                                    switch (m_Order[y][0])
                                    {
                                        case 0: //Enemy
                                            if (m_EnemyTeam[m_Order[y][1]].isDeath())
                                            {
                                                m_Order.RemoveAt(y);
                                                y--;
                                            }
                                            break;
                                        case 1:
                                            if (m_PlayerTeam[m_Order[y][1]].isDeath())
                                            {
                                                m_Order.RemoveAt(y);
                                                y--;
                                            }
                                            break;
                                    }
                                }
                                bool isBattleWonLost = true;
                                //Check if won or lost
                                for (int z = 0; z < m_EnemyTeam.Count; z++)
                                {
                                    if (!(m_EnemyTeam[z].isDeath()))
                                    {
                                        isBattleWonLost = false;
                                    }
                                }
                                if (isBattleWonLost)
                                {
                                    //Can play a win animation later on
                                    m_BattleDone = true;
                                    m_PlayerManager.GetComponent<playerManager>().nextGameLevel();
                                    Debug.Log("BattleIsWon");
                                    needsReset = true;
                                }
                                isBattleWonLost = true;
                                //Check if won or lost
                                for (int z = 0; z < m_PlayerTeam.Count; z++)
                                {
                                    if (!(m_PlayerTeam[z].isDeath()))
                                    {
                                        isBattleWonLost = false;
                                    }
                                }
                                if (isBattleWonLost)
                                {
                                    //Can play a lost animation later on
                                    m_BattleDone = true;
                                    Debug.Log("BattleIsLost");
                                    needsReset = true;

                                }


                                i++;
                                m_Played = false;
                                break;
                        }   
                        break;
    
                    case false:
                        //Continue to wait for animation
                        m_Time -= Time.deltaTime;
                        if (m_Time < 0)
                        {
                            m_Done = true;
                            switch (m_Order[i][0])
                            {
                                case 0:
                                    m_EnemyTeam[m_Order[i][1]].changeAnimation("Iddle");
                                    break;
                                case 1:
                                    m_PlayerTeam[m_Order[i][1]].changeAnimation("Iddle");
                                    break;
                            }
                        }

                        break;
                }
                break;
            case false:
                i = 0;
                break;
        }
    }

    //Finish fight
        //Return to main but this will need to be later as we have no main
        //Give reward if won, also later
    private void ResetFight()
    {
        for (int i = 0; i < m_PlayerTeam.Count; i++)
        {
            m_PlayerTeam.RemoveAt(0);
            i--;
        }
        for (int i = 0;i < m_EnemyTeam.Count; i++)
        {
            m_EnemyTeam.RemoveAt(0);
            i--;
        }
        for (int i = 0; i < m_Order.Count; i++)
        {
            m_Order.RemoveAt(0);
            i--;
        }
        for (i = m_PlayerGameObjectHolder.transform.childCount - 1; i > -1;  i--)
        {
            UnityEngine.Object.Destroy(m_PlayerGameObjectHolder.transform.GetChild(i).gameObject);
        }
        for (i = m_EnemyGameObjectHolder.transform.childCount - 1; i > -1; i--)
        {
            UnityEngine.Object.Destroy(m_EnemyGameObjectHolder.transform.GetChild(i).gameObject);
        }
        i = 0;
    }



    // Start is called before the first frame update
    void Start()
    {
        //m_PlayerManager = GameObject.FindObjectOfType<m_PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("a") && m_BattleDone)
        {
            InitializeBattle();
        }
        switch (m_BattleDone)
        {
            case false:
                BattleSequence();
                break;
            case true:
                switch (needsReset)
                {
                    case true:
                        ResetFight();
                        needsReset = false;
                        break;
                }
                break;
        }
        //Debug.Log("AAAAAAA");
        if (Input.GetKeyDown("b"))
        {
            Debug.Log("alslWhyItNoWorks");
        }
    }

    //Unity is being dumb with me so put this wherever this is needed
    public CHARACTERBOX getCharacters(int id)
    {
        switch (id)
        {
            case 0:
                return new Character1();
            default:
                return null;
        }
    }

}

