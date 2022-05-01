using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class GameSelectManager : MonoBehaviour
{
    private GameManager gameManager;
    private CategoryManager categoryManager;

    public InputField[] gameSettingDataField;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        categoryManager = GameObject.Find("GameManager").GetComponent<CategoryManager>();
    }

    //게임 종류 선택
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

    //카테고리 선택 후
    public void ShowSettingPopUp()
    {
        InitPopUp();
        gameSettingDataField[0].text = gameManager.GetTimeLimit().ToString();
        gameSettingDataField[1].text = gameManager.GetProblemNum().ToString();
        GameObject.Find("PopUpPages").transform.Find("GameSettingPopUp").gameObject.SetActive(true);
    }

    //뒤로가기 버튼
    public void OnClickBackBtn()
    {
        if (GameObject.Find("Canvas").transform.Find("SelectCategoryPage").gameObject.activeSelf == true)
        {
            GameObject.Find("Canvas").transform.Find("SelectCategoryPage").gameObject.SetActive(false);
            GameObject.Find("Canvas").transform.Find("SelectGamePage").gameObject.SetActive(true);
        }
            
        else if (GameObject.Find("Canvas").transform.Find("SelectGamePage").gameObject.activeSelf == true)
        {
            SceneManager.LoadScene("OpeningScene");
        }
    }

    //팝업창 취소 버튼
    public void OnClickCancleBtn()
    {
        InitPopUp();
        GameObject.Find("PopUpPages").transform.Find("GameSettingPopUp").gameObject.SetActive(false);
    }

    //+ 또는 - 버튼 -> need refactoring! 
    public void OnClickControlBtn()
    {
        int tl, pn;
        string btnName = EventSystem.current.currentSelectedGameObject.name;
        if(btnName.Equals("TimePlusBtn"))
        {
            tl = int.Parse(gameSettingDataField[0].text);
            tl++;
            gameSettingDataField[0].text = tl.ToString();
        }
        else if(btnName.Equals("TimeMinusBtn"))
        {
            tl = int.Parse(gameSettingDataField[0].text);
            tl--;
            gameSettingDataField[0].text = tl.ToString();
        }
        else if(btnName.Equals("ProblemPlusBtn"))
        {
            pn = int.Parse(gameSettingDataField[1].text);
            pn++;
            gameSettingDataField[1].text = pn.ToString();
        }
        else if(btnName.Equals("ProblemMinusBtn"))
        {
            pn = int.Parse(gameSettingDataField[1].text);
            pn--;
            gameSettingDataField[1].text = pn.ToString();
        }
    }
    //팝업창 설정 후 확인 버튼
    //TO-DO: 게임 화면과 연결
    public void OnClickSetBtn()
    {
        int tl = int.Parse(gameSettingDataField[0].text);
        int pn = int.Parse(gameSettingDataField[1].text);
        gameManager.SetTimeLimit(tl);
        gameManager.SetProblemNum(pn);
        gameManager.InitGame();
        SceneManager.LoadScene("GameScene");
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
