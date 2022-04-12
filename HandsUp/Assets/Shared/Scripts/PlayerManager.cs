using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int userId = -1;
    public string name = null;
    public string email = null;

    public int GetUserId()
    {
        return userId;
    }

    public void SetUserId(int userId)
    {
        this.userId = userId;
    }

    public string GetUserName()
    {
        return name;
    }

    public void SetUserName(string name)
    {
        this.name = name;
    }

    public string GetUserEmail()
    {
        return email;
    }

    public void SetUserEmail(string email)
    {
        this.email = email;
    }

    public void InitPlayerData()
    {
        userId = -1;
        name = "";
        email = "";
    }
}
