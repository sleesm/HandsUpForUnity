using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CategoryData", menuName = "ScriptableObjects/categoryData", order = 1)]
public class CategoryData : ScriptableObject
{
    public int category_id;
    public int user_id;
    public string name;
    public bool access;
    public bool is_built_in;
    public bool is_shared;
}

