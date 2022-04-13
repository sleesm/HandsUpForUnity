using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;

public class CategoryManager : MonoBehaviour
{
    private PlayerManager playerManager;
    public GameObject categoryItem;

    private List<Category> categories;
    private List<CustomCategory> customCategories;


    private void Awake()
    {
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        categories = new List<Category>();
        customCategories = new List<CustomCategory>();
    }


    public void InitCategories()
    {
        GameObject.Find("Canvas").transform.Find("CategoriesScrollView").gameObject.SetActive(true);
        GameObject.Find("Canvas").transform.Find("CardsScrollView").gameObject.SetActive(false);

        if (categories.Count <= 0)
        {
            GetBuitInCategoriesFromServer();
        }

        if (customCategories.Count > 0)
        {
            GameObject[] content = GameObject.FindGameObjectsWithTag("customCategoryItem");
            for (int i = 0; i < content.Length; i++)
            {
                Destroy(content[i]);
            }
            customCategories.Clear();
        }

        if (playerManager.GetUserId() >= 0)
        {
            GetCustomCategoriesFromServer(playerManager.GetUserId());
        }
    }


    private void GetBuitInCategoriesFromServer()
    {
            StartCoroutine(DataManager.getDataFromServer("category", (raw) =>
           {
               Debug.Log(raw);
               JObject applyJObj = JObject.Parse(raw);
               if (applyJObj["result"].ToString().Equals("success"))
               {
                   foreach (JObject tmpCategory in applyJObj["categories"])
                   {
                       Category tmp = new Category();
                       tmp.SetId((int)tmpCategory["category_id"]);
                       tmp.SetName(tmpCategory["category_name"].ToString());

                       categories.Add(tmp);
                   }
                   CreateNewCategoryItems(categories);
               }
               else
               {
                   Debug.Log("results : fail");
               }

           }));
    }


    private void CreateNewCategoryItems(List<Category> categories)
    {
        for (int i = 0; i < categories.Count; i++)
        {
            GameObject newCategoryItem = Instantiate(categoryItem, new Vector3(0, 0, 0), Quaternion.identity);
            newCategoryItem.transform.SetParent(GameObject.Find("Canvas").transform.Find("CategoriesScrollView/Viewport/Content").transform);
            Debug.Log(newCategoryItem.transform.parent.name);
            newCategoryItem.transform.localScale = new Vector3(1, 1, 1);
            newCategoryItem.GetComponent<Category>().SetId(categories[i].GetId());
            newCategoryItem.GetComponent<Category>().SetName(categories[i].GetName());

            newCategoryItem.GetComponentInChildren<Text>().text = categories[i].GetName();
        }
    }


    public List<Category> GetBuitInCategories()
    {
        return categories;
    }

    public Category GetBuiltInCategoryInfo(int id)
    {
        foreach(Category tmp in categories)
        {
            if (tmp.GetId().Equals(id))
                return tmp;
        }

        return null;
    }


    private void GetCustomCategoriesFromServer(int userId)
    {
        UserData userData = new UserData();
        userData.user_id = userId;

        var req = JsonConvert.SerializeObject(userData);

        customCategories = new List<CustomCategory>();

        StartCoroutine(DataManager.sendDataToServer("category/custom", req, (raw) =>
        {
            Debug.Log(raw);
            JObject applyJObj = JObject.Parse(raw);
            if (applyJObj["results"].Equals("success"))
            {
                foreach (JObject tmpCategory in applyJObj["categories"])
                {
                    CustomCategory tmp = new CustomCategory();
                    tmp.SetCustomCategoryId((int)tmpCategory["category_custom_id"]);
                    tmp.SetCategoryId((int)tmpCategory["category_id"]);
                    tmp.SetCategoryName(tmpCategory["category_name"].ToString());
                    tmp.SetCount((int)tmpCategory["category_shared_count"]);
                    tmp.SetAccess((bool)tmpCategory["category_access"]);

                    customCategories.Add(tmp);
                }

                CreateNewCustomCategoryItems(customCategories);
            }
            else
            {
                Debug.Log("results : fail");
            }

        }));
    }


    private void CreateNewCustomCategoryItems(List<CustomCategory> customCategories)
    {
        for (int i = 0; i < customCategories.Count; i++)
        {
            GameObject newCategoryItem = Instantiate(categoryItem, new Vector3(0, 0, 0), Quaternion.identity);
            newCategoryItem.transform.SetParent(GameObject.Find("Content").transform);
            newCategoryItem.transform.localScale = new Vector3(1, 1, 1);
            newCategoryItem.tag = "customCategoryItem";
            newCategoryItem.GetComponent<CustomCategory>().SetCustomCategoryId(customCategories[i].GetCustomCategoryId());
            newCategoryItem.GetComponent<CustomCategory>().SetCategoryName(customCategories[i].GetCategoryName());

            newCategoryItem.GetComponentInChildren<Text>().text = customCategories[i].GetCategoryName();
        }
    }

    public List<CustomCategory> GetCustomCategories()
    {
        return customCategories;
    }

    public CustomCategory GetCustomCategoryInfo(int id)
    {
        foreach (CustomCategory tmp in customCategories)
        {
            if (tmp.GetCustomCategoryId().Equals(id))
                return tmp;
        }

        return null;
    }
}