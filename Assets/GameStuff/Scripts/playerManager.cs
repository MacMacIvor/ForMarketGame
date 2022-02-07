using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using characterInterface;
using System;


//This script will hold all? of the players things to keep track of
public class playerManager : MonoBehaviour
{
    //This entire thing will all be saved in a file but for now it's not

    List<CHARACTERBOX> playerCharacterInventory = new List<CHARACTERBOX>();

    //This should point to a character inside the character inventory 
    List<int> playerCurrentTeam = new List<int>();

    //Non Character shit
    float playerXP = 0; //A general xp that players will gain and will unlock things later on
    float playerVIPXP = 0; //This is how much $$$ they gave basically

    float heldCoins = 0; //We can change the name for this later but this is the free currency
    float heldGold = 0; //We can change what this is later but it's the purchased currency

    int playerLevel = 0;
    int playerLevelForStoryAndNotJustGeneralLevel = 0;

    bool playerBoughtSupporter = false;
    bool playerBoughtDailyDoubles = false;
    bool playerBoughtPermanentStrength = false;
    bool playerBoughtEasyLeveling = false;


    float timeLeftInMonthlyPack = 0; //We can work out the details later but basically a type of pack that gives a bonus for the amount of time 
    float timeLeftInWeeklyPack = 0;
    float timeLeftInDailyPack = 0;

    

    public void buyMontly()
    {
        //Get the actual month later just to get different days
        timeLeftInMonthlyPack = 30;
    }

    public void buyWeekly()
    {
        timeLeftInWeeklyPack = 7;
    }

    public void buyDaily()
    {
        timeLeftInDailyPack = 1;
    }

    private void countdownPacks()
    {
        if (true)//New day function or check will be added here
        {
            if (timeLeftInDailyPack > 0) 
                timeLeftInDailyPack--;
            if (timeLeftInMonthlyPack > 0)
                timeLeftInMonthlyPack--;
            if (timeLeftInWeeklyPack > 0)
                timeLeftInWeeklyPack--;
        }
    }

    public void boughtGold(float purchaseAmount)
    {
        heldGold += purchaseAmount;
    }

    public bool useGold(float amountToSpend)
    {
        if(heldGold < amountToSpend)
        {
            return false;
        }
        else
        {
            heldGold -= amountToSpend;
            return true;
        }
    }

    public void boughtOrGainedCoins(float purchaseAmount)
    {
        heldCoins += purchaseAmount;
    }

    public bool useCoins(float amountToSpend)
    {
        if (heldCoins < amountToSpend)
        {
            return false;
        }
        else
        {
            heldCoins -= amountToSpend;
            return true;
        }
    }

    public void addPlayerXP(float xpAmount)
    {
        playerXP += xpAmount;
    }

    public void addPlayerVIPXP(float xpAmount)
    {
        playerVIPXP += xpAmount;
    }

    public List<CHARACTERBOX> getPlayerTeam()
    {
        List<CHARACTERBOX> gatheringTeam = new List<CHARACTERBOX>();

        for (int i = 0; i < playerCurrentTeam.Count; i++)
        {
            playerCharacterInventory[playerCurrentTeam[i]].reset();
            gatheringTeam.Add(playerCharacterInventory[playerCurrentTeam[i]]);
        }
        return gatheringTeam;

    }

    public void nextGameLevel()
    {
        playerLevelForStoryAndNotJustGeneralLevel++;
    }

    public int getGameLevel()
    {
        return playerLevelForStoryAndNotJustGeneralLevel;
    }

    public void sacrificeSelected(int sacrificedID, int eaterID)
    {
        playerCharacterInventory[eaterID].addExperience(playerCharacterInventory[sacrificedID].calculateXP(playerCharacterInventory[eaterID].getFaction()));
        if (playerCharacterInventory[sacrificedID].getFormationPosition() != 99)
        {
            playerCurrentTeam.RemoveAt(playerCharacterInventory[sacrificedID].getFormationPosition());
        }
        playerCharacterInventory.RemoveAt(sacrificedID);
        
    }

    public List<int> intentionsToPurchase(string purchaseName)
    {
        List<int> list = new List<int>();

                //Start checking for if the player can even buy the pack
        switch (purchaseName)
        {
            case "FreeCurrency":
                if (heldCoins >= 1000)
                {
                    heldCoins -= 1000;
                    //Return 10 random characters that the player bough, to be added when there exists 10 characters in code
                }
                break;
            case "BetterFreeCurrency":
                if (heldCoins >= 2700)
                {
                    heldCoins -= 2700;
                    //Return 10 random characters that the player bough, to be added when there exists 10 characters in code
                }
                break;
            case "Paid":
                if (heldGold >= 2700)
                {
                    heldGold -= 2700;
                    //Return 10 random characters that the player bough, to be added when there exists 10 characters in code
                }
                break;
            case "Daily":
                //This one has to be tested once we have put it into the playstore, verification of payment is needed here
                buyDaily();
                break;
            case "Weekly":
                //This one has to be tested once we have put it into the playstore, verification of payment is needed here
                buyWeekly();
                break;
            case "Monthly":
                //This one has to be tested once we have put it into the playstore, verification of payment is needed here
                buyMontly();
                break;
            case "500":
                //This one has to be tested once we have put it into the playstore, verification of payment is needed here
                heldGold += 500;
                break;
            case "3000":
                //This one has to be tested once we have put it into the playstore, verification of payment is needed here
                heldGold += 3000;
                break;
            case "10000":
                //This one has to be tested once we have put it into the playstore, verification of payment is needed here
                heldGold += 10000;
                break;
            case "SupporterPack":
                //This one has to be tested once we have put it into the playstore, verification of payment is needed here
                playerBoughtSupporter = true;
                break;
            case "DailyDoubles":
                //This one has to be tested once we have put it into the playstore, verification of payment is needed here
                playerBoughtDailyDoubles = true;
                break;
            case "PermanentStrength":
                //This one has to be tested once we have put it into the playstore, verification of payment is needed here
                playerBoughtPermanentStrength = true;
                break;
            case "EasyLeveling":
                //This one has to be tested once we have put it into the playstore, verification of payment is needed here
                playerBoughtEasyLeveling = true;
                break;

        }
        return list;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Again, this will be loaded from a file but for now it's hard coded
        playerCharacterInventory.Add(new Character1());
        playerCharacterInventory[0].Initialization(0, 0);
        playerCurrentTeam.Add(0);

        playerCharacterInventory.Add(new Character1());
        playerCharacterInventory[1].Initialization(1, 0);
        playerCurrentTeam.Add(1);

    }

    // Update is called once per frame
    void Update()
    {
       //Debug.Log("kjdsf");
    }
}
