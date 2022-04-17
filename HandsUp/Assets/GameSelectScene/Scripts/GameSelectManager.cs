using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSelectManager : MonoBehaviour
{

    public void OnClickSelectCategoryBtn()
    {
        InitPopUp();
        SetPageActive(1);
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
