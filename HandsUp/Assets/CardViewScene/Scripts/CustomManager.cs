using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomManager : MonoBehaviour
{
    private PlayerManager playerManager;
    private CategoryManager categoryManager;
    
    public InputField categoryName;
    public Dropdown dropdown;

    private bool access;

    private void Start()
    {
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        categoryManager = GameObject.Find("CardViewManager").GetComponent<CategoryManager>();
        InitPages();
        InitDropdownOptions();
    }

    private void InitPages()
    {
        GameObject.Find("CustomPage").transform.Find("CustomCategoryPage").gameObject.SetActive(true);
        GameObject.Find("CustomPage").transform.Find("Menus/CategoryBtn/Panel").gameObject.SetActive(false);

        GameObject.Find("CustomPage").transform.Find("CustomCardPage").gameObject.SetActive(false);
        GameObject.Find("CustomPage").transform.Find("Menus/CardBtn/Panel").gameObject.SetActive(true);
    }

    //TO-DO: Refactoring Code
    public void OnClickBtn()
    {
        string[] tmp = EventSystem.current.currentSelectedGameObject.name.Split('B');
        if (tmp[0].Equals("Category"))
        {
            GameObject.Find("CustomPage").transform.Find("CustomCategoryPage").gameObject.SetActive(true);
            GameObject.Find("CustomPage").transform.Find("Menus/CategoryBtn/Panel").gameObject.SetActive(false);

            GameObject.Find("CustomPage").transform.Find("CustomCardPage").gameObject.SetActive(false);
            GameObject.Find("CustomPage").transform.Find("Menus/CardBtn/Panel").gameObject.SetActive(true);
        }
        else
        {
            GameObject.Find("CustomPage").transform.Find("CustomCategoryPage").gameObject.SetActive(false);
            GameObject.Find("CustomPage").transform.Find("Menus/CategoryBtn/Panel").gameObject.SetActive(true);

            GameObject.Find("CustomPage").transform.Find("CustomCardPage").gameObject.SetActive(true);
            GameObject.Find("CustomPage").transform.Find("Menus/CardBtn/Panel").gameObject.SetActive(false);
        }
    }

    private void InitDropdownOptions()
    {
        dropdown.options.Clear();
        List<Category> categories = categoryManager.GetAllCategoris();
        foreach (Category category in categories)
        {
            dropdown.options.Add(new Dropdown.OptionData(category.GetName()));
        }
        dropdown.value = 0;
        dropdown.RefreshShownValue();
    }

    public void OnClickAddCategoryBtn()
    {
        CategoryData categoryData = new CategoryData();
        categoryData.name = categoryName.text;
        categoryData.user_id = playerManager.GetUserId();
        categoryData.access = access;

        var req = JsonConvert.SerializeObject(categoryData);
        StartCoroutine(DataManager.sendDataToServer("category/create", req, (raw) =>
        {
            Debug.Log(raw);
            JObject applyJObj = JObject.Parse(raw);
            if (applyJObj["result"].ToString().Equals("success"))
            {
                Debug.Log("results : success");
            }
            else
            {
                Debug.Log("results : fail");
            }

        }));
    }

    public void ToggleClick(bool isOn)
    {
        if (isOn)
        {
            access = true;
        }
        else
        {
            access = false;
        }
    }
}
