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
}
