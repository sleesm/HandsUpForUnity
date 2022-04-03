using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        InitCategories();
    }

    private void InitCategories()
    {
        List<Category> categories = categoryManager.GetBuitInCategories();
        List<CustomCategory> customCategories = null;
        if (playerManager.GetUserId() >= 0)
            customCategories = categoryManager.GetCustomCategories(playerManager.GetUserId());

        //To-Do : 받아온 리스트를 바탕으로 instatiate 하기 (Content) 아래로
    }



}
