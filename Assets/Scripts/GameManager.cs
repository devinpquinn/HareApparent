using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Character[] characters; // stores all characters in this game

    private void Start()
    {
        SetupCharacters();
        NPC subj = characters[1] as NPC;
        Debug.Log(subj.FindDisliked(1));
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
                NPC selectedChar = currentChar as NPC;
                int thisID = selectedChar.id;
                foreach (Character partner in characters) // offers each other npc a series of deals
                {
                    if(partner is NPC && !partner.eliminated && partner.id != currentChar.id)
                    {
                        selectedChar.OfferVote((NPC)partner);
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
                Debug.Log(thisChar.myName + " is planning to vote for " + characters[thisChar.myVote].myName + ". Conviction: " + thisChar.voteStrength.ToString());
            }
        }
    }

}
