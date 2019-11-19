using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Character[] characters; // stores all characters in this game

    public void SetupCharacters() // fills characters array
    {
        foreach(GameObject thisChar in GameObject.FindGameObjectsWithTag("Character"))
        {
            if(thisChar.GetComponent<Character>() != null)
            {
                Character selected = thisChar.GetComponent<Character>();
                characters[selected.id] = selected;
            }
        }
    }

    public void ResetVotes() // sets each character's first vote choice at each round start
    {
        foreach (NPC currentChar in characters)
        {
            
        }
    }

    public void CalculateVotes() // precalculates NPCs' preferred moves before player phase
    {
        foreach(NPC currentChar in characters)
        {
            int thisID = currentChar.id;
            foreach(NPC partner in characters)
            {
                int partnerID = partner.id;
                foreach(Character target in characters)
                {
                    if(target.id != thisID && target.id != partnerID)
                    {
                        partner.CompareVote(partner.regards[currentChar.id], partner.regards[target.id], target.id);
                    }
                }
            }
        }
    }

}
