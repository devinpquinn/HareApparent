using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public Character[] characters; // stores all characters in this game
    public RPGTalk myTalk;
    public GameObject gameOver;
    public bool locked = false; // is control taken away from the player?
    public string myName;
    private int toEliminate;

    public UnityEvent OnVote;
    public UnityEvent OnVoteEnd;
    public UnityEvent OnFinalVote;
    public UnityEvent OnFinalResult;

    private void Start()
    {
        myTalk.OnMadeChoice += OnMadeChoice;
        SetupCharacters();
        CalculateVotes();
        ShowVotes();
    }

    private void OnMadeChoice(string questionID, int choiceNumber)
    {
        PC myPlayer = characters[0] as PC;
        NPC partnerNPC = characters[myPlayer.talkingTo] as NPC;
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
                string chosenName = myTalk.variables[3 + choiceNumber].variableValue;
                Character targeted = characters[0];
                foreach(Character nameCheck in characters)
                {
                    if(nameCheck.myName.Equals(chosenName))
                    {
                        targeted = nameCheck;
                    }
                }
                myTalk.variables[1].variableValue = targeted.myName;
                targeted.votedAgainst++;
                targeted.regards[0] -= 20;
                foreach(Character alliedChar in characters)
                {
                    if(alliedChar is NPC && !alliedChar.eliminated)
                    {
                        NPC alliedNPC = alliedChar as NPC;
                        if (alliedNPC.myVote == targeted.id);
                        alliedNPC.regards[0] += 10;
                    }
                }
                myTalk.NewTalk("41", "41", myTalk.txtToParse, OnVote);
                break;
            case "Home":
                if(choiceNumber == 0)
                {
                    myPlayer.OfferDeal();
                }
                else if (choiceNumber == 1)
                {
                    int partnerTarget = partnerNPC.myVote;
                    if(partnerTarget == 0)
                    {
                        partnerTarget = partnerNPC.FindDisliked(0, 0);
                    }
                    myTalk.variables[1].variableValue = characters[partnerTarget].myName;
                    myTalk.NewTalk("91", "94");
                }
                else if(choiceNumber == 2)
                {
                    myTalk.variables[2].variableValue = partnerNPC.OpinionsLine();
                    switch (SetDealOptions(partnerNPC.id))
                    {
                        case 3:
                            myTalk.NewTalk("106", "110");
                            break;
                        case 2:
                            myTalk.NewTalk("112", "115");
                            break;
                        case 1:
                            myTalk.NewTalk("117", "119");
                            break;
                    }
                }
                else if (choiceNumber == 3)
                {
                    myTalk.variables[2].variableValue = partnerNPC.GoodbyeLine();
                    myTalk.NewTalk("104", "104");
                }
                break;
            case "VoteDeal":
                string targetName = myTalk.variables[3 + choiceNumber].variableValue;
                int targetCharID = FindByName(targetName).id;
                int NPCThinking = partnerNPC.myVote;
                if(NPCThinking == 0)
                {
                    NPCThinking = partnerNPC.FindDisliked(0, 0);
                }
                if (targetCharID == NPCThinking)
                {
                    partnerNPC.CompareVote(partnerNPC.regards[0], partnerNPC.regards[targetCharID], targetCharID);
                    myTalk.variables[2].variableValue = partnerNPC.SameIdeaLine();
                    myTalk.NewTalk("126", "128");
                    break;
                }
                if (partnerNPC.CompareVote(partnerNPC.regards[0], partnerNPC.regards[targetCharID], targetCharID))
                {
                    myTalk.NewTalk("83", "85");
                }
                else
                {
                    myTalk.NewTalk("87", "89");
                }
                if(choiceNumber == GetRemaining() - 2)
                {
                    myTalk.variables[2].variableValue = partnerNPC.BackToMenuLine();
                    myTalk.NewTalk("62", "66");
                }
                break;
            case "OfferedDeal":
                if(choiceNumber == 2)
                {
                    myTalk.variables[2].variableValue = partnerNPC.GoodbyeLine();
                    myTalk.NewTalk("104", "104");
                }
                break;
            case "BackHomeOrExit":
                if(choiceNumber == 0)
                {
                    myTalk.variables[2].variableValue = partnerNPC.BackToMenuLine();
                    myTalk.NewTalk("62", "66");
                }
                else
                {
                    myTalk.variables[2].variableValue = partnerNPC.GoodbyeLine();
                    myTalk.NewTalk("104", "104");
                }
                break;
            case "DealRejected":
                if(choiceNumber == 0)
                {
                    myTalk.variables[2].variableValue = partnerNPC.DealLine();
                    if (GetRemaining() == 5)
                    {
                        myTalk.NewTalk("68", "72");
                    }
                    else if (GetRemaining() == 4)
                    {
                        myTalk.NewTalk("74", "77");
                    }
                    else if (GetRemaining() == 3)
                    {
                        myTalk.NewTalk("79", "81");
                    }
                }
                else
                {
                    myTalk.variables[2].variableValue = partnerNPC.GoodbyeLine();
                    myTalk.NewTalk("104", "104");
                }
                break;
            case "Opinions":
                if(choiceNumber != GetRemaining() - 2)
                {
                    int askedID = FindByName(myTalk.variables[3 + choiceNumber].variableValue).id;
                    myTalk.variables[2].variableValue = partnerNPC.MyOpinionLine(askedID);
                    myTalk.NewTalk("121", "124");
                }
                else
                {
                    myTalk.variables[2].variableValue = partnerNPC.BackToMenuLine();
                    myTalk.NewTalk("62", "66");
                }
                break;
            case "OpinionGiven":
                if(choiceNumber == 0)
                {
                    myTalk.variables[2].variableValue = partnerNPC.OpinionsLine();
                    switch (SetDealOptions(partnerNPC.id))
                    {
                        case 3:
                            myTalk.NewTalk("106", "110");
                            break;
                        case 2:
                            myTalk.NewTalk("112", "115");
                            break;
                        case 1:
                            myTalk.NewTalk("117", "119");
                            break;
                    }
                }
                else if (choiceNumber == 1)
                {
                    myTalk.variables[2].variableValue = partnerNPC.BackToMenuLine();
                    myTalk.NewTalk("62", "66");
                }
                else {
                    myTalk.variables[2].variableValue = partnerNPC.GoodbyeLine();
                    myTalk.NewTalk("104", "104");
                }
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

    public void ResetVotes() // recalculate votes
    {
        int remaining = 0;
        foreach(Character countChar in characters)
        {
            if(!countChar.eliminated)
            {
                remaining++;
            }
        }
        foreach (Character thisChar in characters)
        {
            thisChar.RoundReset();
        }
        if (remaining <= 2) 
        {
            Debug.Log("Down to final two");
            myTalk.NewTalk("52", "55", myTalk.txtToParse, OnFinalVote);
        }
        else
        {
            NextRound();
        }
    }

    public int GetRemaining()
    {
        int remaining = 0;
        foreach(Character countChar in characters)
        {
            if(!countChar.eliminated)
            {
                remaining++;
            }
        }
        return remaining;
    } // how many total characters remain in contention?

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

    public void NextRound()
    {
        CalculateVotes();
        myTalk.NewTalk("10", "10", myTalk.txtToParse, null);
        locked = false;
    }

    public void ShowVotes() // lists chosen votes for debugging
    {
        foreach(Character myChar in characters)
        {
            if(myChar is NPC)
            {
                NPC thisChar = myChar as NPC;
                Debug.Log("<b> " + thisChar.myName + " is planning to vote for " + characters[thisChar.myVote].myName + ". Conviction: " + thisChar.voteStrength.ToString() + "</b>");
            }
        }
    } 

    public Character FindByName(string charName)
    {
        Character targeted = characters[0];
        foreach (Character nameCheck in characters)
        {
            if (nameCheck.myName.Equals(charName))
            {
                targeted = nameCheck;
            }
        }
        return targeted;
    } // find a character by their name

    public int SetDealOptions(int partner)
    {
        int added = 0;
        for (int i = 0; i < characters.Length; i++)
        {
            Character thisChar = characters[i];
            if (thisChar is NPC && !thisChar.eliminated && thisChar.id != partner)
            {
                myTalk.variables[3 + added].variableValue = thisChar.myName;
                added++;
            }
        }
        return added;
    }

    public int SetVoteOptions() // setup remaining characters as dialogue choices
    {
        int added = 0;
        for(int i = 0; i < characters.Length; i++)
        {
            Character thisChar = characters[i];
            if(thisChar is NPC && !thisChar.eliminated)
            {
                myTalk.variables[3 + added].variableValue = thisChar.myName;
                added++;
            }
        }
        return added;
    }

    public void EndGame()
    {
        locked = true;
        gameOver.SetActive(true);
    } // finish game

    public void ConductFinalVote() // conduct final vote
    {
        bool doneVoting = true;
        foreach (Character selectedChar in characters)
        {
            if (selectedChar.eliminated && !selectedChar.voted)
            {
                selectedChar.GetComponent<Animator>().SetBool("elim", false);
                doneVoting = false;
                if (selectedChar is NPC)
                {
                    NPC npcChar = selectedChar as NPC;
                    npcChar.CastFinalVote();
                    break;
                }
            }
        }
        if (doneVoting)
        {
            FinalTallyVote();
        }
    }

    public void ConductVote() // cycle through each character's vote
    {
        bool doneVoting = true;
        foreach(Character selectedChar in characters)
        {
            doneVoting = true;
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
        if(toEliminate == 0)
        {
            EndGame();
        }
        else
        {
            characters[toEliminate].eliminated = true;
            characters[toEliminate].GetComponent<Animator>().SetBool("elim", true);
            Invoke("ResetVotes", 2f);
        }
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
    }

    public void FinalTallyVote()
    {
        Character[] finalists = new Character[2];
        int added = 0;
        foreach(Character finalChar in characters)
        {
            if(!finalChar.eliminated)
            {
                finalists[0 + added] = finalChar;
                added++;
            }
        }
        if(finalists[0].votedAgainst > finalists[1].votedAgainst)
        {
            toEliminate = finalists[0].id;
            myTalk.variables[1].variableValue = finalists[0].myName;
        }
        else
        {
            toEliminate = finalists[1].id;
            myTalk.variables[1].variableValue = finalists[1].myName;
        }
        myTalk.NewTalk("59", "60", myTalk.txtToParse, OnFinalResult);
    } // resolve final vote

    private void OnMouseEnter()
    {
        if (!myTalk.dialogerObj.activeInHierarchy && !locked)
        {
            this.transform.localScale = new Vector3(5.5f, 5.5f, 5.5f);
        }
    }

    private void OnMouseExit()
    {
        this.transform.localScale = new Vector3(5, 5, 5);
    }

    private void OnMouseUp() // begin conversation
    {
        if (!myTalk.dialogerObj.activeInHierarchy && !locked)
        {
            myTalk.variables[0].variableValue = myName;
            myTalk.NewTalk("4", "6");
        }
    }

}
