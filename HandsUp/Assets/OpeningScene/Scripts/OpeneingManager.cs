using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class OpeneingManager : MonoBehaviour
{
    public GameObject[] Pages = new GameObject[4];

    private UserData userData;
    //private PlayerData playerData;

    public InputField[] userDatas; // for sign in
    public InputField[] userDataField; // for sign up
    public InputField[] userEditDataField; // for edit

    public void OnClickSignUpPageBtn()
    {
        SetPageActive(1);
    }

    public void OnClickSignInPageBtn()
    {
        SetPageActive(0);
    }

    public void OnClickWithoutSignPageBtn()
    {
        // 비회원 이용 버튼 클릭 시 닉네임 버튼의 text 값을 로그인으로 변경
        GameObject.Find("NickNameBtn").GetComponentInChildren<Text>().text = "로그인";
        SetPageActive(2);
    }

    public void OnClickNicknamePageBtn()
    {
        // 닉네임이 있으면(=회원이면) 편집 페이지로 이동, 없으면(=비회원이면) 로그인 페이지로 이동
        //if (playerData.GetUserName() != null) 
        //{
        //	SetPageActive(3);
        //}
        //else
        SetPageActive(3);
    }

    // 페이지 초기화
    private void SetPageActive(int index) // 0 : DefaultPage, 1 : SignUpPage 2 : MainPage 3 : EditPage
    {
        for (int i = 0; i < Pages.Length; i++)
        {
            Pages[i].SetActive(false);
        }
        Pages[index].SetActive(true);
    }

    public void OnClickSignUpBtn()
    {
        //user email form checking
        string userEmail = userDataField[0].text;
        bool valid = Regex.IsMatch(userEmail, @"[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?");
        if (valid)
        {
            Debug.Log("Verified Email");
        }

        //TO-DO : add PW Diffrent alarm
        if (!userDataField[3].text.Equals(userDataField[2].text))
        {
            Debug.Log("PW is diffrent with Confirm PW.");
            return;
        }
        
        SignUp();
    }

    //TO-DO : user signup api update, add alarms
    private void SignUp()
    {
        //만약 playerData가 들어간다면 이렇게? 다만 password 부분은 아직 함수가 없어 주석처리 해놓음
        //playerData = new PlayerData();
        //playerData.SetUserEmail(userDataField[0].text);
        //playerData.SetUserName(userDataField[1].text)
        //playerData.SetUserPassword(userDataField[2].text);

        userData = new UserData();
        userData.email = userDataField[0].text;
        userData.userName = userDataField[1].text; 
        userData.password = userDataField[2].text; 

        var req = JsonConvert.SerializeObject(userData);
        Debug.Log(req);
        StartCoroutine(DataManager.sendDataToServer("/user/signup", req, (raw) =>
        {
            Debug.Log("sign up user data : \n" + req);
            Debug.Log("result of sign up : " + raw);

            if (raw.Equals("0"))
            {
                Debug.Log("Sucessful Sign Up!");
                SetPageActive(0);
            }
            else
            {
                Debug.Log("Fail Sign Up");
            }

        }));
    }

    public void OnClickSignInBtn()
    {
        SignIn();
    }

    private void SignIn()
    {
        userData = new UserData();
        userData.email = userDatas[0].text;
        userData.password = userDatas[1].text;

        var req = JsonConvert.SerializeObject(userData);
        Debug.Log(req);
        StartCoroutine(DataManager.sendDataToServer("/user/signin", req, (raw) =>
        {
            Debug.Log("sign in user data : \n" + req);
            Debug.Log("user's name : " + raw);

            if (raw.Equals("0")) // wrong id
            {
                Debug.Log("Fail Sign Up");
            }
            else if (raw.Equals("1")) // wrong pw
            {
                Debug.Log("Fail Sign Up");
            }
            else
            {
                Debug.Log("Sucessful Sign In!");
<<<<<<< Updated upstream
                userData.userName = raw;
                GameObject.Find("NickNameBtn").GetComponentInChildren<Text>().text = userData.userName;
                SetPageActive(2);
=======
                SetPageActive(2);
                // 닉네임 부분 띄워주기 userName은 따로 받지 않았기 때문에 1. playerData를 통해 받거나, 2. DB에서 받는다? -> playerData 활용
                //GameObject.Find("NickNameBtn").GetComponentInChildren<Text>().text = playerData.GetUserName();
>>>>>>> Stashed changes
            }

        }));
    }
<<<<<<< Updated upstream
 
=======

    public void OnClickEditBtn()
    {
        EditInfo();
    }

>>>>>>> Stashed changes
    private void EditInfo()
    {
        //정보를 받아서 바로 쏴주기 때문에 SignUp과 똑같지만 DB 질의문이 달라지는 구조라고 생각했습니다
        userData = new UserData();
        userData.userName = userDataField[0].text;
        userData.email = userDataField[1].text;
        userData.password = userDataField[2].text;

        var req = JsonConvert.SerializeObject(userData);
        Debug.Log(req);

        // Edit페이지 내 아이디, 닉네임 입력 창에 아이디와 닉네임 희미하게 띄워주기 위해
        GameObject.Find("IDField").GetComponentInChildren<Text>().text = userData.email;
        GameObject.Find("NickNameField").GetComponentInChildren<Text>().text = userData.userName;

        StartCoroutine(DataManager.sendDataToServer("/user/update", req, (raw) =>
        {
            Debug.Log("edit user data : \n" + req);
            Debug.Log("user's info : " + raw);

            userData.userName = raw;
            //userData.email = raw;
            //userData.password = raw;
            GameObject.Find("NickNameBtn").GetComponentInChildren<Text>().text = userData.userName;
            SetPageActive(2);
        }));
    }

}