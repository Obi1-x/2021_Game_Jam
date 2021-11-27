using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class compilerAction : MonoBehaviour
{
    public Transform[] byteCode;
    private GameObject compilerLabel;
    public Slider errorLevel;

    public static int thisBugsCompiled;
    public static int thisnonBugsCompiled;
    public static int thisNoOfBugs;
    public static bool thisToContinue;
    public static float thisBugTolerance;
    public static float percentError;

    void Start()
    {
        compilerLabel = transform.GetChild(0).gameObject;
        thisToContinue = false;
        thisBugsCompiled = 0;
        percentError = 0f;
    }

    void Update()
    {
        if(!compilerLabel.GetComponent<Animation>().isPlaying) compilerLabel.GetComponent<TMP_Text>().text = "Compiler";
        
        if (thisToContinue) // && Operation.pausedReason != "GAMEPAUSED"
        {
            thisBugsCompiled = 0;
            percentError = 0f;
            thisnonBugsCompiled = 0;
            transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = "0%";

            errorLevel.maxValue = thisNoOfBugs;  //errorLevel.maxValue = thisBugTolerance * thisNoOfBugs; errorLevel.value = thisBugsCompiled;
            errorLevel.value = thisBugsCompiled + (thisNoOfBugs - (thisNoOfBugs * thisBugTolerance)); 
            thisToContinue = false;
        } 
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "theBit")
        {
            compilerLabel.GetComponent<TMP_Text>().text = "Compiling...";
            Animation labelAnimator = compilerLabel.GetComponent<Animation>();
            if(!labelAnimator.isPlaying) labelAnimator.Play();

            Color codeColor = new Color(0, 0, 0, 0);
            bool aBug = col.gameObject.GetComponent<SourcecodeBehaviour>().IsBug;

            if (aBug)
            {
                codeColor = Color.red;
                thisBugsCompiled++;
                percentError = (float) thisBugsCompiled / thisNoOfBugs;
                transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = (percentError * 100).ToString() + '%';
                errorLevel.maxValue = thisNoOfBugs;
                errorLevel.value = thisBugsCompiled + (thisNoOfBugs - (thisNoOfBugs * thisBugTolerance));
            }
            else if (!aBug)
            {
                codeColor = Color.white;
                thisnonBugsCompiled++;
            }

            Destroy(col.gameObject);
            foreach(Transform parSys in byteCode)
            {
                ParticleSystem resultingBytecode = parSys.gameObject.GetComponent<ParticleSystem>();
                var innerCol = resultingBytecode.colorOverLifetime;
                innerCol.color = codeColor;
                resultingBytecode.Play();
            }

            if (col.gameObject.GetComponent<SourcecodeBehaviour>().IsLastByte)
            {
                Operation.demoPause = true;
                Operation.pausedReason = "GAMECHECKPOINT";
            }
            else if (percentError >= thisBugTolerance)
            {
                //Operation.gameOver = true;
                Operation.pausedReason = "GAMEOVER";
                Operation.demoPause = true;
                transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = "0%";
            }
        }
    }
}
