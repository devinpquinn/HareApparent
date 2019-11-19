using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Basic Info")]
    public GameManager gm; // management object
    public int id; // character id
    public string myName; // character name
    public int gender; // 0 = male pronouns, 1 = female pronouns, 2 = neutral pronouns
    public int[] regards; // stores opinions of other characters
    bool eliminated = false; // has this character been eliminated from contention?

    public string GetPronoun(int form, bool cap) // form = he/him/his, cap = capitalized?
    {
        switch(form)
        {
            case 0:
                if(gender == 0)
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
                if (gender == 1)
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
                if (gender == 2)
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
                if (gender == 0)
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
                if (gender == 1)
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
                if (gender == 2)
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
                if (gender == 0)
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
                if (gender == 1)
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
                if (gender == 2)
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
