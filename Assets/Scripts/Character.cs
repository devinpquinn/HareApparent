using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public GameManager gm; // management object
    [Range(0, 5)]
    public int id; // character id
    public string myName; // character name
    public Gender myGender;
    public int[] regards; // stores opinions of other characters
    public bool eliminated = false; // has this character been eliminated from contention?
    public bool voted = false; // has this character already voted in this round?
    public int votedAgainst = 0; // how many votes have been cast against this character in this round?
    public RPGTalk myTalk;
    public TextAsset txt;

    private void Start()
    {
        myTalk = gm.myTalk;
        txt = gm.myTalk.txtToParse;
    }

    public string GetPronoun(int form, bool cap) // form = he/him/his, cap = capitalized?
    {
        switch(form)
        {
            case 0:
                if(myGender == Gender.Male)
                {
                    if(cap)
                    {
                        return "He";
                    }
                    else
                    {
                        return "he";
                    }
                }
                if (myGender == Gender.Female)
                {
                    if (cap)
                    {
                        return "She";
                    }
                    else
                    {
                        return "she";
                    }
                }
                if (myGender == Gender.Other)
                {
                    if (cap)
                    {
                        return "They";
                    }
                    else
                    {
                        return "they";
                    }
                }
                break;
            case 1:
                if (myGender == Gender.Male)
                {
                    if (cap)
                    {
                        return "Him";
                    }
                    else
                    {
                        return "him";
                    }
                }
                if (myGender == Gender.Female)
                {
                    if (cap)
                    {
                        return "Her";
                    }
                    else
                    {
                        return "her";
                    }
                }
                if (myGender == Gender.Other)
                {
                    if (cap)
                    {
                        return "Them";
                    }
                    else
                    {
                        return "them";
                    }
                }
                break;
            case 2:
                if (myGender == Gender.Male)
                {
                    if (cap)
                    {
                        return "His";
                    }
                    else
                    {
                        return "his";
                    }
                }
                if (myGender == Gender.Female)
                {
                    if (cap)
                    {
                        return "Hers";
                    }
                    else
                    {
                        return "hers";
                    }
                }
                if (myGender == Gender.Other)
                {
                    if (cap)
                    {
                        return "Theirs";
                    }
                    else
                    {
                        return "theirs";
                    }
                }
                break;
        }
        return "[MISSING]";
    }

    public void RoundReset() // reset for next round of play
    {
        voted = false;
        votedAgainst = 0;
        if(this is NPC && !eliminated)
        {
            NPC amNPC = this as NPC;
            amNPC.myVote = amNPC.FindDisliked(0, this.id);
            amNPC.voteStrength = 0 - regards[amNPC.myVote] - 1;
        }
    }
    
}

public enum Gender {Male, Female, Other}
