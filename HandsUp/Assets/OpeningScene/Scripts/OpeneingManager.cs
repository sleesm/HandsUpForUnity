using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class OpeneingManager : MonoBehaviour
{
    private PlayerManager playerManager;

    private UserData userData;

    public InputField[] userDataField; // for sign in
    public InputField[] userCreateDataField; // for sign up
    public InputField[] userEditDataField; // for edit
    public InputField[] userDeleteDataField; // for edit


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
        InitPopUp();
        SetPageActive(1);
    }

    public void OnClickSignInPageBtn()
    {
        InitPopUp();
        SetPageActive(0);
    }

    public void OnClickMainPageBtn()
    {
        InitPopUp();
        SetPageActive(2);
    }

    public void OnClickWithdrawalPopUpBtn()
    {
        OpenPopUp(EventSystem.current.currentSelectedGameObject.name);
    }

    public void OnClickNicknameBtn()
    {
        InitPopUp();
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


    public void OnClickCardViewBtn()
    {
        SceneManager.LoadScene("CardViewScene");
    }

    /// <summary>
    /// Init All Pages
    /// </summary>
    /// <param name="index"></param>
    private void SetPageActive(int index) // 0 : DefaultPage, 1 : SignUpPage, 2 : MainPage, 3 : EditPage
    {
        for (int i = 0; i < GameObject.Find("Canvas").transform.childCount-1; i++)
        {
            GameObject.Find("Canvas").transform.GetChild(i).gameObject.SetActive(false);
        }
        GameObject.Find("Canvas").transform.GetChild(index).gameObject.SetActive(true);
    }

    private void InitInputFields(InputField[] tmp)
    {
        for (int i = 0; i < tmp.Length; i++)
            tmp[i].text = "";
    }

    private void InitPopUp()
    {
        for (int i = 0; i < GameObject.Find("PopUpPages").transform.childCount; i++)
        {
            GameObject.Find("PopUpPages").transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void OpenPopUp(string name)
    {
        string[] tmp = name.Split('B');
        Debug.Log(name);
        for (int i = 0; i < GameObject.Find("PopUpPages").transform.childCount; i++)
        {
            GameObject.Find("PopUpPages").transform.GetChild(i).gameObject.SetActive(false);
        }
        GameObject.Find("PopUpPages").transform.Find(tmp[0] + "PopUp").gameObject.SetActive(true);
    }


    /// <summary>
    /// Sign up event
    /// </summary>
    public void OnClickSignUpBtn()
    {
        if(!CheckValidFormat(userCreateDataField))
            return;
        SignUp();
    }

    //TO-DO : add alarms
    private void SignUp()
    {
        userData = new UserData();
        userData.email = userCreateDataField[0].text;
        userData.name = userCreateDataField[1].text; 
        userData.password = userCreateDataField[2].text; 

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
                InitInputFields(userCreateDataField);
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
        if (field.Length > 3) { // edit & singup case
            if (!field[3].text.Equals(field[2].text))
            {
                Debug.Log("PW is diffrent with Confirm PW.");
                return false;
            }
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
        userData.email = userDataField[0].text;
        userData.password = userDataField[1].text;

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
                
                playerManager.SetUserId((int)res["user_id"]);
                playerManager.SetUserName(res["user_name"].ToString());
                playerManager.SetUserEmail(userData.email);
                InitInputFields(userDataField);
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
                InitInputFields(userEditDataField);
                SetPageActive(2);
            }
        }));
    }


    /// <summary>
    /// Withdrawal event
    /// </summary>
    public void OnClickWithdrawalBtn()
    {
        if (!CheckValidFormat(userDeleteDataField))
            return;
        DeleteUser();
    }

    private void DeleteUser()
    {
        //정보를 받아서 바로 쏴주기 때문에 SignUp과 똑같지만 DB 질의문이 달라지는 구조라고 생각했습니다
        userData = new UserData();
        userData.email = userDeleteDataField[0].text;
        userData.password = userDeleteDataField[1].text;

        var req = JsonConvert.SerializeObject(userData);
        Debug.Log(req);

        StartCoroutine(DataManager.sendDataToServer("auth/user/delete", req, (raw) =>
        {
            Debug.Log("delete user data : \n" + req);
            JObject res = JObject.Parse(raw);

            if (res["result"].ToString().Equals("fail")) // wrong id
            {
                Debug.Log("Fail Deleting User Info!");
            }
            else if (res["result"].ToString().Equals("success")) // wrong pw
            {
                Debug.Log("Sucessful Deleting User Info!");

                playerManager.InitPlayerData();
                InitInputFields(userDeleteDataField);
                InitPopUp();
                SetPageActive(2);
            }
        }));
    }
}