using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Basic Info")]
    public GameManager gm;
    public int id;
    public string myName;
    public int gender;
    public int[] regards;
    bool eliminated = false;

    [Header("Voting Info")]
    private int myVote;
    private int voteStrength;
}
