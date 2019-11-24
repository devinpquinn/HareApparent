using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTalk : MonoBehaviour
{
    public RPGTalk myTalk;
    public string myName;

    private void Start()
    {
        myName = this.GetComponent<NPC>().myName;
    }

    private void OnMouseUp()
    {
        if(!myTalk.isPlaying)
        {
            myTalk.variables[0].variableValue = myName.ToString();
            myTalk.NewTalk("1", "2");
        }
    }
}
