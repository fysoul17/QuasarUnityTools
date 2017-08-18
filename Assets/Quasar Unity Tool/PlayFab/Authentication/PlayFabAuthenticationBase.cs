////////////////////////////////////////////////////////////////////////////
// PlayFab Authentication Helper - Base Abstract class
//
// DEPENDENCY: PlayFab SDK (https://api.playfab.com/sdks/unity)
//
// Author: Terry Keetae Kwon
// 
// Copyright (C) 2015 Terry K. @ GIX Entertainment Inc. & Quasar Inc.
//
////////////////////////////////////////////////////////////////////////////

using System;
using UnityEngine;

using PlayFab.ClientModels;

namespace Quasar.PlayFab.Authentication
{
    public abstract class PlayFabAuthenticationBase
    {
        LoginType loginType;

        public static void SaveLoginPathway(LoginType loginType)
        {
            PlayerPrefs.SetInt("LoginType", (int)loginType);
        }

        public static LoginType GetLoginPathway()
        {
            return (LoginType)PlayerPrefs.GetInt("LoginType", 0);
        }

        public static void SetSocialPlayformID(string id)
        {
            PlayerPrefs.SetString("SocialPlayformID", id);
        }

        public static string GetSocialPlatformID()
        {
            return PlayerPrefs.GetString("SocialPlayformID", "");
        }

        public abstract void Login(Action<object> LoginSuccessCallback, Action<LoginFailCode> LoginFailedCallback, GetPlayerCombinedInfoRequestParams infoRequestParams = null);
    }

    public enum LoginFailCode
    {
        AccountNotFound,                    // 계정이 존재하지 않음.
        AccountBanned,                      // 계정이 밴 당함.
        InvalidUsernameOrPassword,          // 아이디 혹은 패스워드가 틀림.

        SocialPlatformLoginFailed,          // 구글이나 게임센터 로그인 실패.

        FBInitFailed,                       // 페북 Init 실패.
        FBLoginFailed,                      // 페북 로그인 실패.

        UnknownError                        // 알수 없는 오류.
    }

    public enum LoginType
    {
        PlayFab = 0,
        MobileSpecific,
        Facebook,
        Google,
        GameCenter,

        COUNT
    }
}