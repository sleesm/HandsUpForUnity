using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCard : MonoBehaviour
{
    private int customCardId;
    private int userId;
    private int cardId;
    private int categoryId;
    private string name;
    private string imgPath;


    public int GetCustomCardId()
    {
        return customCardId;
    }
    public void SetCustomCardIdd(int id)
    {
        this.customCardId = id;
    }


    public int GetUserId()
    {
        return userId;
    }
    public void SetUserId(int id)
    {
        this.userId = id;
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
        return name;
    }
    public void SetName(string name)
    {
        this.name = name;
    }

    public string GetImagePath()
    {
        return imgPath;
    }
    public void SetImagePath(string name)
    {
        this.name = imgPath;
    }
}