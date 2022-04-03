using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class CategoryManager : MonoBehaviour
{

    public List<Category> GetBuitInCategories()
    {
        List<Category> categories = new List<Category>();

        StartCoroutine(DataManager.getDataFromServer("/category",  (raw) =>
        {
            Debug.Log(raw);
            JObject applyJObj = JObject.Parse(raw);
            foreach (JObject tmpCategory in applyJObj["categories"])
            {
                Category tmp = new Category();
                tmp.SetId((int)tmpCategory["category_id"]);
                tmp.SetName(tmpCategory["category_name"].ToString());

                categories.Add(tmp);
            }

        }));

        return categories;
    }

    public List<CustomCategory> GetCustomCategories(int userId)
    {
        UserData userData = new UserData();
        userData.userId = userId;

        var req = JsonConvert.SerializeObject(userData);

        List<CustomCategory> customCategories = new List<CustomCategory>();

        StartCoroutine(DataManager.sendDataToServer("/category/custom", req, (raw) =>
        {
            Debug.Log(raw);
            JObject applyJObj = JObject.Parse(raw);
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

        }));

        return customCategories;
    }

}

public class Category
{
    int categoryId;
    string categoryName;

    public int GetId()
    {
        return categoryId;
    }

    public void SetId(int id)
    {
        this.categoryId = id;
    }

    public string GetName()
    {
        return categoryName;
    }
    public void SetName(string name)
    {
        this.categoryName = name;
    }
}

public class CustomCategory
{
    int customCategoryId;
    int categoryId;
    string categoryName;
    int count;
    bool access;

    public int GetCustomCategoryId()
    {
        return customCategoryId;
    }

    public void SetCustomCategoryId(int id)
    {
        this.customCategoryId = id;
    }

    public int GetCategoryId()
    {
        return categoryId;
    }

    public void SetCategoryId(int id)
    {
        this.categoryId = id;
    }

    public string GetCategoryName()
    {
        return categoryName;
    }

    public void SetCategoryName(string name)
    {
        this.categoryName = name;
    }

    public int GetCount()
    {
        return count;
    }

    public void SetCount(int count)
    {
        this.count = count;
    }

    public bool GetAccess()
    {
        return access;
    }
    public void SetAccess(bool access)
    {
        this.access = access;
    }
}