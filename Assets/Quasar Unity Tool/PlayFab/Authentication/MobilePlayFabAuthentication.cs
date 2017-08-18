////////////////////////////////////////////////////////////////////////////
// PlayFab Authentication Helper - Mobile Specific (iOS/Android device unique id)
//
// DEPENDENCY: PlayFab SDK (https://api.playfab.com/sdks/unity)
//
// Author: Terry Keetae Kwon
// 
// Copyright (C) 2015 Terry K. @ GIX Entertainment Inc. & Quasar Inc.
//
////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PlayFab;
using PlayFab.ClientModels;

namespace Quasar.PlayFab.Authentication
{
    public class MobilePlayFabAuthentication : PlayFabAuthenticationBase
    {
        Action<LoginFailCode> FailedCallback { get; set; }

        public override void Login(Action<object> LoginSuccessCallback, Action<LoginFailCode> LoginFailedCallback, GetPlayerCombinedInfoRequestParams infoRequestParams = null)
        {
            FailedCallback = LoginFailedCallback;

#if UNITY_ANDROID
        PlayFabClientAPI.LoginWithAndroidDeviceID(new LoginWithAndroidDeviceIDRequest
        {
            OS = SystemInfo.operatingSystem,
            AndroidDevice = SystemInfo.deviceModel,
            AndroidDeviceId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,

            InfoRequestParameters = infoRequestParams

        }, (result) =>
        {
            SaveLoginPathway(LoginType.MobileSpecific);

            LoginSuccessCallback(result);

        }, ErrorCallback);

#elif UNITY_IPHONE
        PlayFabClientAPI.LoginWithIOSDeviceID(new LoginWithIOSDeviceIDRequest
        {
            DeviceId = UnityEngine.iOS.Device.vendorIdentifier,
            OS = SystemInfo.operatingSystem,
            DeviceModel = SystemInfo.deviceModel,
            CreateAccount = true,

            InfoRequestParameters = infoRequestParams

        }, (result) =>
        {
            SaveLoginPathway(LoginType.MobileSpecific);

            LoginSuccessCallback(result);

        }, ErrorCallback);
#endif
        }

        void ErrorCallback(PlayFabError error)
        {
            Debug.Log(string.Format("Mobile Auth Error {0}: {1}, {2}", error.HttpCode, error.Error.ToString(), error.ErrorMessage));

            if (error.Error == PlayFabErrorCode.AccountNotFound)
            {
                FailedCallback(LoginFailCode.AccountNotFound);
            }
            else if (error.Error == PlayFabErrorCode.AccountBanned)
            {
                FailedCallback(LoginFailCode.AccountBanned);
            }
            else
            {
                FailedCallback(LoginFailCode.UnknownError);
            }
        }
    }
}