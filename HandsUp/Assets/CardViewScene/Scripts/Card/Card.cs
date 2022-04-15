using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    AndroidTTSManager tts;

    private int cardId;
    private int categoryId;
    private string cardName;
    private string imgPath;

    private void Start()
    {
        tts = GameObject.Find("TTSManager").GetComponent<AndroidTTSManager>();
    }

    public int GetCardId()
    {
        return cardId;
    }
    public void SetCardId(int id)
    {
        this.cardId = id;
    }

    public int GetCategoryId()
    {
        return categoryId;
    }
    public void SetCategoryId(int id)
    {
        this.categoryId = id;
    }

    public string GetName()
    {
        return cardName;
    }
    public void SetName(string cardName)
    {
        this.cardName = cardName;
    }

    public string GetImagePath()
    {
        return imgPath;
    }
    public void SetImagePath(string imgPath)
    {
        this.imgPath = imgPath;
    }

    public void OnClickSoundBtn()
    {
        tts.StartTTS(this.gameObject.GetComponentInChildren<Text>().text);
    }
}