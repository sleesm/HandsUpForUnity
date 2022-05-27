using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OthersManager : MonoBehaviour
{
    private CardManager cardManager;
    private CategoryManager categoryManager;
    private PlayerManager playerManager;

    private Category category;

    private void Awake()
    {
        cardManager = GameObject.Find("CardViewManager").gameObject.GetComponent<CardManager>();
        categoryManager = GameObject.Find("CardViewManager").gameObject.GetComponent<CategoryManager>();
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
    }

    public void InitOthersCategory()
    {
        categoryManager.InitCategories(false ,true, "category/custom/public");
    }

    public void OnClikCancelBtn()
    {
        GameObject.Find("PopUpPages").transform.Find("AddOthersCategoryPopUp").gameObject.SetActive(false);
    }

    public void OnClcikAddCategoryBtn()
    {
        CategoryData categoryData = new CategoryData();
        categoryData.user_id = playerManager.GetUserId();
        categoryData.name = category.GetName();
        categoryData.access = category.GetAccess();
        categoryData.is_shared = true;

        var req = JsonConvert.SerializeObject(categoryData);
        StartCoroutine(DataManager.sendDataToServer("category/create", req, (raw) =>
        {
            Debug.Log(raw);
            JObject applyJObj = JObject.Parse(raw);
            if (applyJObj["result"].ToString().Equals("success"))
            {
                Debug.Log("results : success");
                int newCategoryId = (int)applyJObj["category_id"];
                GetCardListFromCategoryId(newCategoryId);
            }
            else
            {
                Debug.Log("results : fail");
                StartCoroutine(OpenPopUp("카테고리가 추가되지 않았습니다."));
            }

        }));
    }

    private void GetCardListFromCategoryId(int categoryId)
    {
        CardData cardData = new CardData();
        cardData.category_id = category.GetCategoryId();
        cardData.user_id = playerManager.GetUserId();

        var req = JsonConvert.SerializeObject(cardData);
        StartCoroutine(DataManager.sendDataToServer("category/card/custom/public", req, (raw) =>
        {
            Debug.Log(raw);
            JObject applyJObj = JObject.Parse(raw);
            if (applyJObj["result"].ToString().Equals("success"))
            {
                Debug.Log("results : success");
                foreach (JObject tmpCard in applyJObj["cards"])
                {
                    int id = (int)tmpCard["card_id"];
                    string name = tmpCard["card_name"].ToString();
                    string imgPath = tmpCard["card_img_path"].ToString();

                    AddCardsFromCategory(categoryId, name, imgPath);
                }
                StartCoroutine(OpenPopUp("카테고리가 추가되었습니다.", true));
            }
            else
            {
                Debug.Log("results : fail");
                StartCoroutine(OpenPopUp("카테고리가 추가되지 않았습니다."));
            }


        }));
    }

    private void AddCardsFromCategory(int categoryId, string cardName, string imgPath)
    {
        CardData cardData = new CardData();

        cardData.user_id = playerManager.GetUserId();
        cardData.name = cardName;
        cardData.category_id = categoryId;
        cardData.img_path = imgPath;
        cardData.is_new = false;

        var req = JsonConvert.SerializeObject(cardData);
        StartCoroutine(DataManager.sendDataToServer("category/card/create", req, (raw) =>
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

    private IEnumerator OpenPopUp(string content, bool isSucess = false)
    {
        GameObject.Find("PopUpPages").transform.Find("AlarmPopUp").GetComponentInChildren<Text>().text = content;
        GameObject.Find("PopUpPages").transform.Find("AlarmPopUp").gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        GameObject.Find("PopUpPages").transform.Find("AlarmPopUp").gameObject.SetActive(false);
        if (isSucess)
            GameObject.Find("PopUpPages").transform.Find("AddOthersCategoryPopUp").gameObject.SetActive(false);
    }

    public void SetCurrentCaategory(Category cate)
    {
        category = cate;
    }
}
