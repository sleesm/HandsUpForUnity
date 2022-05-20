using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EditManager : MonoBehaviour
{
    private PlayerManager playerManager;
    private CategoryManager categoryManager;
    private CardManager cardManager;

    private Category category;

    public InputField categoryName;

    private bool access;


    void Start()
    {
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        categoryManager = this.gameObject.GetComponent<CategoryManager>();
        cardManager = this.gameObject.GetComponent<CardManager>();
    }

    public void OnClickEditBtn()
    {
        GameObject.Find("Canvas").transform.Find("CardViewPage/CardsScrollView").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("EditCategoryPage").gameObject.SetActive(true);
        cardManager.DestoryCards();
        cardManager.InitCards(category.GetCategoryId(), "EditCategoryPage");

        //설정 값 불러오기
        GameObject.Find("Canvas").transform.Find("EditCategoryPage/CategoryName").GetComponent<InputField>().text = category.GetName();
        GameObject.Find("Canvas").transform.Find("EditCategoryPage/Toggles/Public").GetComponent<Toggle>().isOn = category.GetAccess();
        
        // 빌트인 카테고리는 수정 불가능
        bool isInteractable = true;
        if(category.GetCategoryIsBuiltIn())
            isInteractable = false;
        GameObject.Find("Canvas").transform.Find("EditCategoryPage/CategoryName").GetComponent<InputField>().interactable = isInteractable;
        GameObject.Find("Canvas").transform.Find("EditCategoryPage/Toggles").gameObject.SetActive(isInteractable);
        
    }

    public void InitEditCategories(Category cate)
    {
        category = cate;
    }

    public void ToggleClick()
    {
        if (GameObject.Find("Canvas").transform.Find("EditCategoryPage/Toggles/Public").GetComponent<Toggle>().isOn)
        {
            access = true;
        }
        else
        {
            access = false;
        }
    }

}
