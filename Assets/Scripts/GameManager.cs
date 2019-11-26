using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public Character[] characters; // stores all characters in this game
    public RPGTalk myTalk;
    public bool locked = false; // is control taken away from the player?
    private int toEliminate;

    public UnityEvent OnVote;
    public UnityEvent OnVoteEnd;

    private void Start()
    {
        myTalk.OnMadeChoice += OnMadeChoice;
        SetupCharacters();
        CalculateVotes();
        ShowVotes();
    }

    private void OnMadeChoice(string questionID, int choiceNumber)
    {
        switch (questionID)
        {
            case "ReadyToVote":
                if (choiceNumber == 0)
                {
                    ConductVote();
                }
                break;
            case "Vote":
                myTalk.variables[0].variableValue = characters[0].myName;
                Character targeted = characters[1 + choiceNumber];
                myTalk.variables[1].variableValue = targeted.myName;
                targeted.votedAgainst++;
                targeted.regards[0] -= 20;
                myTalk.NewTalk("41", "41");
                break;
        }
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

    public void ConductVote() // cycle through each character's vote
    {
        bool doneVoting = true;
        foreach(Character selectedChar in characters)
        {
            if(!selectedChar.eliminated && !selectedChar.voted)
            {
                doneVoting = false;
                myTalk.variables[1].variableValue = selectedChar.myName;
                if (selectedChar is PC)
                {
                    PC playerChar = selectedChar as PC;
                    playerChar.PlayerVote();
                    break;
                }
                else if (selectedChar is NPC)
                {
                    NPC npcChar = selectedChar as NPC;
                    npcChar.CastVote();
                    break;
                }
            }
        }
        if(doneVoting)
        {
            TallyVote();
        }
    }  

    public void Eliminate()
    {
        characters[toEliminate].eliminated = true;
        characters[toEliminate].GetComponent<Animator>().SetBool("elim", true);
    }

    public void TallyVote() // resolve votes cast
    {
        locked = true;
        int highestIndex = 0;
        int mostVotes = -1;
        bool tied = false;
        foreach(Character thisChar in characters)
        {
            if(thisChar.votedAgainst > mostVotes)
            {
                mostVotes = thisChar.votedAgainst;
                highestIndex = thisChar.id;
                tied = false;
            }
            else if(thisChar.votedAgainst == mostVotes)
            {
                tied = true;
            }
        }
        if(!tied)
        {
            toEliminate = highestIndex;
            myTalk.variables[1].variableValue = characters[highestIndex].myName;
            myTalk.NewTalk("43", "44", myTalk.txtToParse, OnVoteEnd);
        }
        else
        {
            myTalk.variables[0].variableValue = "";
            List<int> tiedIndexList = new List<int>();
            List<Character> tiedCharList = new List<Character>();
            int numTied = 0;
            foreach (Character tiedChar in characters)
            {
                if (tiedChar.votedAgainst == characters[highestIndex].votedAgainst)
                {
                    numTied++;
                    tiedIndexList.Add(tiedChar.id);
                    tiedCharList.Add(tiedChar);
                }
            }
            for(int i = 0; i < tiedCharList.Count; i++)
            {
                string nameToAdd = tiedCharList[i].myName;
                myTalk.variables[0].variableValue += nameToAdd;

                if (numTied == 2)
                {
                    myTalk.variables[0].variableValue += ", and ";
                }
                else if (numTied > 2)
                {
                    myTalk.variables[0].variableValue += ", ";
                }   
                numTied--;
            }
            int randomPick = Random.Range(0, tiedCharList.Count);
            toEliminate = tiedCharList[randomPick].id;
            myTalk.variables[1].variableValue = tiedCharList[randomPick].myName;
            myTalk.NewTalk("46", "49", myTalk.txtToParse, OnVoteEnd);
        }
        locked = false;
    }

}
