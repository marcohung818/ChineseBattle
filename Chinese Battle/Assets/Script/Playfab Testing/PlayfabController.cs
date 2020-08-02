using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabController : MonoBehaviour
{
    public static PlayfabController instance;

    string titleID = "C4562"; //App Playfab ID
    string userEmail;
    string userPassWord;
    string userName;
    string PlayerPrefsEmail = "EMAIL";
    string PlayerPrefsPassWord = "PASSWORD";
    [SerializeField]private GameObject loginPanel;
    [SerializeField]private GameObject addLoginPanel;
    [SerializeField] private GameObject recoverButton;

    private void Awake()
    {
        instance = this;
    }

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
#if UNITY_ANDROID
            var requestAndroid = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = ReturnMobileID(), CreateAccount = true };
            PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, OnLoginMobileSuccess, OnLoginMobileFailure);
#endif

#if UNITY_IOS
            var requestIOS = new LoginWithIOSDeviceIDRequest { DeviceId = ReturnMobileID(), CreateAccount = true };
            PlayFabClientAPI.LoginWithIOSDeviceID(requestIOS, OnLoginMobileSuccess, OnLoginMobileFailure);
#endif
        }
    }
    #region Login
    private void OnLoginMobileSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        RecordLoginData();
        loginPanel.SetActive(false);
        recoverButton.SetActive(true);
        GetStats();
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
        GetStats();
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
        GetStats();
    }

    private void OnAddLoginFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }
    #endregion Login

    #region PlayerStats
    public int playerLevel;
    public int playerGemAmount;
    public void SetStats()
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
            {Statistics = new List<StatisticUpdate> {
                new StatisticUpdate { StatisticName = "PlayerLevel", Value = playerLevel }, 
                new StatisticUpdate { StatisticName = "PlayerGemAmount", Value = playerGemAmount }
                } 
            },
            result => { Debug.Log("User statistics updated"); },
            error => { Debug.LogError(error.GenerateErrorReport()); }
        );
    }

    public void GetStats()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            OnGetStats,
            error => Debug.LogError(error.GenerateErrorReport())
        );
    }

    public void OnGetStats(GetPlayerStatisticsResult result)
    {
        Debug.Log("Received the following Statistics:");
        foreach (var eachStat in result.Statistics)
        {
            Debug.Log("Statistic (" + eachStat.StatisticName + "): " + eachStat.Value);
            switch (eachStat.StatisticName)
            {
                case "PlayerLevel":
                    playerLevel = eachStat.Value;
                    break;
                case "PlayerGemAmount":
                    playerGemAmount = eachStat.Value;
                    break;
            }
        }
    }

    #endregion PlayerStats
}
