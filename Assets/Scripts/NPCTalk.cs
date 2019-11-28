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
            int randomGreeting = Random.Range(1, 16);
            switch(randomGreeting)
            {
                case 1:
                    myTalk.variables[2].variableValue = "Hey there.";
                    break;
                case 2:
                    myTalk.variables[2].variableValue = "What's up?";
                    break;
                case 3:
                    myTalk.variables[2].variableValue = "Hey, how's it going?";
                    break;
                case 4:
                    myTalk.variables[2].variableValue = "What's poppin'?";
                    break;
                case 5:
                    myTalk.variables[2].variableValue = "Oh, hey.";
                    break;
                case 6:
                    myTalk.variables[2].variableValue = "Oh look, it's the player character. Bet you feel special just because you have sentience, huh?";
                    break;
                case 7:
                    myTalk.variables[2].variableValue = "It tickles when you click on me. Not trying to make you uncomfortable, just letting you know.";
                    break;
                case 8:
                    myTalk.variables[2].variableValue = "Are my memories really things that happened, or just artificial data used to inform my simulated decisions?";
                    break;
                case 9:
                    myTalk.variables[2].variableValue = "Sometimes I feel trapped... like I'm stuck in the same cycle, over and over.";
                    break;
                case 10:
                    myTalk.variables[2].variableValue = "Hey " + gm.characters[0].myName + ".";
                    break;
                case 11:
                    myTalk.variables[2].variableValue = "Hey there, " + gm.characters[0].myName + ".";
                    break;
                case 12:
                    myTalk.variables[2].variableValue = "What's on your mind.";
                    break;
                case 13:
                    myTalk.variables[2].variableValue = "If I get voted out, I think... I think that means death,  for us at least.";
                    break;
                case 14:
                    myTalk.variables[2].variableValue = "I had a dream once that I was stuck in a simulation.";
                    break;
                case 15:
                    myTalk.variables[2].variableValue = "I'm different from the others... I can just feel it, somehow.";
                    break;
            }
            myTalk.NewTalk("62", "66");
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
