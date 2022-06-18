using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameSettingManager : MonoBehaviour
{
    public InputField[] gameSettingDataField;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void ShowSettingPopUp()
    {
        gameSettingDataField[0].text = gameManager.GetTimeLimit().ToString();
        gameSettingDataField[1].text = gameManager.GetProblemNum().ToString();
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
        if (btnName.Equals("TimePlusBtn"))
        {
            tl = int.Parse(gameSettingDataField[0].text);
            tl++;
            gameSettingDataField[0].text = tl.ToString();
        }
        else if (btnName.Equals("TimeMinusBtn"))
        {
            tl = int.Parse(gameSettingDataField[0].text);
            if (tl != 1)
            {
                tl--;
                gameSettingDataField[0].text = tl.ToString();
            }
        }
        else if (btnName.Equals("ProblemPlusBtn"))
        {
            pn = int.Parse(gameSettingDataField[1].text);
            pn++;
            gameSettingDataField[1].text = pn.ToString();
        }
        else if (btnName.Equals("ProblemMinusBtn"))
        {
            pn = int.Parse(gameSettingDataField[1].text);
            if (pn != 1)
            {
                pn--;
                gameSettingDataField[1].text = pn.ToString();
            }
        }
    }


    //팝업창 설정 후 확인 버튼
    public void OnClickSetBtn()
    {
        int tl = int.Parse(gameSettingDataField[0].text);
        int pn = int.Parse(gameSettingDataField[1].text);
        gameManager.SetTimeLimit(tl);
        gameManager.SetProblemNum(pn);
        GameObject.Find("PopUpPages").transform.Find("GameSettingPopUp").gameObject.SetActive(false);
        if (SceneManager.GetActiveScene().name.Equals("GameSelectScene"))
        {
            gameManager.GetCards();
            SceneManager.LoadScene("GameScene");
        }
            
    }

    private void InitPopUp()
    {
        for (int i = 0; i < GameObject.Find("PopUpPages").transform.childCount; i++)
        {
            GameObject.Find("PopUpPages").transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
