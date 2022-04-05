using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private int userId = -1;
    private string name = null;
    private string email = null;

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
}
