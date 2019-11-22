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

    public void RoundReset() // reset for next round of play
    {
        voteStrength = -1;
    }

    public void SetVote(int id, int str) // update new chosen vote
    {
        myVote = id;
        voteStrength = str;
    }

    private int FindDisliked(int tried) // find most disliked among remaining, un-offered options
    {
        List<int> alreadyOffered = new List<int>();
        int lowest = 0;
        while (tried >= 0)
        {
            lowest = 0;
            for (int i = 0; i < regards.Length; i++)
            {
                if (i != this.id && !alreadyOffered.Contains(i) && i <= regards[lowest] && !gm.characters[i].eliminated)
                {
                    lowest = i;
                }
            }
            alreadyOffered.Add(lowest);
            tried -= 1;
        }
        return lowest;
    }

    public void OfferVote(NPC myPartner) // keep offering votes in descending order of personal preference until agreement is reached
    {
        int offers = 0;
        while(offers < gm.characters.Length)
        {
            int myTarget = FindDisliked(offers);
            if(CompareVote(myPartner.regards[this.id], myPartner.regards[myTarget], myTarget))
            {
                Debug.Log(myName + " convinces " + myPartner.myName + " to vote for " + gm.characters[myTarget].myName);
                break;
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
