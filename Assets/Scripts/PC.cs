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
                myTalk.NewTalk("19", "25", txt, gm.OnVote);
                break;
        }
    }

    public void OfferDeal()
    {
        switch(gm.SetDealOptions(talkingTo))
        {
            case 3:
                break;
            case 2:
                break;
            case 1:
                break;
        }
    }
}
