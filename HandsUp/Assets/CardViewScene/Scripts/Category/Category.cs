using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Category : MonoBehaviour
{
    public int category_id;
    private string name;

    public int GetId()
    {
        return category_id;
    }

    public void SetId(int id)
    {
        this.category_id = id;
    }

    public string GetName()
    {
        return name;
    }
    public void SetName(string name)
    {
        this.name = name;
    }

    public void OnClickCategory()
    {
        GameObject.Find("CardViewManager").GetComponent<CardManager>().InitCards(this.category_id);
    }
}
