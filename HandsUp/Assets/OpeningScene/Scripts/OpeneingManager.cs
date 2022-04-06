using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpeneingManager : MonoBehaviour
{
    public GameObject[] Pages = new GameObject[4];

    private UserData userData;

    public InputField[] userDatas; // for sign in
    public InputField[] userDataField; // for sign up

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
        SetPageActive(2);
        GameObject.Find("NickNameBtn").GetComponentInChildren<Text>().text = "로그인";
    }

    public void OnClickNicknamePageBtn()
    {
        SetPageActive(3);
    }

    private void SetPageActive(int index) // 0 : DefaultPage, 1 : SignUpPage 2 : MainPage 3 : EditPage
    {
        for (int i = 0; i < Pages.Length; i++)
        {
            Pages[i].SetActive(false);
        }
        Pages[index].SetActive(true);
    }

    //TO-DO : user email form checking
    public void OnClickSignUpBtn()
    {

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
                userData.userName = raw;
                GameObject.Find("NickNameBtn").GetComponentInChildren<Text>().text = userData.userName;
                SetPageActive(2);
            }

        }));
    }
 
    private void EditInfo()
    {
        userData = new UserData();
        userData.userName = userDataField[0].text;
        userData.email = userDataField[1].text;
        userData.password = userDataField[2].text;

        var req = JsonConvert.SerializeObject(userData);
        Debug.Log(req);

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