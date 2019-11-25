using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTalk : MonoBehaviour
{
    public RPGTalk myTalk;
    public string myName;
    public GameManager gm;

    private void Start()
    {
        myName = this.GetComponent<NPC>().myName;
    }

    private void OnMouseUp() // begin conversation
    {
        if(!myTalk.isPlaying && !gm.locked)
        {
            myTalk.variables[0].variableValue = myName.ToString();
            myTalk.NewTalk("1", "2");
        }
    }
}
