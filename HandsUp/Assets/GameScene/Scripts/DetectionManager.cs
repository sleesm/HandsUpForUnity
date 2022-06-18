using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionManager : MonoBehaviour
{

    private GameData gameData;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    public void GetResultFromImg(string jpgBase64)
    {
        gameData = new GameData();
        gameData.gameVersion = gameManager.GetGameVersion();
        gameData.image = jpgBase64;
        gameData.answer = gameManager.GetAnswer();
        var req = JsonConvert.SerializeObject(gameData);

        StartCoroutine(DataManager.sendDataToServer("game/result", req, (raw) =>
        {
            Debug.Log(raw);
            JObject res = JObject.Parse(raw);

            if(res["result"].ToString().Equals("fail"))
            {
                Debug.Log("통신 에러가 있습니다. 혹은 인식된 값이 하나도 없습니다.");
                GameManager.isResultCorrect = false;
            }
            else if(res["result"].ToString().Equals("success"))
            {
                if(res["correct"].ToString().Equals("correct"))
                {
                    GameManager.isResultCorrect = true;
                }
                else
                {
                    GameManager.isResultCorrect = false;
                }
            }

            GameManager.isResultGot = true;
        }));
    }

    public void OnClickNextProbBtn()
    {
        gameManager.showTime = 0;
        GameObject.Find("PopUpPages").transform.Find("OXPopUp").gameObject.SetActive(false);
        GameObject.Find("PopUpPages").transform.Find("CardInfoPopUp").gameObject.SetActive(false);
        GameManager.isNextGameReady = true;
    }
}
