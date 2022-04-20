using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int gameVersion = -1;
    public int gameCategory = -1;
    public int timeLimit = 5;
    public int problemNum = 5;
    public int gameTimeLimit = -1;
    public int gameProblemNum = -1;

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

    public int GetTimeLimit()
    {
        return timeLimit;
    }

    public void SetTimeLimit(int timeLimit)
    {
        this.timeLimit = timeLimit;
    }

    public int GetProblemNum()
    {
        return problemNum;
    }

    public void SetProblemNum(int problemNum)
    {
        this.problemNum = problemNum;
    }

    public int GetGameTimeLimit()
    {
        return gameTimeLimit;
    }

    public void SetGameTimeLimit(int gameTimeLimit)
    {
        this.gameTimeLimit = gameTimeLimit;
    }

    public int GetGameProblemNum()
    {
        return gameProblemNum;
    }

    public void SetGameProblemNum(int gameProblemNum)
    {
        this.gameProblemNum = gameProblemNum;
    }
}
