using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
                firstView = "CardViewPage/PR_CategoriesScroll";
                secondView = "CardViewPage/CardsScrollView";
                nextScene = "OpeningScene";
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
