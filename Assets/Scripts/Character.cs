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
    bool eliminated = false; // has this character been eliminated from contention?

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
    
}

public enum Gender {Male, Female, Other}
