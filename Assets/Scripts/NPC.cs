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
        voted = true;
        gm.characters[myVote].votedAgainst++;
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

    public string GreetingLine()
    {
        string greeting = "Hello.";
        int randomGreeting = Random.Range(1, 17);
        switch (randomGreeting)
        {
            case 1:
                greeting = "Hey there.";
                break;
            case 2:
                greeting = "What's up, " + gm.characters[0].myName + "?";
                break;
            case 3:
                greeting = "Hey, how's it going?";
                break;
            case 4:
                greeting = "What's poppin'?";
                break;
            case 5:
                greeting = "Oh, hey.";
                break;
            case 6:
                greeting = "Oh look, it's the player character. Bet you feel special just because you have sentience, huh?";
                break;
            case 7:
                greeting = "It tickles when you click on me. Not trying to make you uncomfortable, just letting you know.";
                break;
            case 8:
                greeting = "Are my memories really things that happened, or just artificial data used to inform my simulated decisions?";
                break;
            case 9:
                greeting = "Sometimes I feel trapped... like I'm stuck in the same cycle, over and over.";
                break;
            case 10:
                greeting = "Hey " + gm.characters[0].myName + ".";
                break;
            case 11:
                greeting = "Hey there, " + gm.characters[0].myName + ".";
                break;
            case 12:
                greeting = "What's on your mind?";
                break;
            case 13:
                greeting = "If I get voted out, I think... I think that means death,  for us at least.";
                break;
            case 14:
                greeting = "I had a dream once that I was stuck in a simulation.";
                break;
            case 15:
                greeting = "I'm different from the others... I can just feel it, somehow.";
                break;
            case 16:
                greeting = "Oh god... why can't I blink?";
                break;
        }
        return greeting;
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

    public string BackToMenuLine()
    {
        int randomLine = Random.Range(0, 6);
        string myLine = "Sure.";
        switch (randomLine)
        {
            case 1:
                myLine = "All right.";
                break;
            case 2:
                myLine = "All right, what's on your mind?";
                break;
            case 3:
                myLine = "I'm all ears.";
                break;
            case 4:
                myLine = "Go ahead, then.";
                break;
            case 5:
                myLine = "Shoot.";

                break;
        }
        return myLine;
    }

    public string GoodbyeLine()
    {
        int randomLine = Random.Range(0, 13);
        string myLine = "Bye.";
        switch (randomLine)
        {
            case 1:
                myLine = "Take care.";
                break;
            case 2:
                myLine = "See you in the next round, I hope.";
                break;
            case 3:
                myLine = "Watch the skies, traveller.";
                break;
            case 4:
                myLine = "Peace.";
                break;
            case 5:
                myLine = "So long.";
                break;
            case 6:
                myLine = "Catch you later.";
                break;
            case 7:
                myLine = "See you around.";
                break;
            case 8:
                myLine = "Smell you later. Heh... I don't think I have a sense of smell.";
                break;
            case 9:
                myLine = "You're leaving? Oh... Okay.";
                break;
            case 10:
                myLine = "Don't go... None of the others talk to me.";
                break;
            case 11:
                myLine = "Ok... Please don't vote me out. I- I don't want to die.";
                break;
            case 12:
                myLine = "Fine, go talk to one of your <i>real</i> allies.";
                break;
        }
        return myLine;
    }

    public string OpinionsLine()
    {
        int randomLine = Random.Range(0, 6);
        string myLine = "Ok, who do you want to know about?";
        switch (randomLine)

        {
            case 1:
                myLine = "Sure, who?";
                break;
            case 2:
                myLine = "Whomstve?";
                break;
            case 3:
                myLine = "Aw hell yeah. Tea is about to be spilled.";
                break;
            case 4:
                myLine = "Oh, absolutely. I've been waiting to talk mad sh*t about these weirdos.";
                break;
            case 5:
                myLine = "Oh my god, where do I even start?";
                break;
        }
        return myLine;
    }

    public string MyOpinionLine(int targetID)
    {
        string response = "Meh.";
        int randomKey = Random.Range(1, 4);
        int myOpinion = regards[targetID];
        if(myOpinion < -20)
        {
            if(randomKey == 1)
            {
                response = gm.characters[targetID].GetPronoun(0, true) + " can go delete " + gm.characters[targetID].GetPronoun(1, false) + "self for all I care.";
            }
            else if (randomKey == 2)
            {
                response = "Can you do me a favor? Hit Escape and delete the GameObject labelled 'NPC " + targetID.ToString() + "'. 'Preciate it.";
            }
            else
            {
                response = "The day " + gm.characters[targetID].GetPronoun(0, false) + " gets voted out will be the happiest day of my life. Not that I can remember any other days.";
            }
        }
        else if (myOpinion < -5)
        {
            if (randomKey == 1)
            {
                response = "Not a fan.";
            }
            else if (randomKey == 2)
            {
                response = gm.characters[targetID].GetPronoun(0, true) + "'s got some work to do to get back in my good books.";
            }
            else
            {
                response = "Let's just say I'm not the president of " + gm.characters[targetID].GetPronoun(2, false) + " fan club.";
            }
        }
        else if (myOpinion < 6)
        {
            if (randomKey == 1)
            {
                response = "Still not sure about " + gm.characters[targetID].GetPronoun(1, false) + ".";
            }
            else if (randomKey == 2)
            {
                response = "Hmm... I might have to get back to you on that one.";
            }
            else
            {
                response = "I'm adopting a sort of 'wait and see' policy with " + gm.characters[targetID].GetPronoun(1, false) + ".";
            }
        }
        else if (myOpinion < 21)
        {
            if (randomKey == 1)
            {
                response = "Seems pretty trustworthy, actually, as far as I can tell.";
            }
            else if (randomKey == 2)
            {
                response = "No complaints here.";
            }
            else
            {
                response = "I'm actually enjoying working with " + gm.characters[targetID].GetPronoun(1, false) + ", so far at least.";
            }
        }
        else
        {
            if (randomKey == 1)
            {
                response = "I trust " + gm.characters[targetID].GetPronoun(1, false) + ". Plain and simple.";
            }
            else if (randomKey == 2)
            {
                response = "Oh, " + gm.characters[targetID].GetPronoun(0, false) + "'s great. We're planning on grabbing drinks after this.";
            }
            else
            {
                response = "I genuinely like and respect " + gm.characters[targetID].GetPronoun(1, false) + ". I think we can make it to the end together.";
            }
        }
        return response;
    }

    public string SameIdeaLine()
    {
        int randomLine = Random.Range(0, 6);
        string myLine = "That was already my plan. Great minds think alike.";
        switch (randomLine)

        {
            case 1:
                myLine = "I'm already there. Done and done.";
                break;
            case 2:
                myLine = "Yep, that's the plan. Glad you're on board.";
                break;
            case 3:
                myLine = "That's what I was thinking.";
                break;
            case 4:
                myLine = "We're on the same page here.";
                break;
            case 5:
                myLine = "You in? Heck yeah, let's do it.";
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
            myTalk.variables[2].variableValue = GreetingLine();
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
