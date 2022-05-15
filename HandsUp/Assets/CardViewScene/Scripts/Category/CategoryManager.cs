using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CategoryManager : MonoBehaviour
{
    private PlayerManager playerManager;
    public GameObject categoryItem;

    private List<Category> categories;
    private List<Category> customCategories;
    private List<Category> allCategories;

    private bool isCategoryLoaded = false;
    private bool isCustomCategoryLoaded = false;


    private void Awake()
    {
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        categories = new List<Category>();
        customCategories = new List<Category>();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name.Equals("CardViewScene") && isCategoryLoaded && isCustomCategoryLoaded)
        {
            SetAllCategories();
            if (playerManager.GetUserId() >= 0)
                CreateAddCategoryBtn();
        }

    }


    public void InitCategories()
    {
        string path = "";

        if (SceneManager.GetActiveScene().name.Equals("CardViewScene")) {
            path = "CardViewPage";
            GameObject.Find("Canvas").transform.Find("CardViewPage/PR_CategoriesScroll").gameObject.SetActive(true);
            GameObject.Find("Canvas").transform.Find("CardViewPage/CardsScrollView").gameObject.SetActive(false);
        }
        else if (SceneManager.GetActiveScene().name.Equals("GameSelectScene"))
        {
            path = "SelectCategoryPage";
            GameObject.Find("Canvas").transform.Find("SelectCategoryPage/PR_CategoriesScroll").gameObject.SetActive(true);
            categories.Clear();
            customCategories.Clear();
        }

        if (categories.Count <= 0)
        {
            GetBuiltInCategoriesFromServer(path);
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
            GetCustomCategoriesFromServer(playerManager.GetUserId(), path);
        }
        else
        {
            isCustomCategoryLoaded = true;
        }

    }

    private void CreateAddCategoryBtn()
    {
        isCategoryLoaded = false;
        isCustomCategoryLoaded = false;
        GameObject newCategoryItem = Instantiate(categoryItem, new Vector3(0, 0, 0), Quaternion.identity);
        newCategoryItem.transform.SetParent(GameObject.Find("Canvas").transform.Find("CardViewPage/PR_CategoriesScroll/Viewport/Content").transform);
        newCategoryItem.transform.localScale = new Vector3(1, 1, 1);
        newCategoryItem.GetComponentInChildren<Text>().text = "+";
    }


    private void GetBuiltInCategoriesFromServer(string path)
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
                   tmp.SetCustomCategoryId(-1);
                   tmp.SetId((int)tmpCategory["category_id"]);
                   tmp.SetName(tmpCategory["category_name"].ToString());
                   tmp.SetAccess(false);

                   categories.Add(tmp);
               }
               CreateNewCategoryItems(categories, path);
               isCategoryLoaded = true;
           }
           else
           {
               Debug.Log("results : fail");
           }

       }));
    }


    private void CreateNewCategoryItems(List<Category> categories, string path)
    {
        for (int i = 0; i < categories.Count; i++)
        {
            GameObject newCategoryItem = Instantiate(categoryItem, new Vector3(0, 0, 0), Quaternion.identity);
            newCategoryItem.transform.SetParent(GameObject.Find("Canvas").transform.Find(path).transform.Find("PR_CategoriesScroll/Viewport/Content").transform);

            newCategoryItem.transform.localScale = new Vector3(1, 1, 1);
            newCategoryItem.GetComponent<Category>().SetId(categories[i].GetId());
            newCategoryItem.GetComponent<Category>().SetName(categories[i].GetName());

            newCategoryItem.GetComponentInChildren<Text>().text = categories[i].GetName();
        }
    }


    public List<Category> GetBuiltInCategories()
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


    private void GetCustomCategoriesFromServer(int userId, string path)
    {
        UserData userData = new UserData();
        userData.user_id = userId;

        var req = JsonConvert.SerializeObject(userData);

        customCategories = new List<Category>();

        StartCoroutine(DataManager.sendDataToServer("category/custom", req, (raw) =>
        {
            Debug.Log(raw);
            JObject applyJObj = JObject.Parse(raw);
            if (applyJObj["result"].ToString().Equals("success"))
            {
                foreach (JObject tmpCategory in applyJObj["categories"])
                {
                    Category tmp = new Category();
                    tmp.SetCustomCategoryId((int)tmpCategory["category_custom_id"]);
                    tmp.SetId((int)tmpCategory["category_id"]);
                    tmp.SetName(tmpCategory["category_name"].ToString());
                    tmp.SetAccess((bool)tmpCategory["category_access"]);

                    customCategories.Add(tmp);
                }

                CreateNewCustomCategoryItems(customCategories, path);
                isCustomCategoryLoaded = true;
            }
            else
            {
                Debug.Log("results : fail");
            }

        }));
    }


    private void CreateNewCustomCategoryItems(List<Category> customCategories, string path)
    {
        for (int i = 0; i < customCategories.Count; i++)
        {
            GameObject newCategoryItem = Instantiate(categoryItem, new Vector3(0, 0, 0), Quaternion.identity);
            newCategoryItem.transform.SetParent(GameObject.Find("Canvas").transform.Find(path).transform.Find("PR_CategoriesScroll/Viewport/Content").transform);
            newCategoryItem.transform.localScale = new Vector3(1, 1, 1);
            newCategoryItem.tag = "customCategoryItem";
            newCategoryItem.GetComponent<Category>().SetId(customCategories[i].GetCustomCategoryId());
            newCategoryItem.GetComponent<Category>().SetName(customCategories[i].GetName());

            newCategoryItem.GetComponentInChildren<Text>().text = customCategories[i].GetName();
        }
    }

    public List<Category> GetCustomCategories()
    {
        return customCategories;
    }

    public Category GetCustomCategoryInfo(int id)
    {
        foreach (Category tmp in customCategories)
        {
            if (tmp.GetCustomCategoryId().Equals(id))
                return tmp;
        }

        return null;
    }

    private void SetAllCategories()
    {
        allCategories = new List<Category>();
        foreach (Category tmp in categories)
        {
            allCategories.Add(tmp);
        }
        if (customCategories.Count > 0)
            foreach (Category tmp in customCategories)
            {
                allCategories.Add(tmp);
            }
    }

    public List<Category> GetAllCategoris()
    {
        return allCategories;
    }

    public Category GetCategory(int index)
    {
        return allCategories[index];
    }
}