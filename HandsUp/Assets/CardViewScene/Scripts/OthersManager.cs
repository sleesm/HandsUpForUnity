using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OthersManager : MonoBehaviour
{
    private CardManager cardManager;
    private CategoryManager categoryManager;

    private void Awake()
    {
        cardManager = GameObject.Find("CardViewManager").gameObject.GetComponent<CardManager>();
        categoryManager = GameObject.Find("CardViewManager").gameObject.GetComponent<CategoryManager>();
    }

    public void InitOthersCategory()
    {
        categoryManager.InitCategories(false ,true, "category/custom/public");
    }
}
