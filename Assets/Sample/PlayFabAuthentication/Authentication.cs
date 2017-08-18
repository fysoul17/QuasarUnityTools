using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PlayFab.ClientModels;

using Quasar.PlayFab.Authentication;

public class Authentication : MonoBehaviour
{
    public bool autoLogin = true;

    Dictionary<LoginType, Action> LoginPathways;

    GetPlayerCombinedInfoRequestParams infoRequestParams;

    private void Awake()
    {
        // We need 'Player Profile' and 'User data' on login.
        infoRequestParams = new GetPlayerCombinedInfoRequestParams()
        {
            GetPlayerProfile = true,
            GetUserReadOnlyData = true,
            GetUserVirtualCurrency = true,
            //GetPlayerStatistics = true
        };

        InitialiseLoginPathways();

        if (autoLogin)
        {
            LoginType currentLoginPathway = PlayFabAuthenticationBase.GetLoginPathway();

            LoginPathways[currentLoginPathway].Invoke();
        }
    }

    void InitialiseLoginPathways()
    {
        LoginPathways = new Dictionary<LoginType, Action>()
        {
            { LoginType.PlayFab, LoginWithMobileSpecific },
            { LoginType.MobileSpecific, LoginWithMobileSpecific },
            { LoginType.GameCenter, LoginWithGameCenterAccount},
            { LoginType.Google, LoginWithGoogleAccount },
            { LoginType.Facebook, LoginWithFacebook }
        };
    }

    public void LoginWithMobileSpecific()
    {
#if UNITY_EDITOR
        (new PlayFabAuthentication("PlayFabTestAccount", "Test1234")).Login(OnLoginSuccess, OnLoginFailed, infoRequestParams);
#elif UNITY_STANDALONE
        (new PlayFabAuthentication("PlayFabTestAccount" + UnityEngine.Random.Range(0, 10), "Test1234")).Login(OnLoginSuccess, OnLoginFailed, infoRequestParams);
#else
        (new MobilePlayFabAuthentication()).Login(OnLoginSuccess, OnLoginFailed, infoRequestParams);
#endif
    }

    public void LoginWithFacebook()
    {
        new FacebookAuthentication().Login((callback) =>
        {
            OnLoginSuccess(callback);

        }, OnLoginFailed, infoRequestParams);
    }

    public void LoginWithGameCenterAccount()
    {
        new GameCenterAuthentication().Login(OnLoginSuccess, OnLoginFailed, infoRequestParams);
    }

    public void LoginWithGoogleAccount()
    {
        new GooglePlayAuthentication().Login(OnLoginSuccess, OnLoginFailed, infoRequestParams);
    }

    #region Login result handlers
    void OnLoginSuccess(object callbackData)
    {
        LoginResult result = callbackData as LoginResult;

        Debug.Log(string.Format("PlayFab Authentication Successful! -- Player ID:{0}, SessionTicket: {1}", result.PlayFabId, result.SessionTicket));

        // result.PlayFabId;
        // result.InfoResultPayload.PlayerProfile;
        // result.InfoResultPayload.UserReadOnlyData;
        // result.InfoResultPayload.UserVirtualCurrency;
        // result.InfoResultPayload.PlayerStatistics;
        // etc..

        // Start loading data from PlayFab Server if exist any.
        StartCoroutine("PerformLoading");
    }

    void OnLoginFailed(LoginFailCode failCode)
    {
        if (failCode == LoginFailCode.InvalidUsernameOrPassword)
        {
            // TODO. 아이디 혹은 비밀번호가 올바르지 않는다는 팝업.
            Debug.Log("아이디 혹은 비번이 안맞음.");
        }
        else
        {
            // TODO. 기타 다른 문제 알리는 팝업.
            Debug.Log("알 수 없는 오류");
        }
    }
    #endregion Login result handlers END

    IEnumerator PerformLoading()
    {
        Debug.Log("Start loading data from server");

        yield return null;
    }
}
