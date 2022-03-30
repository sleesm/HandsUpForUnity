using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpeneingManager : MonoBehaviour
{
    public GameObject[] Pages = new GameObject[2];

    private UserData userData;
    public InputField[] userDataField;


    public void OnClickSignUpPageBtn()
    {
        SetPageActive(1);
    }

    public void OnClickSignInPageBtn()
    {
        SetPageActive(0);
    }

    private void SetPageActive(int index) // 0 : DefaultPage, 1 : SignUpPage
    {
        if(index == 1)
        {
            Pages[0].SetActive(false);
            Pages[1].SetActive(true);
        }
        else
        {
            Pages[1].SetActive(false);
            Pages[0].SetActive(true);
        }
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
}
