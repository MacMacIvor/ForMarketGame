using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using characterInterface;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;
public class characterInventory : MonoBehaviour
{
    //Update with the total amount of characters in the game;
    //I couldn't think of a better way to do this
    public const float amountOfCharacters = 1;

    public List<GameObject> pages = new List<GameObject>();

    public GameObject m_PlayerManager;
    public GameObject m_CharacterList;

    List<CHARACTERBOX> m_PlayerTeam = new List<CHARACTERBOX>();

    public int currentPage = 0; //One page for actual inventory, 0 another for all possible characters and showing what is owned or not, 2 for the character info and leveling

    private Touch theTouch;

    private Vector3 lastMoved = new Vector3();
    private bool reseted = false;
    private bool inputReleased = true;
    public int UILayer;

    private float screenTouchTimerForTapOrNot = 0;
    private float screenTouchMaxTimerForTapOrNot = 0.1f;
    private Vector3 initScreenTouchPos = new Vector3();
    private float giveRoomBeforeConsiteredSliding = 3;
    private List<int> selectedForSacrifice = new List<int>();
    private int selectedCharacterID = 0;
    float moveDistance = 0;
    float moveDistance2 = 0;

    //Returns 'true' if we touched or hovering on Unity UI element.
    public int IsPointerOverUIElement(Vector3 position)
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults(position));
    }


    //Returns number of the gameobject so we know which one was clicked
    private int IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.transform.parent.name == "CharacterList")
                return curRaysastResult.gameObject.transform.GetSiblingIndex();
            else if (curRaysastResult.gameObject.transform.parent.name == "GoBack" || curRaysastResult.gameObject.name == "GoBack")
                return -1;
            else if (curRaysastResult.gameObject.transform.name == "Sacrifice Button")
                return -3;

        }
        return -2; //This is an error, we shouldn't ever get this really is what I thought at first but turns out I forgot this would be default value that is return if nothing of value is pressed
    }


    //Gets all event system raycast results of current mouse or touch position.
    //Thanks to daveMennenoh https://forum.unity.com/threads/how-to-detect-if-mouse-is-over-ui.1025533/
    static List<RaycastResult> GetEventSystemRaycastResults(Vector3 position)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = position;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }

    public GameObject newCharacter(int id, int page, bool gray, int childPos = 0) //Not necessarily enemies just adding characters to their holders
    {
        GameObject newObject = Instantiate(m_CharacterList.transform.GetChild(id).gameObject, pages[page].transform.GetChild(childPos).transform);
        switch (gray)
        {
            case true:
                var seperation = newObject.GetComponent<RawImage>().color = Color.gray;
                break;

        }
        return newObject;
    }


    void loadThings()
    {
        if (pages[0].transform.GetChild(0).transform.childCount != 0) //Checks if lists are already loaded because if they are then calling this again means something changed and they should be reset 
        {
            for (int i = 0; i < pages[0].transform.GetChild(0).transform.childCount; i++)
            {
                Destroy(pages[0].transform.GetChild(0).transform.GetChild(i).transform.gameObject, 0.01f);
            }
            for (int i = 0; i < pages[1].transform.GetChild(0).transform.childCount; i++)
            {
                Destroy(pages[1].transform.GetChild(0).transform.GetChild(i).transform.gameObject, 0.01f);
            }
            for (int i = 0; i < pages[2].transform.GetChild(0).transform.childCount; i++)
            {
                Destroy(pages[2].transform.GetChild(0).transform.GetChild(i).transform.gameObject, 0.01f);
            }
        }
        int amountOfCharcters = m_CharacterList.transform.childCount;
        List<int> listTot = new List<int>();
        List<int> listHas = new List<int>();
        for (int i = 0; i < amountOfCharcters; i++)
        {
            listTot.Add(i);
        }


        m_PlayerTeam = new List<CHARACTERBOX>(m_PlayerManager.GetComponent<playerManager>().getPlayerTeam());
        for (int i = 0; i < m_PlayerTeam.Count; i++)
        {
            newCharacter(m_PlayerTeam[i].getCharacterID(), 1, false);
            newCharacter(m_PlayerTeam[i].getCharacterID(), 2, false);
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


    // Start is called before the first frame update
    void Start()
    {
        loadThings();

    }

    // Update is called once per frame
    void Update()
    {
        switch (currentPage)
        {
            case 2:
                ///////////////////////////////////////////////////////////////////////////////////////
                ///This is for the sacrifice page
                ///////////////////////////////////////////////////////////////////////////////////////
                if (Input.touchCount > 0)
                {
                    Touch touch2 = Input.GetTouch(0); // get first touch2 since touch2 count is greater than zero


                    switch (screenTouchTimerForTapOrNot == 0)
                    {
                        case true:
                            initScreenTouchPos = Camera.main.ScreenToWorldPoint(touch2.position);
                            break;
                        case false:

                            break;
                    }
                    moveDistance2 = Vector3.Distance(initScreenTouchPos, Camera.main.ScreenToWorldPoint(touch2.position));
                    switch (screenTouchTimerForTapOrNot < screenTouchMaxTimerForTapOrNot && moveDistance2 < giveRoomBeforeConsiteredSliding)
                    {
                        case true:
                            screenTouchTimerForTapOrNot += Time.deltaTime;

                            break;
                        case false: //Once here then we know the player is not just tapping the screen, this means they are either holding it for some reason or are moving it and either case it's not a tap
                                    //so we do not look into the tapping part of things
                            if (touch2.phase == TouchPhase.Stationary || touch2.phase == TouchPhase.Moved)
                            {
                                switch (reseted)
                                {
                                    case true:

                                        // get the touch2 position from the screen touch2 to world point
                                        Vector3 touch2edPos = (Camera.main.ScreenToWorldPoint(new Vector3(0, touch2.position.y, 0)));
                                        // lerp and set the position of the current object to that of the touch2, but smoothly over time.
                                        //transform.position = Vector3.Lerp(transform.position, touch2edPos, Time.deltaTime);
                                        Vector3 amountToMove2 = touch2edPos - lastMoved;
                                        pages[currentPage].transform.GetChild(0).transform.position += amountToMove2;
                                        lastMoved = touch2edPos;

                                        break;
                                    case false:

                                        Vector3 touch2edPos2 = Camera.main.ScreenToWorldPoint((new Vector3(0, touch2.position.y, 0)));
                                        lastMoved = touch2edPos2;
                                        reseted = true;
                                        
                                        break;
                                }

                            }
                            break;
                    }

                }
                else //When the user lets go of the screen
                {
                    if (screenTouchTimerForTapOrNot != 0)
                    {
                        switch (screenTouchTimerForTapOrNot < screenTouchMaxTimerForTapOrNot && moveDistance2 < giveRoomBeforeConsiteredSliding)
                        {
                            case true:
                                //Call the touch2 function here
                                int num2 = IsPointerOverUIElement(initScreenTouchPos);
                                switch (num2)
                                {
                                    case -2:
                                        Debug.LogError("YoSomethingWentWrongWithTheUIClickFunction"); //Not necessarely true but I leave it for now because it's useful if something goes wrong
                                        break;
                                    case -1:
                                        pages[currentPage].SetActive(false);
                                        currentPage = currentPage == 1 ? 0 : 1;
                                        pages[currentPage].SetActive(true);
                                        for (int i = selectedForSacrifice.Count -1; i > -1 ; i--)
                                        {
                                            pages[2].transform.GetChild(num2).GetComponent<RawImage>().color = Color.white;
                                            selectedForSacrifice.Remove(i);
                                        }
                                        break;
                                    case -3:
                                        if (selectedForSacrifice.Count > 0)
                                        {
                                            //Time for the sacrifice
                                            for (int i = selectedForSacrifice.Count -1; i > -1; i--)
                                            {
                                                m_PlayerManager.GetComponent<playerManager>().sacrificeSelected(selectedForSacrifice[i], selectedCharacterID);
                                                selectedForSacrifice.RemoveAt(i);
                                            }
                                            loadThings();
                                        }
                                        break;
                                    default:

                                        //Start by looking if it's already in the list to know if we need to add it or remove it
                                        bool wasFound = false;
                                        if (num2 == selectedCharacterID)
                                        {
                                            wasFound = true; //Ignore this because the player clicked on the character they have in the main slot
                                        }
                                        else
                                        {

                                            for (int i = 0; i < selectedForSacrifice.Count; i++)
                                            {
                                                if (selectedForSacrifice[i] == num2)
                                                {
                                                    wasFound = true;
                                                    selectedForSacrifice.RemoveAt(i);
                                                    pages[2].transform.GetChild(0).transform.GetChild(num2).GetComponent<RawImage>().color = Color.white;

                                                }
                                            }
                                            if (!wasFound)
                                            {
                                                bool inserted = false;
                                                for (int i = 0;i < selectedForSacrifice.Count; i++)
                                                {
                                                    if (num2 < selectedForSacrifice[i])
                                                    {
                                                        selectedForSacrifice.Insert(i, num2);
                                                        inserted = true;
                                                    }
                                                }
                                                if (!inserted)
                                                    selectedForSacrifice.Add(num2);


                                                pages[2].transform.GetChild(0).transform.GetChild(num2).GetComponent<RawImage>().color = Color.blue;
                                            }
                                        }


                                        break;
                                }

                                break;
                            default:
                                break;
                        }
                    }
                    screenTouchTimerForTapOrNot = 0;
                    lastMoved = Vector3.zero;
                    reseted = false;
                    inputReleased = true;
                }
                break;



            default:
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///This is the non sacrifice page
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                

                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0); // get first touch since touch count is greater than zero

                    
                    switch (screenTouchTimerForTapOrNot == 0)
                    {
                        case true:
                            initScreenTouchPos = Camera.main.ScreenToWorldPoint(touch.position);
                            break;
                        case false:

                            break;
                    }
                    moveDistance = Vector3.Distance(initScreenTouchPos, Camera.main.ScreenToWorldPoint(touch.position));
                    switch (screenTouchTimerForTapOrNot < screenTouchMaxTimerForTapOrNot && moveDistance < giveRoomBeforeConsiteredSliding)
                    {
                        case true:
                            screenTouchTimerForTapOrNot += Time.deltaTime;

                            break;
                        case false: //Once here then we know the player is not just tapping the screen, this means they are either holding it for some reason or are moving it and either case it's not a tap
                                    //so we do not look into the tapping part of things
                            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                            {
                                switch (reseted)
                                {
                                    case true:

                                        // get the touch position from the screen touch to world point
                                        Vector3 touchedPos = Camera.main.ScreenToWorldPoint((new Vector3(0, touch.position.y, 0)));
                                        // lerp and set the position of the current object to that of the touch, but smoothly over time.
                                        //transform.position = Vector3.Lerp(transform.position, touchedPos, Time.deltaTime);
                                        Vector3 amountToMove = touchedPos - lastMoved;
                                        pages[currentPage].transform.GetChild(0).transform.position += amountToMove;
                                        lastMoved = touchedPos;

                                        break;
                                    case false:

                                        Vector3 touchedPos2 = Camera.main.ScreenToWorldPoint((new Vector3(0, touch.position.y, 0)));
                                        lastMoved = touchedPos2;
                                        reseted = true;

                                        break;
                                }

                            }
                            break;
                    }

                }
                else //When the user lets go of the screen
                {
                    if (screenTouchTimerForTapOrNot != 0)
                    {
                        switch (screenTouchTimerForTapOrNot < screenTouchMaxTimerForTapOrNot && moveDistance < giveRoomBeforeConsiteredSliding)
                        {
                            case true:
                                //Call the touch function here
                                int num = IsPointerOverUIElement(initScreenTouchPos);
                                switch (num)
                                {
                                    case -2:
                                        Debug.LogError("YoSomethingWentWrongWithTheUIClickFunction"); //Not necessarely true but I leave it for now because it's useful if something goes wrong
                                        break;
                                    case -1:
                                        pages[currentPage].SetActive(false);
                                        currentPage = currentPage == 1 ? 0 : 1;
                                        pages[currentPage].SetActive(true);
                                        break;
                                    default:
                                        //We get all necessary info for the display
                                        Debug.Log("It Worked up until here");
                                        pages[currentPage].SetActive(false);
                                        currentPage = 2;
                                        pages[currentPage].SetActive(true);

                                        selectedCharacterID = num;
                                        List<string> temp = m_PlayerTeam[num].displayInformation();
                                        pages[currentPage].transform.GetChild(4).transform.gameObject.GetComponent<TextMeshProUGUI>().text = temp[0];
                                        pages[currentPage].transform.GetChild(5).transform.gameObject.GetComponent<TextMeshProUGUI>().text = temp[1];
                                        pages[currentPage].transform.GetChild(3).transform.gameObject.GetComponent<RawImage>().texture = m_CharacterList.transform.GetChild(num).transform.GetComponent<RawImage>().texture;


                                        break;
                                }

                                break;
                            default:
                                break;
                        }
                    }
                    screenTouchTimerForTapOrNot = 0;
                    lastMoved = Vector3.zero;
                    reseted = false;
                    inputReleased = true;
                }
                break;
        }
        
    }
}
