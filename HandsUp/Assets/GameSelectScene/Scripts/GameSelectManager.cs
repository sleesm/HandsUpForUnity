using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class GameSelectManager : MonoBehaviour
{
    private GameManager gameManager;
    private CategoryManager categoryManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        categoryManager = GameObject.Find("GameManager").GetComponent<CategoryManager>();
    }

    public void OnClickSelectCategoryBtn()
    {
        ///get version of game
        string btnName = EventSystem.current.currentSelectedGameObject.name;
        int tmp = btnName.IndexOf('r');
        int version = btnName[tmp + 1] - '0';
  
        gameManager.SetGameVersion(version);
        InitPopUp();
        SetPageActive(1);

        categoryManager.InitCategories();
    }

    public void ShowSettingPopUp()
    {
        InitPopUp();
        GameObject.Find("PopUpPages").transform.Find("GameSettingPopUp").gameObject.SetActive(true);
    }

    public void OnClickCancleBtn()
    {
        InitPopUp();
        GameObject.Find("PopUpPages").transform.Find("GameSettingPopUp").gameObject.SetActive(false);
    }

    /// <summary>
    /// Init All Pages
    /// </summary>
    /// <param name="index"></param>
    private void SetPageActive(int index) // 0 : SelectGamePage, 1 : SelectCategoryPage
    {
        for (int i = 0; i < GameObject.Find("Canvas").transform.childCount - 1; i++)
        {
            GameObject.Find("Canvas").transform.GetChild(i).gameObject.SetActive(false);
        }
        GameObject.Find("Canvas").transform.GetChild(index).gameObject.SetActive(true);
    }

    private void InitPopUp()
    {
        for (int i = 0; i < GameObject.Find("PopUpPages").transform.childCount; i++)
        {
            GameObject.Find("PopUpPages").transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
