using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Category : MonoBehaviour
{
    //private int custom_card_id;
    public int category_id;
    private string name;
    private bool access;

    private int userId;
    private bool is_built_in;

    /*
    public int GetCustomCategoryId()
    {
        return custom_card_id;
    }

    public void SetCustomCategoryId(int id)
    {
        this.custom_card_id = id;
    }
    */

    public int GetCategoryId()
    {
        return category_id;
    }

    public void SetCategoryId(int id)
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

    public bool GetAccess()
    {
        return access;
    }
    public void SetAccess(bool access)
    {
        this.access = access;
    }

    public int GetUserId()
    {
        return userId;
    }
    public void SetUserId(int id)
    {
        this.userId = id;
    }

    public bool GetCategoryIsBuiltIn()
    {
        return is_built_in;
    }
    public void SetCategoryIsBuiltIn(bool is_built_in)
    {
        this.is_built_in = is_built_in;
    }

    public void OnClickCategory()
    {
        if (SceneManager.GetActiveScene().name.Equals("CardViewScene"))
        {
            if (this.transform.parent.parent.parent.parent.name.Equals("OthersCategoryPage"))
            {
                GameObject.Find("Canvas").transform.Find("OthersCategoryPage/PR_CategoriesScroll").gameObject.SetActive(false);
                GameObject.Find("Canvas").transform.Find("OthersCategoryPage/CardsScrollView").gameObject.SetActive(true);
                GameObject.Find("Canvas").transform.Find("OthersCategoryPage").GetComponentInChildren<Text>().text = "다른 사용자의 카드";
                GameObject.Find("CardViewManager").GetComponent<CardManager>().InitCards(this.category_id, "OthersCategoryPage", false, "category/card/custom/public");
            }
            else
            {
                GameObject.Find("Canvas").transform.Find("CardViewPage/PR_CategoriesScroll").gameObject.SetActive(false);

                if(EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text == "+")
                {
                    GameObject.Find("Canvas").transform.Find("CardViewPage").gameObject.SetActive(false);
                    GameObject.Find("Canvas").transform.Find("ItemAddPage").gameObject.SetActive(true);
                }
                else
                {
                    GameObject.Find("Canvas").transform.Find("CardViewPage/CardsScrollView").gameObject.SetActive(true);
                    if(GameObject.Find("PlayerManager").GetComponent<PlayerManager>().GetUserId() > 0)
                        GameObject.Find("Canvas").transform.Find("CardViewPage/EditBtn").gameObject.SetActive(true);
                    GameObject.Find("CardViewManager").GetComponent<CardManager>().InitCards(this.category_id, "CardViewPage");
                    GameObject.Find("CardViewManager").GetComponent<EditManager>().InitEditCategories(this);
                }
            }

        }
        else if (SceneManager.GetActiveScene().name.Equals("GameSelectScene"))
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().SetGameCategory(this.category_id);
            GameObject.Find("Canvas").GetComponent<GameSelectManager>().ShowSettingPopUp();
        }
    }
}
