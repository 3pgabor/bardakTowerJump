using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHighscores : MonoBehaviour 
{
    public TMPro.TextMeshProUGUI[] rNames;
    public TMPro.TextMeshProUGUI[] rScores;

    public TMPro.TextMeshProUGUI[] hardcorerNames;
    public TMPro.TextMeshProUGUI[] hardcorerScores;
    public GameObject hardcorePanel;
    HighScores myScores;

    void Start() //Fetches the Data at the beginning
    {
        for (int i = 0; i < rNames.Length;i ++)
        {
            rNames[i].text = i + 1 + ". Fetching...";
        }
        myScores = GetComponent<HighScores>();
        StartCoroutine("RefreshHighscores");
    }
    public void SetScoresToMenu(PlayerScore[] highscoreList) //Assigns proper name and score for each text value
    {
        for (int i = 0; i < rNames.Length;i ++)
        {
            rNames[i].text = i + 1 + ". ";
            if (highscoreList.Length > i)
            {
                int scoreAbs = highscoreList[i].score;
                //int scoreAbs = highscoreList[i].score;
                rScores[i].text = scoreAbs.ToString();
                rNames[i].text = highscoreList[i].username;
            }
        }
    }

    public void SetHardcoreScoresToMenu(PlayerScore[] highscoreList2) //Assigns proper name and score for each text value
    {
        for (int i = 0; i < rNames.Length; i++)
        {
            hardcorerNames[i].text = i + 1 + ". ";
            if (highscoreList2.Length > i)
            {
                int scoreAbs = highscoreList2[i].score;
                //int scoreAbs = highscoreList[i].score;
                hardcorerScores[i].text = scoreAbs.ToString();
                hardcorerNames[i].text = highscoreList2[i].username;
            }
        }
    }

    IEnumerator RefreshHighscores() //Refreshes the scores every 30 seconds
    {
        while(true)
        {
            myScores.DownloadScores();
            yield return new WaitForSeconds(60);
        }
    }

    public void hardCoreTapped()
    {
        hardcorePanel.SetActive(true);
    }

    public void normalTapped()
    {
        hardcorePanel.SetActive(false);
    }
}
