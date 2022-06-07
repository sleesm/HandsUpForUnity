using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleManager : MonoBehaviour
{
    public Toggle toggle;
    private Category category;

    void OnEnable()
    {
        toggle.isOn = category.GetAccess();
    }

    public void InitEditCategories(Category cate)
    {
        category = cate;
    }

}
