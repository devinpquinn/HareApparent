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

    public void CalculateVotes() // precalculates NPCs' preferred moves before player phase
    {
        foreach(NPC currentChar in characters)
        {

        }
    }

}
