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

    private bool isCategoryLoaded = false;

    private void Awake()
    {
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        categories = new List<Category>();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name.Equals("CardViewScene") && isCategoryLoaded)
        {
            GetCategories();
            if (playerManager.GetUserId() >= 0)
                CreateAddCategoryBtn();
        }
    }

    public void InitCategories(bool isFromCard = false, bool isCreate = true, string api = "category")
    {
        string path = "";
        if (SceneManager.GetActiveScene().name.Equals("CardViewScene")) {
            path = "CardViewPage";
            if (!isFromCard)
            {
                GameObject.Find("Canvas").transform.Find("CardViewPage/PR_CategoriesScroll").gameObject.SetActive(true);
                GameObject.Find("Canvas").transform.Find("CardViewPage/CardsScrollView").gameObject.SetActive(false);
            }
            if (api.Equals("category/custom/public"))
                path = "OthersCategoryPage";
        }
        else if (SceneManager.GetActiveScene().name.Equals("GameSelectScene"))
        {
            path = "SelectCategoryPage";
            GameObject.Find("Canvas").transform.Find("SelectCategoryPage/PR_CategoriesScroll").gameObject.SetActive(true);
            categories.Clear();
        }

        if (categories.Count > 0)
        {
            DestoryCategories();
            categories.Clear();
        }

        if (categories.Count <= 0)
        {
            GetCategoriesFromServer(path, api, isCreate);
        }
    }

    public void DestoryCategories()
    {
        GameObject[] content = GameObject.FindGameObjectsWithTag("categoryItem");
        for (int i = 0; i < content.Length; i++)
        {
            Destroy(content[i]);
        }
    }

    private void CreateAddCategoryBtn()
    {
        isCategoryLoaded = false;
        GameObject newCategoryItem = Instantiate(categoryItem, new Vector3(0, 0, 0), Quaternion.identity);
        newCategoryItem.transform.SetParent(GameObject.Find("Canvas").transform.Find("CardViewPage/PR_CategoriesScroll/Viewport/Content").transform);
        newCategoryItem.transform.localScale = new Vector3(1, 1, 1);
        newCategoryItem.GetComponentInChildren<Text>().text = "+";
    }

    private void GetCategoriesFromServer(string path, string api, bool isCreate = true)
    {
        CategoryData categoryData = new CategoryData();
        if (playerManager.GetUserId() >= 0)
            categoryData.user_id = playerManager.GetUserId();

        var req = JsonConvert.SerializeObject(categoryData);

        StartCoroutine(DataManager.sendDataToServer(api, req, (raw) =>
        {
            Debug.Log(raw);
            JObject applyJObj = JObject.Parse(raw);
            if (applyJObj["result"].ToString().Equals("success"))
            {
                foreach (JObject tmpCategory in applyJObj["categories"])
                {
                    Category tmp = new Category();
                    tmp.SetCategoryId((int)tmpCategory["category_id"]);
                    tmp.SetName(tmpCategory["category_name"].ToString());
                    tmp.SetAccess((bool)tmpCategory["category_access"]);
                    tmp.SetCustomCategoryId((int)tmpCategory["category_custom_id"]);
                    if ((int)tmpCategory["category_is_built_in"] == 1)
                    {
                        tmp.SetCategoryIsBuiltIn(true);
                        tmp.SetUserId(-1);
                    }
                    else
                    {
                        tmp.SetCategoryIsBuiltIn(false);
                        tmp.SetUserId(playerManager.GetUserId());
                    }

                    categories.Add(tmp);
                }

                if (isCreate)
                {
                    CreateNewCategoryItems(categories, path);
                    isCategoryLoaded = true;
                }
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
            newCategoryItem.GetComponent<Category>().SetCategoryId(categories[i].GetCategoryId());
            newCategoryItem.GetComponent<Category>().SetName(categories[i].GetName());
            newCategoryItem.GetComponent<Category>().SetAccess(categories[i].GetAccess());
            newCategoryItem.GetComponent<Category>().SetCustomCategoryId(categories[i].GetCustomCategoryId());
            if (categories[i].GetCategoryIsBuiltIn())
            {
                newCategoryItem.GetComponent<Category>().SetCategoryIsBuiltIn(true);
                newCategoryItem.GetComponent<Image>().color = new Color32(210, 211, 241, 255);
            }
            else
            {
                newCategoryItem.GetComponent<Category>().SetCategoryIsBuiltIn(false);
                newCategoryItem.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            }

            if (path.Equals("OthersCategoryPage"))
                newCategoryItem.transform.Find("ScrapImg").gameObject.SetActive(true);
            newCategoryItem.GetComponentInChildren<Text>().text = categories[i].GetName();
        }
    }

    public List<Category> GetCategories()
    {
        return categories;
    }

    public Category GetCategoryInfo(int id)
    {
        foreach(Category tmp in categories)
        {
            if (tmp.GetCategoryId().Equals(id))
                return tmp;
        }

        return null;
    }
    
    public Category GetCategory(int index)
    {
        return categories[index];
    }

    public int GetCategoryIndex(int categoryId)
    {
        int index;
        
        for(index = 0; index < categories.Count; index++)
        {
            if (categories[index].GetCategoryId() == categoryId)
                break;
                
        }
        return index;
    }
    
}