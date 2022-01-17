using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using characterInterface;
using UnityEngine.UI;

public class characterInventory : MonoBehaviour
{
    //Update with the total amount of characters in the game;
    //I couldn't think of a better way to do this
    public const float amountOfCharacters = 1;

    public List<GameObject> pages = new List<GameObject>();

    public GameObject m_PlayerManager;
    public GameObject m_CharacterList;

    public int currentPage = 0; //One page for actual inventory and another for all possible characters and showing what is owned or not

    private Touch theTouch;

    private Vector3 lastMoved = new Vector3();
    private bool reseted = false;
    private bool inputReleased = true;




    public GameObject newCharacter(int id, int page, bool gray) //Not necessarily enemies just adding characters to their holders
    {
        GameObject newObject = Instantiate(m_CharacterList.transform.GetChild(id).gameObject, pages[page].transform.GetChild(0).transform);
        switch (gray)
        {
            case true:
                var seperation = newObject.GetComponent<RawImage>().color = Color.gray;
                break;

        }
        return newObject;
    }




    // Start is called before the first frame update
    void Start()
    {
        int amountOfCharcters = m_CharacterList.transform.childCount;
        List<int> listTot = new List<int>();
        List<int> listHas = new List<int>();
        for (int i = 0; i < amountOfCharcters; i++)
        {
            listTot.Add(i);
        }


        List<CHARACTERBOX> m_PlayerTeam = new List<CHARACTERBOX>(m_PlayerManager.GetComponent<playerManager>().getPlayerTeam());
        for (int i = 0; i < m_PlayerTeam.Count; i++)
        {
            newCharacter(m_PlayerTeam[i].getCharacterID(), 1, false);
            listHas.Add(m_PlayerTeam[i].getCharacterID());
        }

        for (int i = 0; i < listTot.Count; i++)
        {
            bool isPresent = false;
            for (int j = 0; j < listHas.Count; j++)
            {
                if (listTot[i] == listHas[j])
                {
                    isPresent = true;
                    j = listHas.Count;
                }
            }
            if (!isPresent)
            {
                newCharacter(i, 0, true); //Make it grey somehow
            }
            else
            {
                newCharacter(i, 0, false); //Make it normal
                
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // get first touch since touch count is greater than zero

            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                switch (reseted)
                {
                    case true:

                        // get the touch position from the screen touch to world point
                        Vector3 touchedPos = (new Vector3(0, touch.position.y, 0));
                        // lerp and set the position of the current object to that of the touch, but smoothly over time.
                        //transform.position = Vector3.Lerp(transform.position, touchedPos, Time.deltaTime);
                        Vector3 amountToMove = touchedPos - lastMoved;
                        pages[currentPage].transform.GetChild(0).transform.position += amountToMove;
                        lastMoved = touchedPos;

                        break;
                    case false:

                        Vector3 touchedPos2 = (new Vector3(0, touch.position.y, 0));
                        lastMoved = touchedPos2;
                        reseted = true;

                        break;
                }
                
            }
            else 
            {
                if (inputReleased)
                {
                    if (touch.position.y < 384)
                    {
                        pages[currentPage].SetActive(false);
                        currentPage = currentPage == 1 ? 0 : 1;
                        pages[currentPage].SetActive(true);

                    }
                    inputReleased = false;
                }
            }
        }
        else //When the user lets go of the screen
        {
            lastMoved = Vector3.zero;
            reseted=false;
            inputReleased = true;
        }
    }
}
