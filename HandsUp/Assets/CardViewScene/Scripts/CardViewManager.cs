using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardViewManager : MonoBehaviour
{
    private PlayerManager playerManager;
    private CategoryManager categoryManager;
    private CardManager cardManager;

    public GameObject categoryItem;

    private void Start()
    {
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        categoryManager = this.gameObject.GetComponent<CategoryManager>();
        cardManager = this.gameObject.GetComponent<CardManager>();

        InitCategories();
    }

    private void InitCategories()
    {
        List<Category> categories = categoryManager.GetBuitInCategoriesFromServer();
        CreateNewCategoryItems(categories);

        if (playerManager.GetUserId() >= 0)
        {
            List<CustomCategory> customCategories = categoryManager.GetCustomCategoriesFromServer(playerManager.GetUserId());
            CreateNewCustomCategoryItems(customCategories);
        }

    }

    private void CreateNewCategoryItems(List<Category> categories)
    {
        for (int i = 0; i< categories.Count; i++)
        {
            GameObject newCategoryItem = Instantiate(categoryItem, new Vector3(0, 0, 0), Quaternion.identity);
            newCategoryItem.transform.SetParent(GameObject.Find("Content").transform);
            newCategoryItem.transform.localScale = new Vector3(1, 1, 1);
            newCategoryItem.GetComponent<Category>().SetId(categories[i].GetId());
            newCategoryItem.GetComponent<Category>().SetName(categories[i].GetName());

            newCategoryItem.GetComponentInChildren<Text>().text = categories[i].GetName();
        }
    }

    private void CreateNewCustomCategoryItems(List<CustomCategory> customCategories)
    {
        for (int i = 0; i < customCategories.Count; i++)
        {
            GameObject newCategoryItem = Instantiate(categoryItem, new Vector3(0, 0, 0), Quaternion.identity);
            newCategoryItem.transform.SetParent(GameObject.Find("Content").transform);
            newCategoryItem.transform.localScale = new Vector3(1, 1, 1);
            newCategoryItem.GetComponent<CustomCategory>().SetCustomCategoryId(customCategories[i].GetCustomCategoryId());
            newCategoryItem.GetComponent<CustomCategory>().SetCategoryName(customCategories[i].GetCategoryName());

            newCategoryItem.GetComponentInChildren<Text>().text = customCategories[i].GetCategoryName();
        }
    }

}
