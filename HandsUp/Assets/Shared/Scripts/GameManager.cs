using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int gameVersion = -1;
    public int gameCategory = -1;
    public int timeLimit = 5;
    public int problemNum = 5;
    public string answer = "";
    public static bool isResultCorrect = true;

    private int curIndex = 0;
    private List<Card> cards;
    public static bool isCardLoaded = false;
    public static bool isCustomCardLoaded = false;
    public static bool isImgLoaded = false;
    public static bool isResultGot = false;
    public static bool isGameEnd = false;
    public static bool isGameStopped = false;
    public static bool isNextGameReady = false;

    public int curProgress = 1;
    private int curProblemNum = 0;
    private float curTime = 0;
    private float diffTime = 0;
    public float showTime = 0;
    private bool isStart = false;

    private Card nowCard;

    private List<Card> correctCards;
    private List<Card> wrongCards;

    private CardManager cardManager;
    private PlayerManager playerManager;
    private CameraManager cameraManager;

    private void Start()
    {
        cardManager = GameObject.Find("GameManager").GetComponent<CardManager>();
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        
        correctCards = new List<Card>();
        wrongCards = new List<Card>();
    }

    private void Update()
    {
        if (isStart && !isGameEnd)
        {
            TimeSetting();
        }
    }

    // Express the Current Time
    private void TimeSetting()
    {
        if(!isGameStopped)
        {
            curTime += Time.deltaTime;
            diffTime = timeLimit * 60 - curTime;
        }
        else
        {
            showTime += Time.deltaTime;
            if(showTime > 2)
            {
                // Show Card Info and Next Btn
                GameObject.Find("PopUpPages").transform.Find("CardInfoPopUp").gameObject.SetActive(true);
                StartCoroutine(cardManager.getImagesFromURL(nowCard.GetImagePath(), GameObject.Find("PopUpPages").transform.Find("CardInfoPopUp/PR_CardItem/CardImg").gameObject, false));
                GameObject.Find("PopUpPages").transform.Find("CardInfoPopUp/PR_CardItem/CardName").GetComponent<Text>().text = nowCard.GetName();
                GameObject.Find("PopUpPages").transform.Find("CardInfoPopUp/PR_CardItem/EditImg").gameObject.SetActive(false);
            }
        }
        int min = ((int)diffTime / 60 % 60);
        int seconds = ((int)diffTime % 60);
        string minString = min.ToString();
        string secondsString = seconds.ToString();
        if (min / 10 == 0)
            minString = "0" + min.ToString();
        if (seconds / 10 == 0)
            secondsString = "0" + seconds.ToString();
        GameObject.Find("Canvas").transform.Find("GamePage/LimitedTime").GetComponent<Text>().text = "제한 시간 " + minString + " : " + secondsString;

        if(diffTime >= 0.0f && diffTime < 1.0f)
        {
            isGameEnd = true;
            cameraManager.CameraOff();
            CheckStatus(nowCard, false);
        }
    }

    public void GetCards()
    {
        // Get cards form cardManager
        cardManager.InitCards(gameCategory, "", true);
        StartCoroutine((WaitForLoading()));
    }

    IEnumerator WaitForLoading()
    {
        while (true)
        {
            if (isCardLoaded && isCustomCardLoaded)
            {
                InitGame();
                break;
            }else if (isImgLoaded)
            {
                StartGame(cards[curIndex]);
                break;
            }
            else if (isResultGot)
            {
                isResultGot = false;
                CheckStatus(nowCard, isResultCorrect);
                break;
            }
            else if (isNextGameReady)
            {
                CheckNextScene();
            }
            else
                yield return new WaitForSeconds(Time.deltaTime);
        }
        yield return null;
    }

    private void InitGame()
    {
        Debug.Log("InitGame");

        isCardLoaded = false;
        isCustomCardLoaded = false;
        isImgLoaded = false;
        isResultGot = false;
        isGameEnd = false;
        isGameStopped = false;

        cards = cardManager.GetCards();

        // Init
        curIndex = 0;
        curProblemNum = problemNum;
        if (cards.Count < problemNum)
            curProblemNum = cards.Count;
        correctCards = new List<Card>();
        wrongCards = new List<Card>();
        curTime = 0;
        curProgress = 1;

        // Get Random Card
        int[] randIndex = GetRandIndex(cards.Count);
        InitCard(cards[curIndex]);
        StartCoroutine(WaitForLoading());
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

    private void InitCard(Card card)
    {
        GameObject.Find("GamePage").transform.Find("Card/CardBGImg").gameObject.SetActive(false);
        GameObject.Find("GamePage").transform.Find("Card/CardTxt").gameObject.SetActive(false);
        StartCoroutine(cardManager.getImagesFromURL(card.GetImagePath(), GameObject.Find("GamePage").transform.Find("Card/CardBGImg").gameObject, true));
        GameObject.Find("GamePage").transform.Find("Card/CardTxt").GetComponent<Text>().text = card.GetName();
    }

    private void StartGame(Card card)
    {
        // Init Loded values
        isCardLoaded = false;
        isCustomCardLoaded = false;
        isImgLoaded = false;
        isResultGot = false;

        nowCard = card;
        string answer = card.GetName();
        SetAnswer(answer);
        cameraManager = GameObject.Find("CameraManager").GetComponent<CameraManager>();

        GameObject.Find("GamePage").transform.Find("CurProbNum").GetComponent<Text>().text = "문제 수 : " + curProgress + "/" + curProblemNum;
        
        // Game
        if (gameVersion == 1)
        {
            GameObject.Find("GamePage").transform.Find("Card/CardBGImg").gameObject.SetActive(true);
            GameObject.Find("GamePage").transform.Find("Card/CardTxt").gameObject.SetActive(false);
        }
        else
        {
            GameObject.Find("GamePage").transform.Find("Card/CardBGImg").gameObject.SetActive(false);
            GameObject.Find("GamePage").transform.Find("Card/CardTxt").gameObject.SetActive(true);
        }

        // Text or Object Detection
        cameraManager.CameraOn();
        StartCoroutine(WaitForLoading());

        // Time Function
        isStart = true;

    }

    private void CheckStatus(Card card, bool isCorrect)
    {
        curProgress++;
        
        if(isGameEnd)
        {
            int temp = curProblemNum - curProgress + 1;
            for (int i = 0; i <= temp; i++)
            {
                wrongCards.Add(cards[curIndex++]);
            }
        }
        else
        {
            // Check Score & Correct/Wrong Cards
            if (isCorrect)
            {
                // Show O
                GameObject.Find("PopUpPages").transform.Find("OXPopUp").gameObject.SetActive(true);
                GameObject.Find("PopUpPages").transform.Find("OXPopUp/CorrectExpression").gameObject.SetActive(true);
                GameObject.Find("PopUpPages").transform.Find("OXPopUp/WrongExpression").gameObject.SetActive(false);
                isGameStopped = true;
                // Add Card to Correct Card list
                correctCards.Add(card);
            }
            else
            {
                // Show X
                GameObject.Find("PopUpPages").transform.Find("OXPopUp").gameObject.SetActive(true);
                GameObject.Find("PopUpPages").transform.Find("OXPopUp/CorrectExpression").gameObject.SetActive(false);
                GameObject.Find("PopUpPages").transform.Find("OXPopUp/WrongExpression").gameObject.SetActive(true);
                isGameStopped = true;
                // Add Card to Wrong Card list
                wrongCards.Add(card);
            }
        }
        StartCoroutine(WaitForLoading());
    }

    public void CheckNextScene()
    {
        isNextGameReady = false;
        isGameStopped = false;
        if (curIndex < curProblemNum - 1)
        {
            curIndex += 1;
            InitCard(cards[curIndex]);
            StartCoroutine(WaitForLoading());
        }
        else
        {
            isStart = false;
            isGameEnd = true;
            cameraManager.CameraOff();
            // Connect to Result Page
            GameObject.Find("GamePage").SetActive(false);
            GameObject.Find("PopUpPages").transform.Find("ResultPopUp").gameObject.SetActive(true);
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

    public void SetAnswer(string answer)
    {
        this.answer = answer;
    }

    public string GetAnswer()
    {
        return answer;
    }

    public List<Card> GetCorrectCards()
    {
        return correctCards;
    }

    public List<Card> GetWrongCards()
    {
        return wrongCards;
    }
}
