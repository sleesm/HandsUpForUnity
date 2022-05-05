using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

}
