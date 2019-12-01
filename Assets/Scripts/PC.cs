using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC : Character
{

    public int talkingTo;

    public void PlayerVote() // player casts vote
    {
        voted = true;
        switch(gm.SetVoteOptions())
        {
            case 2:
                myTalk.NewTalk("34", "38", txt, gm.OnVote);
                break;
            case 3:
                myTalk.NewTalk("27", "32", txt, gm.OnVote);
                break;
            case 4:
                myTalk.NewTalk("19", "25", txt, null);
                break;
        }
    }

    public void OfferDeal()
    {
        NPC myPartner = gm.characters[talkingTo] as NPC;
        myTalk.variables[2].variableValue = myPartner.DealLine();
        switch(gm.SetDealOptions(talkingTo))
        {
            case 3:
                myTalk.NewTalk("68", "72");
                break;
            case 2:
                myTalk.NewTalk("74", "77");
                break;
            case 1:
                myTalk.NewTalk("79", "81");
                break;
        }
    }
}
