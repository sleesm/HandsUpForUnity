using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

public class OpeneingManager : MonoBehaviour
{
    private PlayerManager playerManager;

    private UserData userData;

    public InputField[] userDatas; // for sign in
    public InputField[] userDataField; // for sign up
    public InputField[] userEditDataField; // for edit

    private void Start()
    {
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
    }

    private void Update()
    {
        // for nickname button's text
        if (playerManager.GetUserName() != "")
        {
            GameObject.Find("Canvas").transform.Find("MainPage/Btns/NickNameBtn").GetComponentInChildren<Text>().text = playerManager.GetUserName();
        }
        else
        {
            GameObject.Find("Canvas").transform.Find("MainPage/Btns/NickNameBtn").GetComponentInChildren<Text>().text = "로그인";
        }
    }


    public void OnClickSignUpPageBtn()
    {
        SetPageActive(1);
    }

    public void OnClickSignInPageBtn()
    {
        SetPageActive(0);
    }

    public void OnClickMainPageBtn()
    {
        SetPageActive(2);
    }

    public void OnClickNicknameBtn()
    {
        // 닉네임이 있으면(=회원이면) 편집 페이지로 이동, 없으면(=비회원이면) 로그인 페이지로 이동
        if (playerManager.GetUserName() != "") 
        {
            // Edit페이지 내 아이디, 닉네임 입력 창에 아이디와 닉네임 희미하게 띄워주기 위해
            GameObject.Find("Canvas").transform.Find("EditPage/EditTxt/IDField").GetComponentInChildren<Text>().text = playerManager.GetUserEmail();
            GameObject.Find("Canvas").transform.Find("EditPage/EditTxt/NickNameField").GetComponentInChildren<Text>().text = playerManager.GetUserName();

            SetPageActive(3);
        }
        else
            SetPageActive(0);
    }


    /// <summary>
    /// Init All Pages
    /// </summary>
    /// <param name="index"></param>
    private void SetPageActive(int index) // 0 : DefaultPage, 1 : SignUpPage, 2 : MainPage, 3 : EditPage
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject.Find("Canvas").transform.GetChild(i).gameObject.SetActive(false);
        }
        GameObject.Find("Canvas").transform.GetChild(index).gameObject.SetActive(true);
    }



    /// <summary>
    /// Sign up event
    /// </summary>
    public void OnClickSignUpBtn()
    {
        if(!CheckValidFormat(userDataField))
            return;
        SignUp();
    }

    //TO-DO : add alarms
    private void SignUp()
    {
        userData = new UserData();
        userData.email = userDataField[0].text;
        userData.name = userDataField[1].text; 
        userData.password = userDataField[2].text; 

        var req = JsonConvert.SerializeObject(userData);
        Debug.Log(req);
        StartCoroutine(DataManager.sendDataToServer("auth/signup", req, (raw) =>
        {
            Debug.Log("sign up user data : \n" + req);

            JObject res = JObject.Parse(raw);

            if (res["result"].ToString().Equals("fail")) // wrong id
            {
                Debug.Log("Fail Sign Up");
            }
            else {

                Debug.Log("Sucessful Sign Up!");
                SetPageActive(0);
            }

        }));
    }


    /// <summary>
    /// check email format or confirm pw is same with pw
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    private bool CheckValidFormat(InputField[] field)
    {
        Debug.Log("check");
        //check user email form
        string userEmail = field[0].text;
        bool valid = Regex.IsMatch(userEmail, @"[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?");
        if (!valid)
        {
            Debug.Log("Verified Email");
            return false;
        }

        //TO-DO : add PW Diffrent alarm
        // check confirm PW
        if (!field[3].text.Equals(field[2].text))
        {
            Debug.Log("PW is diffrent with Confirm PW.");
            return false;
        }

        return true;
    }


    /// <summary>
    /// Sign in event
    /// </summary>
    public void OnClickSignInBtn()
    {
        SignIn();
    }

    //TO-DO : add alarms
    private void SignIn()
    {
        userData = new UserData();
        userData.email = userDatas[0].text;
        userData.password = userDatas[1].text;

        var req = JsonConvert.SerializeObject(userData);
        Debug.Log(req);
        StartCoroutine(DataManager.sendDataToServer("auth/signin", req, (raw) =>
        {
            Debug.Log("sign in user data : \n" + req);

            JObject res = JObject.Parse(raw);

            if (res["result"].ToString().Equals("fail")) // wrong id
            {
                Debug.Log("Fail Sign In");
            }
            else if (res["result"].ToString().Equals("success")) // wrong pw
            {
                Debug.Log("Sucessful Sign In!");
                Debug.Log((int)res["user_id"]);
                playerManager.SetUserId((int)res["user_id"]);
                playerManager.SetUserName(res["user_name"].ToString());
                playerManager.SetUserEmail(userData.email);
                SetPageActive(2);
            }

        }));
    }



    /// <summary>
    /// Edit user info event
    /// </summary>
    public void OnClickEditBtn()
    {
        if(!CheckValidFormat(userEditDataField))
            return;
        EditInfo();
    }

    private void EditInfo()
    {
        //정보를 받아서 바로 쏴주기 때문에 SignUp과 똑같지만 DB 질의문이 달라지는 구조라고 생각했습니다
        userData = new UserData();
        userData.user_id = playerManager.GetUserId();
        userData.email = userEditDataField[0].text;
        userData.name = userEditDataField[1].text;
        userData.password = userEditDataField[2].text;

        var req = JsonConvert.SerializeObject(userData);
        Debug.Log(req);

        StartCoroutine(DataManager.sendDataToServer("auth/user/update", req, (raw) =>
        {
            Debug.Log("edit user data : \n" + req);
            Debug.Log("user's info : " + raw);
            JObject res = JObject.Parse(raw);

            if (res["result"].ToString().Equals("fail")) // wrong id
            {
                Debug.Log("Fail Editing User Info!");
            }
            else if (res["result"].ToString().Equals("success")) // wrong pw
            {
                Debug.Log("Sucessful Editing User Info!");

                playerManager.SetUserName(userData.name);
                playerManager.SetUserEmail(userData.email);
                SetPageActive(2);
            }
        }));
    }


    /// <summary>
    /// Withdrawal event
    /// </summary>
    public void OnClickWithdrawalBtn()
    {
        // TO-DO : 정말 탈퇴할 것인지 확인하는 창 만들기

    }

}