using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (SceneManager.GetActiveScene().name.Equals("CardViewScene"))
        {
            GameObject.Find("CardViewManager").GetComponent<CardManager>().InitCards(this.category_id);
        }
        else if (SceneManager.GetActiveScene().name.Equals("GameSelectScene"))
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().SetGameCategory(this.category_id);
            GameObject.Find("GameManager").GetComponent<GameSelectManager>().ShowSettingPopUp();
        }
    }
}
