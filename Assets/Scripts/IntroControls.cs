using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroControls : MonoBehaviour
{
    public Transform introByte;
    public Transform introBullet;
    private char randomBug;
    private bool runningroutine, isTyping;

    private int nameDisplayed;
    private char[] title = { 'D', 'e', 'b', 'u', 'g', '.', 'S', 'h', 'o', 'o', 't', '(', ')' };
    private string stringBuffer;

    void Start()
    {
        randomBug = (char)Random.Range(200, 255);
        runningroutine = isTyping = false;
        nameDisplayed = 0;
    }

    void Update()
    {
        if (nameDisplayed < 13 && !isTyping) StartCoroutine("startTyping");
        else if (nameDisplayed == 13)
        {
            transform.GetChild(2).GetChild(0).GetComponent<TMP_Text>().text = "Debug.Shoot()";
            nameDisplayed++;
        }

        if (!runningroutine) StartCoroutine("introAnim");
    }

    IEnumerator startTyping()
    {
        isTyping = true;

        stringBuffer += title[nameDisplayed].ToString();
        string nextDisplay = stringBuffer + '|';
        transform.GetChild(2).GetChild(0).GetComponent<TMP_Text>().text = nextDisplay;
        nameDisplayed++;

        yield return new WaitForSeconds(0.125f);
        isTyping = false;
        StopCoroutine("startTyping");
    }

    IEnumerator introAnim()
    {
        runningroutine = true;

        //Place byte.
        Transform exampByte = Instantiate(introByte, transform);
        exampByte.GetChild(0).GetComponent<TMP_Text>().text = randomBug.ToString(); 
        exampByte.GetChild(0).GetComponent<TMP_Text>().color = Color.red;
        exampByte.GetComponent<RectTransform>().localPosition = new Vector3(-232, -1.1f, 0);

        yield return new WaitForSeconds(2);

        //Fire bullet
        Transform aBullet = Instantiate(introBullet, transform);
        aBullet.GetComponent<Rigidbody2D>().gravityScale = -15f;

        yield return new WaitForSeconds(4);
        runningroutine = false;
        StopCoroutine("introAnim");
    }

    public void StartPlaying()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("FirstScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
