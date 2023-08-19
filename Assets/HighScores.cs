using System.Collections;
using System.Collections.Generic;
using LootLocker.Requests;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEngine.Networking;

public class HighScores : MonoBehaviour
{
    

    public PlayerScore[] scoreList;
    public PlayerScore[] hardcoreScoreList;

    public Text[] Entries;
    DisplayHighscores myDisplay;
    public string playerIdentifier = "unique_player_identifier_here";

    //android
    //public static int boardID = 6788;

    public static int boardID = 8320;

    public static int hardcoreBoardID = 8321;

    static HighScores instance; //Required for STATIC usability

    void Awake()
    {
        instance = this; //Sets Static Instance
        myDisplay = GetComponent<DisplayHighscores>();
        playerIdentifier = (PlayerPrefs.GetString("unique_player_identifier", "0"));
        if (playerIdentifier.Equals("0"))
        {
            playerIdentifier = Guid.NewGuid().ToString();
            PlayerPrefs.SetString("unique_player_identifier", playerIdentifier);
        }

        LootLockerSDKManager.StartSession(playerIdentifier, (response) =>
        {
            if (response.success)
            {
                Debug.Log("session with LootLocker started");
            }
            else
            {
                Debug.Log("failed to start sessions" + response.Error);
            }
        });
        //UploadScore("cant play past level", 916, true);
    }
    
    public static void UploadScore(string username, int score, bool isHardcore)  //CALLED when Uploading new Score to WEBSITE
    {//STATIC to call from other scripts easily
        int validBoardId = boardID;
        if(isHardcore)
        {
            validBoardId = hardcoreBoardID;
        }
       LootLockerSDKManager.SubmitScore(username, score, validBoardId, (response) =>
       {
           if (response.success)
           {
               Debug.Log("upload score success");
           }
           else
           {
               Debug.Log("upload score failed" + response.Error);
           }

       });
    }

    public void DownloadScores()
    {
        //OrganizeInfo(loaded.downloadHandler.text);
        LootLockerSDKManager.GetScoreList(boardID, 10, (response) =>
        {
            if (response.success)
            {
                Debug.Log("get score success");


                LootLockerLeaderboardMember[] scores = response.items;

                scoreList = new PlayerScore[scores.Length];

                for (int i = 0; i < scores.Length; i++) //For each entry in the string array
                {
                    string username = scores[i].member_id;
                    int score = scores[i].score;
                    scoreList[i] = new PlayerScore(username, score);
                    print("nane"+scoreList[i].username + ": " + scoreList[i].score);
                }
                myDisplay.SetScoresToMenu(scoreList);
            }
            else
            {
                Debug.Log("get score failed" + response.Error);
            }
        });


        //OrganizeInfo(loaded.downloadHandler.text);
        LootLockerSDKManager.GetScoreList(hardcoreBoardID, 10, (response) =>
        {
            if (response.success)
            {
                Debug.Log("get score success");


                LootLockerLeaderboardMember[] scores = response.items;

                hardcoreScoreList = new PlayerScore[scores.Length];

                for (int i = 0; i < scores.Length; i++) //For each entry in the string array
                {
                    string username = scores[i].member_id;
                    int score = scores[i].score;
                    hardcoreScoreList[i] = new PlayerScore(username, score);
                    //print(hardcoreScoreList[i].username + ": " + hardcoreScoreList[i].score);
                }
                myDisplay.SetHardcoreScoresToMenu(hardcoreScoreList);
            }
            else
            {
                Debug.Log("get score failed" + response.Error);
            }
        });



    }
}

public struct PlayerScore //Creates place to store the variables for the name and score of each player
{
    public string username;
    public int score;

    public PlayerScore(string _username, int _score)
    {
        username = _username;
        score = _score;
    }
}