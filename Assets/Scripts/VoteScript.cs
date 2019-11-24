using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteScript : MonoBehaviour
{
    public RPGTalk myTalk;

    private void OnMouseUp() // begin conversation
    {
        if(!myTalk.isPlaying)
        {
            myTalk.NewTalk("4", "6");
        }  
    }
}
