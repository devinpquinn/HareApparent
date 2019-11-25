using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Character[] characters; // stores all characters in this game
    public RPGTalk myTalk;
    public bool locked = false; // is control taken away from the player?

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
                selected.RoundReset();
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
                Debug.Log("<b> " + thisChar.myName + " is planning to vote for " + characters[thisChar.myVote].myName + ". Conviction: " + thisChar.voteStrength.ToString() + "</b>");
            }
        }
    } // lists chosen votes for debugging

    public int SetVoteOptions() // setup remaining characters as dialogue choices
    {
        int added = 0;
        for(int i = 0; i < characters.Length; i++)
        {
            Character thisChar = characters[i];
            if(thisChar is NPC && !thisChar.eliminated)
            {
                myTalk.variables[2 + added].variableValue = thisChar.myName;
                added++;
            }
        }
        return added;
    }

    public void ConductVote()
    {
        for(int i = 0; i < characters.Length; i++)
        {
            Character selectedChar = characters[i];
            if(!selectedChar.eliminated)
            {
                if (selectedChar is PC)
                {
                    PC playerChar = selectedChar as PC;
                    playerChar.PlayerVote();
                }
                else if (selectedChar is NPC)
                {
                    NPC npcChar = selectedChar as NPC;
                    npcChar.CastVote();
                }
            }
        }
    }

}
