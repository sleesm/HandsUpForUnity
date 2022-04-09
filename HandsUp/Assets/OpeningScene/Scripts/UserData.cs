using System;
using UnityEngine;

[CreateAssetMenu(fileName = "UserData", menuName = "ScriptableObjects/userData", order = 1)]
public class UserData : ScriptableObject
{
    public int userId; // 사용자 번호
    public string email; // 사용자 이메일
    public string userName; // 사용자 이름
    public string password; // 비밀번호
}
