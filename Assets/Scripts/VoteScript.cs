using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteScript : MonoBehaviour
{
    public RPGTalk myTalk;
    public GameManager gm;

    private void Start()
    {
        myTalk.OnMadeChoice += MyTalk_OnMadeChoice;
    }

    private void MyTalk_OnMadeChoice(string questionID, int choiceNumber)
    {
        if(questionID == "ReadyToVote" && choiceNumber == 0)
        {
            
        }
    }

    private void OnMouseUp() // begin conversation
    {
        if(!myTalk.isPlaying && !gm.locked)
        {
            myTalk.NewTalk("4", "6");
        }  
    }

    public void VotingPhase() // conduct the vote
    {

    }
}
