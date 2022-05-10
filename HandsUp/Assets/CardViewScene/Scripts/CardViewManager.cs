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
        }
    }
}
