using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Character
{
    public int myVote; // id of current vote target
    public int voteStrength; // point strength of current vote deal

    public void RandomizeAttitude() // randomize starting attitudes toward other characters
    {
        foreach(Character subj in gm.characters)
        {
            if(subj.id != this.id)
            {
                int randomAttitude = Random.Range(-2, 3);
                this.regards[subj.id] = randomAttitude * 5;
            }
        }
    }

    public void SetVote(int id, int str) // update new chosen vote
    {
        myVote = id;
        voteStrength = str;
    }

    public int FindDisliked(int tried, int partnerID) // find most disliked among remaining, un-offered options
    {
        List<int> alreadyTried = new List<int>();
        int minIndex = 0;
        int minRegard = int.MaxValue;
        while(tried >= 0)
        {
            minIndex = 0;
            minRegard = int.MaxValue;
            for (int i = 0; i < regards.Length; i++)
            {
                if (regards[i] <= minRegard)
                {
                    if (!alreadyTried.Contains(i) && i != this.id && i != partnerID && !gm.characters[i].eliminated)
                    {
                        minIndex = i;
                        minRegard = regards[i];
                    } 
                }
            }
            tried--;
            alreadyTried.Add(minIndex);
        }
        return minIndex;
    }

    public void OfferVote(NPC myPartner) // keep offering votes in descending order of personal preference until agreement is reached
    {
        int offers = 0;
        int remaining = -2;
        foreach(Character scanned in gm.characters)
        {
            if(!scanned.eliminated)
            {
                remaining++; // number of potential targets
            }
        }
        while(offers < remaining)
        {
            int myTarget = FindDisliked(offers, myPartner.id);
            if(myPartner.CompareVote(myPartner.regards[this.id], myPartner.regards[myTarget], myTarget))
            {
                //Debug.Log(myName + " convinces " + myPartner.myName + " to vote for " + gm.characters[myTarget].myName);
                break;
            }
            else
            {
                //Debug.Log(myName + " fails to convince " + myPartner.myName + " to vote for " + gm.characters[myTarget].myName);
            }
            offers++;
        }
    }

    public bool CompareVote(int proposer, int target, int targetID) // function for comparing various votes
    {
        int str = proposer - target;
        if(str > voteStrength)
        {
            myVote = targetID;
            voteStrength = str;
            return true;
        }
        if(str == voteStrength)
        {
            int tiebreaker = Random.Range(0, 2);
            if(tiebreaker == 0)
            {
                myVote = targetID;
                voteStrength = str;
                return true;
            }
        }
        return false;
    }

    public void CastVote()// cast vote and increment acccordingly
    {
        gm.characters[myVote].votedAgainst++;
        this.voted = true;
        myTalk.variables[0].variableValue = myName;
        myTalk.variables[1].variableValue = gm.characters[myVote].myName;
        gm.characters[myVote].regards[id] -= 20;
        foreach (Character alliedChar in gm.characters)
        {
            if (alliedChar is NPC && !alliedChar.eliminated)
            {
                NPC alliedNPC = alliedChar as NPC;
                if (alliedNPC.myVote == gm.characters[myVote].id)
                {
                    alliedNPC.regards[this.id] += 10; 
                }
            }
        }
        myTalk.NewTalk("40", "41", txt, gm.OnVote);
    }

    public void CastFinalVote()
    {
        myVote = FindDisliked(1, id);
        gm.characters[myVote].votedAgainst++;
        this.voted = true;
        myTalk.variables[0].variableValue = myName;
        myTalk.variables[1].variableValue = gm.characters[myVote].myName;
        myTalk.NewTalk("57", "57", txt, gm.OnFinalVote);
    }

    public string DealLine()
    {
        int randomLine = Random.Range(0, 6);
        string myLine = "Who's the target?";
        switch(randomLine)
        {
            case 1:
                myLine = "Who do you have in mind?";
                break;
            case 2:
                myLine = "Who do you want to go for?";
                break;
            case 3:
                myLine = "All right, who do you want to vote for?";
                break;
            case 4:
                myLine = "Who should we go for?";
                break;
            case 5:
                myLine = "Sure, who are you thinking of voting for?";
                break;
        }
        return myLine;
    }

    private void OnMouseUp() // begin conversation
    {
        if (!myTalk.dialogerObj.activeInHierarchy && !gm.locked)
        {
            myTalk.variables[0].variableValue = myName.ToString();
            PC playerChar = gm.characters[0] as PC;
            playerChar.talkingTo = this.id;
            int randomGreeting = Random.Range(1, 17);
            switch (randomGreeting)
            {
                case 1:
                    myTalk.variables[2].variableValue = "Hey there.";
                    break;
                case 2:
                    myTalk.variables[2].variableValue = "What's up, " + gm.characters[0].myName + "?";
                    break;
                case 3:
                    myTalk.variables[2].variableValue = "Hey, how's it going?";
                    break;
                case 4:
                    myTalk.variables[2].variableValue = "What's poppin'?";
                    break;
                case 5:
                    myTalk.variables[2].variableValue = "Oh, hey.";
                    break;
                case 6:
                    myTalk.variables[2].variableValue = "Oh look, it's the player character. Bet you feel special just because you have sentience, huh?";
                    break;
                case 7:
                    myTalk.variables[2].variableValue = "It tickles when you click on me. Not trying to make you uncomfortable, just letting you know.";
                    break;
                case 8:
                    myTalk.variables[2].variableValue = "Are my memories really things that happened, or just artificial data used to inform my simulated decisions?";
                    break;
                case 9:
                    myTalk.variables[2].variableValue = "Sometimes I feel trapped... like I'm stuck in the same cycle, over and over.";
                    break;
                case 10:
                    myTalk.variables[2].variableValue = "Hey " + gm.characters[0].myName + ".";
                    break;
                case 11:
                    myTalk.variables[2].variableValue = "Hey there, " + gm.characters[0].myName + ".";
                    break;
                case 12:
                    myTalk.variables[2].variableValue = "What's on your mind?";
                    break;
                case 13:
                    myTalk.variables[2].variableValue = "If I get voted out, I think... I think that means death,  for us at least.";
                    break;
                case 14:
                    myTalk.variables[2].variableValue = "I had a dream once that I was stuck in a simulation.";
                    break;
                case 15:
                    myTalk.variables[2].variableValue = "I'm different from the others... I can just feel it, somehow.";
                    break;
                case 16:
                    myTalk.variables[2].variableValue = "Oh god... why can't I blink?";
                    break;
            }
            myTalk.NewTalk("62", "66");
        }
    }

    private void OnMouseEnter()
    {
        if (!myTalk.dialogerObj.activeInHierarchy && !gm.locked)
        {
            this.transform.localScale = new Vector3(5.5f, 5.5f, 5.5f);
        }
    }

    private void OnMouseExit()
    {
        this.transform.localScale = new Vector3(5, 5, 5);
    }
}
