using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Character
{
    public int myVote; // id of current vote target
    public int voteStrength; // point strength of current vote deal

    private void Start()
    {
        //RoundReset();
    }

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
                    if (!alreadyTried.Contains(i) && i != this.id && i != partnerID)
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
                Debug.Log(myName + " convinces " + myPartner.myName + " to vote for " + gm.characters[myTarget].myName);
                break;
            }
            else
            {
                Debug.Log(myName + " fails to convince " + myPartner.myName + " to vote for " + gm.characters[myTarget].myName);
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

}
