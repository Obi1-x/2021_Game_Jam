using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Difficulty variable:
//  time interval,
//  no of nonbugs, X can be used instead of (chances of having a bug).
//  no of bugs,
//  chances of having a bug, ||Used by shouldBeBug in operation. 
//  bullet speed, X should be moved to player controls (Power ups)
//  vertical container width, X should be part of power ups.
//  bug tolerance level of complier (%). Proportional to game level.

//Some variables are easy by default, while others are hard. 
//Users pay to reduce the hard variables and play to increase the easy variables.

//this.noOfNonBugs = 15;

public class DifficultyVariations
{
    //Each level is represented by the competence level.
    private int competenceLevel;

    private float interval;
    private int noOfBugs;
    private float bugTolerance;

    public float Interval { get => interval; set => interval = value; }
    public int NoOfBugs { get => noOfBugs; set => noOfBugs = value; }
    public float BugTolerance { get => bugTolerance; set => bugTolerance = value; }

    public string fairWell = "Congratulations, you have reached the end of this game. Thank you for playing. *To be continued..*";

    public DifficultyVariations() //Default property settings.
    {
        this.interval = 0.2f; //Was on 1f
        this.noOfBugs = 5;
        this.bugTolerance = 0.8f;
    }

    public void adjustDifficulty(int competence_level)
    {
        if (competence_level > competenceLevel)
        {
            interval -= 0.2f;
            if (interval < 0.2f)
            {
                interval = 1f;
                noOfBugs += 2;
                if (noOfBugs >= 13)
                {
                    noOfBugs = 5;
                    bugTolerance -= 0.2f;
                    if (bugTolerance < 0.2f)
                    {
                        bugTolerance = 0.8f;
                    }
                }
            }
        }
        competenceLevel = competence_level;
        Debug.Log("interval: " + interval);
        Debug.Log("NoOfBugs: " + noOfBugs);
        Debug.Log("BugTolerance: " + bugTolerance);
    }

    public void resetDifficulty()
    {
        interval = 0.2f;
        noOfBugs = 5;
        bugTolerance = 0.8f;
    }
}
