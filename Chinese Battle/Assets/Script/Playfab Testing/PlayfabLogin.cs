using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabLogin : MonoBehaviour
{
    string titleID = "C4562"; //App Playfab ID
    string userEmail;
    string userPassWord;
    string userName;
    string PlayerPrefsEmail = "EMAIL";
    string PlayerPrefsPassWord = "PASSWORD";
    [SerializeField]private GameObject loginPanel;
    [SerializeField]private GameObject addLoginPanel;
    [SerializeField] private GameObject recoverButton;

    public void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            /*
            Please change the titleId below to your own titleId from PlayFab Game Manager.
            If you have already set the value in the Editor Extensions, this can be skipped.
            */
            PlayFabSettings.staticSettings.TitleId = titleID;
        }
        if(PlayerPrefs.HasKey(PlayerPrefsEmail) && PlayerPrefs.HasKey(PlayerPrefsPassWord))
        {
            GetLoginData();
            var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassWord };
            PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
        }
        else
        {
#if UNITY_IOS
            var requestIOS = new LoginWithIOSDeviceIDRequest { DeviceId = ReturnMobileID(), CreateAccount = true };
            PlayFabClientAPI.LoginWithIOSDeviceID(requestIOS, OnLoginMobileSuccess, OnLoginMobileFailure);
#endif
        }
        //var request = new LoginWithCustomIDRequest { CustomId = "GettingStartedGuide", CreateAccount = true };
        //PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginMobileSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        RecordLoginData();
        loginPanel.SetActive(false);
        recoverButton.SetActive(false);
    }

    private void OnLoginMobileFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
        Register();
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        RecordLoginData();
        loginPanel.SetActive(false);
    }

    private void OnRegisterFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        RecordLoginData();
        loginPanel.SetActive(false);
        recoverButton.SetActive(false);
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
        Register();
    }

    private void RecordLoginData()
    {
        PlayerPrefs.SetString(PlayerPrefsEmail, userEmail);
        PlayerPrefs.SetString(PlayerPrefsPassWord, userPassWord);
    }

    private void GetLoginData()
    {
        userEmail = PlayerPrefs.GetString(PlayerPrefsEmail);
        userPassWord = PlayerPrefs.GetString(PlayerPrefsPassWord);
    }

    public void GetUserEmail(string emailIn)
    {
        userEmail = emailIn;
    }

    public void GetUserPassword(string passwordIn)
    {
        userPassWord = passwordIn;
    }

    public void GetUserName(string nameIn)
    {
        userName = nameIn;
    }

    //Login by the Email and Password, If fail to login, which means the user didn't register yet, so go to LoginFailure
    public void Login()
    {
        var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassWord };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }

    public void Register()
    {
        var registerRequest = new RegisterPlayFabUserRequest { Email = userEmail, Password = userPassWord, Username = userName };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSuccess, OnRegisterFailure);
    }

    public static string ReturnMobileID()
    {
        string deviceID = SystemInfo.deviceUniqueIdentifier;
        return deviceID;
    }

    public void OpenAddLogin()
    {
        addLoginPanel.SetActive(true);
    }

    public void AddLogin()
    {
        var addLoginRequest = new AddUsernamePasswordRequest { Email = userEmail, Password = userPassWord, Username = userName };
        PlayFabClientAPI.AddUsernamePassword(addLoginRequest, OnAddLoginSuccess, OnAddLoginFailure);
    }

    private void OnAddLoginSuccess(AddUsernamePasswordResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        RecordLoginData();
        loginPanel.SetActive(false);
    }

    private void OnAddLoginFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }
}
