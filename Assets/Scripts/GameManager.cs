using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Character[] characters; // stores all characters in this game

    public void SetupCharacters() // fills characters array
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
    }

    public void ResetVotes() // sets each character's first vote choice at each round start
    {
        foreach (NPC currentChar in characters) // for each npc
        {
            int thisID = currentChar.id;
            foreach (Character target in characters) // cycles through potential targets
            {
                currentChar.CompareVote(-1, currentChar.regards[target.id], target.id); // select most disliked character
            }
        }
    }

    public void CalculateVotes() // precalculates NPCs' preferred moves before player phase
    {
        foreach(NPC currentChar in characters) // each npc
        {
            int thisID = currentChar.id;
            foreach(NPC partner in characters) // offers each other npc a series of deals
            {
                int partnerID = partner.id;
                foreach(Character target in characters) // offer deals
                {
                    if(target.id != thisID && target.id != partnerID)
                    {
                        partner.CompareVote(partner.regards[currentChar.id], partner.regards[target.id], target.id); // partner considers deal
                    }
                }
            }
        }
    }

}
