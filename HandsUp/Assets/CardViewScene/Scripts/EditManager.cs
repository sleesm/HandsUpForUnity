using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EditManager : MonoBehaviour
{
    private PlayerManager playerManager;
    private CategoryManager categoryManager;
    private CardManager cardManager;
    private ImageManager imageManager;

    private Category category;
    private Card card;

    public InputField categoryName;
    public InputField cardName;
    public Dropdown dropdown;

    private bool access;
    private int selectedCategoryId;


    void Start()
    {
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        categoryManager = this.gameObject.GetComponent<CategoryManager>();
        cardManager = this.gameObject.GetComponent<CardManager>();
        imageManager = GameObject.Find("CardViewManager").GetComponent<ImageManager>();
    }

    public void OnClickEditBtn()
    {
        GameObject.Find("Canvas").transform.Find("CardViewPage").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("EditCategoryPage").gameObject.SetActive(true);
        cardManager.InitCards(category.GetCategoryId(), "EditCategoryPage");

        //설정 값 불러오기
        GameObject.Find("Canvas").transform.Find("EditCategoryPage/CategoryName").GetComponent<InputField>().text = category.GetName();
        GameObject.Find("Canvas").transform.Find("EditCategoryPage/Toggles/Public").GetComponent<Toggle>().isOn = category.GetAccess();
        
        // 빌트인 카테고리는 수정 불가능
        bool isInteractable = !category.GetCategoryIsBuiltIn();
        GameObject.Find("Canvas").transform.Find("EditCategoryPage/CategoryName").GetComponent<InputField>().interactable = isInteractable;
        GameObject.Find("Canvas").transform.Find("EditCategoryPage/Toggles").gameObject.SetActive(isInteractable);
        
    }

    public void InitEditCategories(Category cate)
    {
        category = cate;
    }

    public void OnClickCategoryEditBtn()
    {
        CategoryData categoryData = new CategoryData();
        categoryData.category_id = category.GetCategoryId();
        categoryData.name = categoryName.text;
        categoryData.user_id = playerManager.GetUserId();
        categoryData.access = access;

        var req = JsonConvert.SerializeObject(categoryData);
        StartCoroutine(DataManager.sendDataToServer("category/update", req, (raw) =>
        {
            Debug.Log(raw);
            JObject applyJObj = JObject.Parse(raw);
            if (applyJObj["result"].ToString().Equals("success"))
            {
                Debug.Log("results : success");
                StartCoroutine(OpenPopUp("카테고리가 수정되었습니다.", true));
                category.SetName(categoryName.text);
                category.SetAccess(access);
            }
            else
            {
                Debug.Log("results : fail");
                StartCoroutine(OpenPopUp("카테고리가 수정되지 않았습니다."));
            }

        }));
    }

    private IEnumerator OpenPopUp(string content, bool isSucess = false)
    public void InitEditCard(Card ca)
    {
        card = ca;
        //설정 값 불러오기
        StartCoroutine(cardManager.getImagesFromURL(card.GetImagePath(), GameObject.Find("PopUpPages/EditCardPopUp").transform.Find("CardImg").gameObject));
        GameObject.Find("Canvas").transform.Find("PopUpPages/EditCardPopUp/CardName").GetComponent<InputField>().text = card.GetName();
        
        dropdown.value = categoryManager.GetCategoryIndex(category.GetCategoryId());
        dropdown.Select();
        dropdown.RefreshShownValue();
        selectedCategoryId = category.GetCategoryId();
    }

    public void InitDropdownOptions()
    {
        dropdown.options.Clear();
        List<Category> categories = categoryManager.GetCategories();
        foreach (Category category in categories)
        {
            dropdown.options.Add(new Dropdown.OptionData(category.GetName()));
        }
        dropdown.value = 0;
        dropdown.RefreshShownValue();
    }

    public void OnDropdownChanged(Dropdown select)
    {
        selectedCategoryId = categoryManager.GetCategory(select.value).GetCategoryId();
    }

    public int GetEditCardsCategory()
    {
        return category.GetCategoryId();
    }

    {
        GameObject.Find("PopUpPages").transform.Find("AlarmPopUp").GetComponentInChildren<Text>().text = content;
        GameObject.Find("PopUpPages").transform.Find("AlarmPopUp").gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        GameObject.Find("PopUpPages").transform.Find("AlarmPopUp").gameObject.SetActive(false);
        if (isSucess)
        {
            GameObject.Find("Canvas").transform.Find("CardViewPage/CardsScrollView").gameObject.SetActive(true);
            GameObject.Find("Canvas").transform.Find("EditCategoryPage").gameObject.SetActive(false);
        }
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
