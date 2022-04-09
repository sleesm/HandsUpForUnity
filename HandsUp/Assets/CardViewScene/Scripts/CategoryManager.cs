using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class CategoryManager : MonoBehaviour
{
    private List<Category> categories;
    private List<CustomCategory> customCategories;

    public List<Category> GetBuitInCategoriesFromServer()
    {
        categories = new List<Category>();

        StartCoroutine(DataManager.getDataFromServer("/category", (raw) =>
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

        // For test code
        /*        Category test = new Category();
                test.SetId(0);
                test.SetName("³¯¾¾");

                categories.Add(test);


                Category test2 = new Category();
                test2.SetId(1);
                test2.SetName("°úÀÏ");

                categories.Add(test2);*/

        return categories;
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


    public List<CustomCategory> GetCustomCategoriesFromServer(int userId)
    {
        UserData userData = new UserData();
        userData.userId = userId;

        var req = JsonConvert.SerializeObject(userData);

        customCategories = new List<CustomCategory>();

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