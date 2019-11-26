using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteScript : MonoBehaviour
{
    public RPGTalk myTalk;
    public GameManager gm;
    public string myName;

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

    private void OnMouseUp() // begin conversation
    {
        if(!myTalk.dialogerObj.activeInHierarchy && !gm.locked)
        {
            myTalk.variables[0].variableValue = myName;
            myTalk.NewTalk("4", "6");
        }  
    }
}
