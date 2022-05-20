using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BackBtnManager : MonoBehaviour
{

    public void OnClickBackBtn()
    {
        string firstView = "";
        string secondView = "";
        string nextScene = "";

        switch (SceneManager.GetActiveScene().name)
        {
            case "CardViewScene":
                if(EventSystem.current.currentSelectedGameObject.transform.parent.name == "CustomPage" || EventSystem.current.currentSelectedGameObject.transform.parent.name == "ItemAddPage")
                {
                    firstView = "ItemAddPage";
                    secondView = "CustomPage";
                    nextScene = "CardViewScene";
                    GameObject.Find("Canvas").transform.Find("CustomPage").GetComponent<CustomManager>().InitCustomPages();
                }
                else
                {
                    firstView = "CardViewPage/PR_CategoriesScroll";
                    secondView = "CardViewPage/CardsScrollView";
                    nextScene = "OpeningScene";

                    GameObject.Find("Canvas").transform.Find("CardViewPage/EditBtn").gameObject.SetActive(false);
                }
                break;

            case "GameSelectScene":
                firstView = "SelectGamePage";
                secondView = "SelectCategoryPage";
                nextScene = "OpeningScene";
                break;
            case "GameScene":
                firstView = "PopUpPages/ResultPopUp";
                secondView = "PopUpPages/CardListPopUp";
                nextScene = "OpeningScene";
                break;

        }

        if (GameObject.Find("Canvas").transform.Find(firstView).gameObject.activeSelf == true)
        {
            SceneManager.LoadScene(nextScene);
        }
        else if (GameObject.Find("Canvas").transform.Find(secondView).gameObject.activeSelf == true)
        {
            GameObject.Find("Canvas").transform.Find(secondView).gameObject.SetActive(false);
            GameObject.Find("Canvas").transform.Find(firstView).gameObject.SetActive(true);
        }
    }
}
