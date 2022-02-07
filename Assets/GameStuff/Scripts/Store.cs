using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using characterInterface;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;
public class Store : MonoBehaviour
{
    int currentPage = 0; //0 is main, 1 is card, 2 is cardShowing, 3 is timePack, 4 is skin, 5 is currency, 6 is permanent
    int lastSavedPage = 0;
    private float screenTouchTimerForTapOrNot = 0;
    private float screenTouchMaxTimerForTapOrNot = 0.1f; //Honestly Should have made a script for input management instead of doing this multiple times, but it is done now so it shall stay as is

    private Vector3 initScreenTouchPos = new Vector3();
    private float giveRoomBeforeConsiteredSliding = 3;
    float moveDistance2 = 0;

    public List<GameObject> ListOfPages = new List<GameObject>();

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
            switch (curRaysastResult.gameObject.transform.name)
            {
                case "toCards":
                    return 1;
                case "FreeCurrency":
                case "BetterFreeCurrency":
                case "Paid":
                    //Will check if they have enough money later
                    return 2;
                case "toTimePacks":
                    return 3;
                case "toSkins":
                    return 4;
                case "toCurrency":
                    return 5;
                case "toPermanentPacks":
                    return 6;
                case "goBack":
                    return currentPage == 2 ? 1 : 0;

                //These are not for the pages but rather the individual items themselves
                default:
                    if (curRaysastResult.gameObject.transform.name != "Background")
                    {

                    }
                    break;
            }
            
        }
        return 99; //This is an error
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        switch (currentPage)
        {
            case 4:
                break;
            default:
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
                    switch (screenTouchTimerForTapOrNot < screenTouchMaxTimerForTapOrNot)
                    {
                        case true:
                            screenTouchTimerForTapOrNot += Time.deltaTime;

                            break;
                        case false: //Once here then we know the player is not just tapping the screen, this means they are either holding it for some reason or are moving it and either case it's not a tap
                                    //So We don't care about hte input because the only place that it matters is in the skin menu
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
                                Debug.Log(IsPointerOverUIElement(initScreenTouchPos));

                                currentPage = IsPointerOverUIElement(initScreenTouchPos) > 6 ? currentPage : IsPointerOverUIElement(initScreenTouchPos);
                                if (currentPage != lastSavedPage)
                                    switch (currentPage)
                                    {
                                        case 0:
                                            ListOfPages[0].SetActive(true);
                                            ListOfPages[1].SetActive(false);
                                            ListOfPages[2].SetActive(false);
                                            ListOfPages[3].SetActive(false);
                                            ListOfPages[4].SetActive(false);
                                            ListOfPages[5].SetActive(false);
                                            ListOfPages[6].SetActive(false);
                                            break;
                                        case 1:
                                            ListOfPages[0].SetActive(false);
                                            ListOfPages[1].SetActive(true);
                                            ListOfPages[2].SetActive(false);
                                            ListOfPages[3].SetActive(false);
                                            ListOfPages[4].SetActive(false);
                                            ListOfPages[5].SetActive(false);
                                            ListOfPages[6].SetActive(false);
                                            break;
                                        case 2:
                                            ListOfPages[0].SetActive(false);
                                            ListOfPages[1].SetActive(true);
                                            ListOfPages[2].SetActive(true);
                                            ListOfPages[3].SetActive(false);
                                            ListOfPages[4].SetActive(false);
                                            ListOfPages[5].SetActive(false);
                                            ListOfPages[6].SetActive(false);
                                            break;
                                        case 3:
                                            ListOfPages[0].SetActive(false);
                                            ListOfPages[1].SetActive(false);
                                            ListOfPages[2].SetActive(false);
                                            ListOfPages[3].SetActive(true);
                                            ListOfPages[4].SetActive(false);
                                            ListOfPages[5].SetActive(false);
                                            ListOfPages[6].SetActive(false);
                                            break;
                                        case 4:
                                            ListOfPages[0].SetActive(false);
                                            ListOfPages[1].SetActive(false);
                                            ListOfPages[2].SetActive(false);
                                            ListOfPages[3].SetActive(false);
                                            ListOfPages[4].SetActive(true);
                                            ListOfPages[5].SetActive(false);
                                            ListOfPages[6].SetActive(false);
                                            break;
                                        case 5:
                                            ListOfPages[0].SetActive(false);
                                            ListOfPages[1].SetActive(false);
                                            ListOfPages[2].SetActive(false);
                                            ListOfPages[3].SetActive(false);
                                            ListOfPages[4].SetActive(false);
                                            ListOfPages[5].SetActive(true);
                                            ListOfPages[6].SetActive(false);
                                            break;
                                        case 6:
                                            ListOfPages[0].SetActive(false);
                                            ListOfPages[1].SetActive(false);
                                            ListOfPages[2].SetActive(false);
                                            ListOfPages[3].SetActive(false);
                                            ListOfPages[4].SetActive(false);
                                            ListOfPages[5].SetActive(false);
                                            ListOfPages[6].SetActive(true);
                                            break;

                                    }
                                lastSavedPage = currentPage;
                                break;
                            default:
                                break;
                        }
                    }
                    screenTouchTimerForTapOrNot = 0;
                }
                break;
        }
        
    }
}
