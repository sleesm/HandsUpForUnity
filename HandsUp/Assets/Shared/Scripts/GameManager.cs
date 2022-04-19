using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int gameVersion = -1;
    public int gameCategory = -1;

    public int GetGameVersion()
    {
        return gameVersion;
    }

    public void SetGameVersion(int gameVersion)
    {
        this.gameVersion = gameVersion;
    }

    public int GetGameCategory()
    {
        return gameCategory;
    }

    public void SetGameCategory(int gameCategory)
    {
        this.gameCategory = gameCategory;
    }
}
