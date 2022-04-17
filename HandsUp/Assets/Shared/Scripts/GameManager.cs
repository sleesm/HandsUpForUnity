using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int gameVersion = -1;

    public int GetGameVersion()
    {
        return gameVersion;
    }

    public void SetGameVersion(int gameVersion)
    {
        this.gameVersion = gameVersion;
    }
}
