﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameResultManager : MonoBehaviour
{

    private CardManager cardManager;
    private GameManager gameManager;

    private List<Card> correctCards;
    private List<Card> wrongCards;


    private void Start()
    {
        cardManager = GameObject.Find("GameManager").GetComponent<CardManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        correctCards = gameManager.GetCorrectCards();
        wrongCards = gameManager.GetWrongCards();

        GameObject.Find("PopUpPages").transform.Find("ResultPopUp/Numbers/CorrectNum/Txt").GetComponent<Text>().text = correctCards.Count.ToString();
        GameObject.Find("PopUpPages").transform.Find("ResultPopUp/Numbers/WrongNum/Txt").GetComponentInChildren<Text>().text = wrongCards.Count.ToString();
    }

    public void OnClickCardBtn()
    {
        GameObject.Find("PopUpPages").transform.Find("ResultPopUp").gameObject.SetActive(false);
        GameObject.Find("PopUpPages").transform.Find("CardListPopUp").gameObject.SetActive(true);
        cardManager.DestoryCards();

        if (EventSystem.current.currentSelectedGameObject.name == "CorrectBtn")
        {
            GameObject.Find("PopUpPages").transform.Find("CardListPopUp").GetComponentInChildren<Text>().text = "맞은 단어";
            cardManager.CreateNewCardItems(correctCards,"PopUpPages/CardListPopUp");
        }
        else
        {
            GameObject.Find("PopUpPages").transform.Find("CardListPopUp").GetComponentInChildren<Text>().text = "틀린 단어";
            cardManager.CreateNewCardItems(wrongCards, "PopUpPages/CardListPopUp");
        }
    }
}
