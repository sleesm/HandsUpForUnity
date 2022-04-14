using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private int cardId;
    private int categoryId;
    private string cardName;
    private string imgPath;

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

    }
}