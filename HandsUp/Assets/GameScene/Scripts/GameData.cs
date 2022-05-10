using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/gameData", order = 1)]
public class GameData : ScriptableObject
{
    public int gameVersion;
    public string image; // 전송할 이미지
    public string answer;
}
