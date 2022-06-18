using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CardViewManager : MonoBehaviour
{
    private PlayerManager playerManager;
    private CategoryManager categoryManager;
    private CardManager cardManager;
    

    private void Start()
    {
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        categoryManager = this.gameObject.GetComponent<CategoryManager>();
        cardManager = this.gameObject.GetComponent<CardManager>();

        categoryManager.InitCategories();
    }


    public void OnClickAddBtn()
    {
        if (EventSystem.current.currentSelectedGameObject.name.Equals("DirectAddBtn"))
        {
            GameObject.Find("Canvas").transform.Find("ItemAddPage").gameObject.SetActive(false);
            GameObject.Find("Canvas").transform.Find("CustomPage").gameObject.SetActive(true);
            GameObject.Find("Canvas").transform.Find("CustomPage").GetComponent<CustomManager>().InitPages();
            GameObject.Find("Canvas").transform.Find("CustomPage").GetComponent<CustomManager>().InitDropdownOptions();
        }
        else
        {
            GameObject.Find("Canvas").transform.Find("ItemAddPage").gameObject.SetActive(false);
            GameObject.Find("Canvas").transform.Find("OthersCategoryPage").gameObject.SetActive(true);
            GameObject.Find("Canvas").transform.Find("OthersCategoryPage/PR_CategoriesScroll").gameObject.SetActive(true);
            GameObject.Find("Canvas").transform.Find("OthersCategoryPage/CardsScrollView").gameObject.SetActive(false);
            GameObject.Find("Canvas").transform.Find("OthersCategoryPage").gameObject.GetComponent<OthersManager>().InitOthersCategory();
        }
    }
}
