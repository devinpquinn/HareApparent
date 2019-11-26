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
        if(!myTalk.dialogerObj.activeInHierarchy && !gm.locked)
        {
            myTalk.variables[0].variableValue = myName.ToString();
            myTalk.NewTalk("1", "2");
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
