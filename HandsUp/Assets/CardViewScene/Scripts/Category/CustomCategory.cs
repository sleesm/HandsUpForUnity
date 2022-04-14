using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCategory : MonoBehaviour
{
    private int custom_card_id;
    private int category_id;
    private string name;
    private int count;
    private bool access;

    public int GetCustomCategoryId()
    {
        return custom_card_id;
    }

    public void SetCustomCategoryId(int id)
    {
        this.custom_card_id = id;
    }

    public int GetCategoryId()
    {
        return category_id;
    }

    public void SetCategoryId(int id)
    {
        this.category_id = id;
    }

    public string GetCategoryName()
    {
        return name;
    }

    public void SetCategoryName(string name)
    {
        this.name = name;
    }

    public int GetCount()
    {
        return count;
    }

    public void SetCount(int count)
    {
        this.count = count;
    }

    public bool GetAccess()
    {
        return access;
    }
    public void SetAccess(bool access)
    {
        this.access = access;
    }
}
