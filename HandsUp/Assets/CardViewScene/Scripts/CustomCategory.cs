using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCategory : MonoBehaviour
{
    private int customCategoryId;
    private int categoryId;
    private string categoryName;
    private int count;
    private bool access;

    public int GetCustomCategoryId()
    {
        return customCategoryId;
    }

    public void SetCustomCategoryId(int id)
    {
        this.customCategoryId = id;
    }

    public int GetCategoryId()
    {
        return categoryId;
    }

    public void SetCategoryId(int id)
    {
        this.categoryId = id;
    }

    public string GetCategoryName()
    {
        return categoryName;
    }

    public void SetCategoryName(string name)
    {
        this.categoryName = name;
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
