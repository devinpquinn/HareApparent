using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Character[] characters; // stores all characters in this game

    private void Start()
    {
        SetupCharacters();
        CalculateVotes();
        ShowVotes();
    }

    public void SetupCharacters() // fills characters array and randomizes attitudes
    {
        GameObject[] charsFound = GameObject.FindGameObjectsWithTag("Character");
        int charsNum = charsFound.Length;
        characters = new Character[charsNum]; // set length to number of detected characters
        foreach(GameObject thisChar in charsFound)
        {
            if(thisChar.GetComponent<Character>() != null)
            {
                Character selected = thisChar.GetComponent<Character>();
                characters[selected.id] = selected;
                thisChar.GetComponent<Character>().regards = new int[charsNum];
            }
        }
        foreach(Character myChar in characters)
        {
            if(myChar is NPC)
            {
                NPC selected = myChar as NPC;
                selected.RandomizeAttitude();
            }
        }
    }

    public void CalculateVotes() // precalculates NPCs' preferred moves before player phase
    {
        foreach(Character currentChar in characters) // each npc
        {
            if(currentChar is NPC && !currentChar.eliminated)
            {
                int thisID = currentChar.id;
                foreach (Character partner in characters) // offers each other npc a series of deals
                {
                    if(partner is NPC && !partner.eliminated)
                    {
                        int partnerID = partner.id;
                        NPC myPartner = partner as NPC;
                        foreach (Character target in characters) // offer deals
                        {
                            if (target.id != thisID && target.id != partnerID && !target.eliminated)
                            {
                                myPartner.CompareVote(myPartner.regards[currentChar.id], myPartner.regards[target.id], target.id); // partner considers deal
                            }
                        }
                    }
                }
            } 
        }
    }

    public void ShowVotes()
    {
        foreach(Character myChar in characters)
        {
            if(myChar is NPC)
            {
                NPC thisChar = myChar as NPC;
                Debug.Log(thisChar.myName + " is planning to vote for " + characters[thisChar.ShareVote(false)].myName + ". Conviction: " + thisChar.ShareVote(true).ToString());
            }
        }
    }

}
