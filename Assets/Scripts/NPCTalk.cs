using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTalk : MonoBehaviour
{
    public RPGTalk myTalk;
    public string myName;

    private void OnMouseDown()
    {
        myTalk.variables[0].variableValue = myName.ToString();
        myTalk.NewTalk("1", "2");
    }
}
