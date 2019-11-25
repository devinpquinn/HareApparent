using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC : Character
{
    private RPGTalk myTalk;

    private void Start()
    {
        myTalk = gm.myTalk;
    }

    public void PlayerVote()
    {
        myTalk.variables[0].variableValue = myName;
        switch(gm.SetVoteOptions())
        {
            case 2:
                myTalk.NewTalk("34", "38");
                break;
            case 3:
                myTalk.NewTalk("27", "32");
                break;
            case 4:
                myTalk.NewTalk("19", "25");
                break;
            case 5:
                myTalk.NewTalk("10", "17");
                break;
        }
    }
}
