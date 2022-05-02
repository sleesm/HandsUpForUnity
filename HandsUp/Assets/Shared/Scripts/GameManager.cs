using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int gameVersion = -1;
    public int gameCategory = -1;
    public int timeLimit = 5;
    public int problemNum = 5;

    private int curIndex = 0;
    private List<Card> cards;
    private List<CustomCard> customCards;

    private int curScore = 0;
    private int curProblemNum = 0;
    private float curTime = 0;
    private bool isStart = false;

    private CardManager cardManager;
    private PlayerManager playerManager;

    private void Start()
    {
        cardManager = GameObject.Find("GameManager").GetComponent<CardManager>();
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
    }

    private void Update()
    {
        // TO-DO : Match the time format
        // Express the Current Time
        if (isStart)
        {
            curTime += Time.deltaTime;
            int min = ((int)curTime / 60 % 60);
            int seconds = ((int)curTime % 60);
            GameObject.Find("GamePage").transform.Find("LimitedTime").GetComponent<Text>().text = "제한 시간" + min + seconds;
        }
    }

    public void InitGame()
    {
        // Get cards form cardManager
        cardManager.GetBuitInCardsFromServer(gameCategory, true);
        cards = cardManager.GetBuitInCards();
        customCards = null;
        if (playerManager.GetUserId() >= 0)
        {
            cardManager.GetCustomCardsFromServer(gameCategory, playerManager.GetUserId());
            customCards = cardManager.GetCustomCards();
        }

        // Init
        curIndex = 0;
        curScore = 0;
        curProblemNum = problemNum;
        if (cards.Count + customCards.Count < problemNum)
            curProblemNum = cards.Count + customCards.Count;

        // To-Do : Include CustomCards in the algorithm
        // Get Random Card
        int[] randIndex = GetRandIndex(cards.Count);
        StartGame(cards[curIndex]);
    }

    private int[] GetRandIndex(int length)
    {
        int[] ranArr = Enumerable.Range(0, length).ToArray();
        for (int i = 0; i < length; ++i)
        {
            int ranIdx = Random.Range(i, length);
            int tmp = ranArr[ranIdx];
            ranArr[ranIdx] = ranArr[i];
            ranArr[i] = tmp;
        }
        return ranArr;
    }

    private void StartGame(Card cards)
    {
        StartCoroutine(cardManager.getImagesFromURL(cards.GetImagePath(), GameObject.Find("GamePage").transform.Find("Card/CardBGImg").gameObject));
        GameObject.Find("GamePage").transform.Find("Card/CardTxt").GetComponent<Text>().text = cards.GetName();

        if (gameVersion == 1)
        {
            GameObject.Find("GamePage").transform.Find("Card/CardBGImg").gameObject.SetActive(false);
            GameObject.Find("GamePage").transform.Find("Card/CardTxt").gameObject.SetActive(true);
            
            // Text Detection Function
        }
        else
        {
            GameObject.Find("GamePage").transform.Find("Card/CardBGImg").gameObject.SetActive(true);
            GameObject.Find("GamePage").transform.Find("Card/CardTxt").gameObject.SetActive(false);
            // Object Detection Function
        }

        // Time Function
        isStart = true;

        // Check Correct/Wrong
        bool isCorrect = true;

        CheckStatus(isCorrect);
    }

    private void CheckStatus(bool isCorrect)
    {
        // Check Score & Correct/Wrong Cards
        if (isCorrect)
        {
            curScore++;

            // Express the Current Score
            string tmp = curScore.ToString();
            if(curScore / 10 == 0)
                tmp = "0" + curScore.ToString();

            GameObject.Find("GamePage").transform.Find("CurProbNum").GetComponent<Text>().text = "문제 수 : " + tmp + "/" + problemNum;

            // Add Card to Correct Card list

        }
        else
        {
            // Add Card to Wrong Card list

        }

        if (curIndex < curProblemNum - 1)
            StartGame(cards[curIndex + 1]);
        else
        {
            // Connect to Result Page
        }
    }

    public int GetGameVersion()
    {
        return gameVersion;
    }

    public void SetGameVersion(int gameVersion)
    {
        this.gameVersion = gameVersion;
    }

    public int GetGameCategory()
    {
        return gameCategory;
    }

    public void SetGameCategory(int gameCategory)
    {
        this.gameCategory = gameCategory;
    }

    public int GetTimeLimit()
    {
        return timeLimit;
    }

    public void SetTimeLimit(int timeLimit)
    {
        this.timeLimit = timeLimit;
    }

    public int GetProblemNum()
    {
        return problemNum;
    }

    public void SetProblemNum(int problemNum)
    {
        this.problemNum = problemNum;
    }
}
