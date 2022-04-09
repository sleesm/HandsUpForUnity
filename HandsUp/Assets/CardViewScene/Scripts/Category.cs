using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Category : MonoBehaviour
{
    private int categoryId;
    private string categoryName;

    public int GetId()
    {
        return categoryId;
    }

    public void SetId(int id)
    {
        this.categoryId = id;
    }

    public string GetName()
    {
        return categoryName;
    }
    public void SetName(string name)
    {
        this.categoryName = name;
    }
}
