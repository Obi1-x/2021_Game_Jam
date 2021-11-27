using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Operation : MonoBehaviour
{
    public static bool aboutToChange;
    public static bool demoPause;
    private bool isInInneroutine;
    private bool trigger_1;
    private bool toContinue;
    private bool toQuit;

    private int bugsDeployed;
    private int nonBugsDeployed;
    private float totalPoints, gamePoints;
    private int gameLevel;
    public Transform codeBlock;
    public Transform pausedCard; 
    private DifficultyVariations difficulty;

    public static string pausedReason;
    private const string GAMEOVER = "GAMEOVER";
    private const string GAMEPAUSED = "GAMEPAUSED";
    private const string GAMECHECKPOINT = "GAMECHECKPOINT";

    void Start()
    {
        pausedReason = "NONE";
        isInInneroutine = false;
        aboutToChange = false;
        demoPause = false;
        trigger_1 = false;
        toContinue = false;
        toQuit = false;
        bugsDeployed = nonBugsDeployed = 0;
        gameLevel = 0;
        difficulty = new DifficultyVariations();

        compilerAction.thisNoOfBugs = difficulty.NoOfBugs;
        compilerAction.thisBugTolerance = difficulty.BugTolerance;

        transform.GetChild(10).GetComponent<Slider>().maxValue = difficulty.NoOfBugs;
        transform.GetChild(10).GetComponent<Slider>().value = compilerAction.thisBugsCompiled + (difficulty.NoOfBugs - (difficulty.NoOfBugs * difficulty.BugTolerance));
    }

    void Update()
    {
        GameObject[] leftOverCode;
        if (!isInInneroutine && !demoPause)
        {
            StartCoroutine("clkCoroutine");
            if (bugsDeployed == difficulty.NoOfBugs) //INITIATES CHECKPOINT
            {
                leftOverCode = GameObject.FindGameObjectsWithTag("theBit");
                if(leftOverCode.Length == 0)
                {
                    demoPause = true;
                    pausedReason = "GAMECHECKPOINT";
                }
            }
        }
        else if(demoPause && !trigger_1)
        {
            trigger_1 = true;
            int thisGameLevel = gameLevel + 1;

            float codePoints;
            if (nonBugsDeployed > 0) codePoints = (compilerAction.thisnonBugsCompiled / nonBugsDeployed) * thisGameLevel * 2;
            else codePoints = 0;
            float bugPoints = compilerAction.percentError * thisGameLevel * 4;
            gamePoints = codePoints + bugPoints;

            switch (pausedReason)
            {
                case GAMEOVER:
                    Debug.Log("Game over");
                    leftOverCode = GameObject.FindGameObjectsWithTag("theBit");
                    if (leftOverCode.Length > 0) foreach (GameObject junk in leftOverCode) Destroy(junk);

                    pausedCard.GetChild(0).GetComponent<TMP_Text>().text = "Compilation " + thisGameLevel + ": Failed";
                    pausedCard.GetChild(0).GetComponent<TMP_Text>().color = Color.red;
                    //Continue means retry from level 1
                    pausedCard.GetChild(4).GetChild(0).GetComponent<TMP_Text>().text = "Retry";
                    //Quit means go to main menu.
                    break;
                case GAMEPAUSED:
                    Debug.Log("Game paused");
                    pausedCard.GetChild(0).GetComponent<TMP_Text>().text = "Compilation " + thisGameLevel + ": Paused.";
                    pausedCard.GetChild(0).GetComponent<TMP_Text>().color = Color.yellow;
                    //Continue means continue current level.
                    //Quit means go to main menu.
                    break;
                case GAMECHECKPOINT:
                    Debug.Log("Checkpoint reached");
                    pausedCard.GetChild(0).GetComponent<TMP_Text>().text = "Compilation " + thisGameLevel + ": Complete!";
                    pausedCard.GetChild(0).GetComponent<TMP_Text>().color = Color.green;
                    if(gameLevel >= 95) pausedCard.GetChild(3).GetComponent<TMP_Text>().text = "Output: " + difficulty.fairWell; //Was on 96.
                    totalPoints += gamePoints;
                    //Continue means continue to next level.
                    //Quit means go to main menu.
                    break;
            }

            pausedCard.GetChild(1).GetComponent<TMP_Text>().text = "Error: " + compilerAction.percentError * 100 + '%';
            pausedCard.GetChild(2).GetComponent<TMP_Text>().text = "Points: " + totalPoints;
            pausedCard.GetComponent<RectTransform>().localPosition = new Vector3(20, 0, 0);
        }

        if(demoPause && trigger_1 && toContinue)
        {
            switch (pausedReason)
            {
                case GAMEOVER:
                    //Difficulty has to be reset.
                    gameLevel = 0;
                    difficulty.resetDifficulty();
                    compilerAction.thisNoOfBugs = difficulty.NoOfBugs;
                    compilerAction.thisBugTolerance = difficulty.BugTolerance;
                    bugsDeployed = 0;
                    totalPoints = gamePoints = 0;
                    compilerAction.thisToContinue = true;
                    break;
                case GAMEPAUSED:
                    Debug.Log("Game resumed");
                    break;
                case GAMECHECKPOINT:
                    //Increase gameplay.
                    gameLevel++;
                    difficulty.adjustDifficulty(gameLevel);
                    compilerAction.thisNoOfBugs = difficulty.NoOfBugs;
                    compilerAction.thisBugTolerance = difficulty.BugTolerance;
                    bugsDeployed = 0;
                    compilerAction.thisToContinue = true;
                    break;
            }
            
            pausedReason = "NONE";
            demoPause = false;
            toQuit = toContinue = false;
            trigger_1 = false;

            isInInneroutine = false;
            StopCoroutine("clkCoroutine");
        }else if(demoPause && trigger_1 && toQuit) SceneManager.LoadScene("introScene");
    }

    IEnumerator clkCoroutine()
    {
        char codeContent = 'X';
        isInInneroutine = true;
        aboutToChange = true;
        yield return new WaitForSecondsRealtime(difficulty.Interval);

        if(bugsDeployed < difficulty.NoOfBugs) 
        { 
            Transform aCode = Instantiate(codeBlock, transform);
            GameObject innerCode = aCode.GetChild(0).gameObject;
            TMP_Text innerCodeText = innerCode.GetComponent<TMP_Text>();
            int shouldBeBug = Random.Range(0, 2);

            if (shouldBeBug != 1)
            {
                codeContent = (char)Random.Range(97, 122);
                nonBugsDeployed++;
            }
            else if (shouldBeBug == 1)
            {
                codeContent = (char)Random.Range(160, 255); //173 is blank
                aCode.GetComponent<SourcecodeBehaviour>().IsBug = true;
                innerCodeText.color = Color.red;
                bugsDeployed++;
            }

            Debug.Log((int) codeContent);
            Debug.Log(codeContent);
            if (bugsDeployed == difficulty.NoOfBugs) aCode.GetComponent<SourcecodeBehaviour>().IsLastByte = true; //INITIATES CHECKPOINT
            innerCodeText.text = codeContent.ToString();
            aCode.GetComponent<RectTransform>().localPosition = new Vector3(408, -1.1f, 0); //Return x to 408   -104
        }
        
        aboutToChange = false;
        yield return new WaitForSecondsRealtime(difficulty.Interval);

        isInInneroutine = false;
        StopCoroutine("clkCoroutine");
    }

    public void advance()
    {
        toContinue = true;
        pausedCard.GetComponent<RectTransform>().localPosition = new Vector3(20, 600, 0);
        pausedCard.GetChild(4).GetChild(0).GetComponent<TMP_Text>().text = "Continue";
    }

    public void Pause()
    {
        demoPause = true;
        pausedReason = GAMEPAUSED;
    }

    public void quitting()
    {
        toQuit = true;
        Debug.Log("Quitting");
    }
}
