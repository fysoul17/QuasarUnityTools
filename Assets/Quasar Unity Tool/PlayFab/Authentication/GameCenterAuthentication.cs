////////////////////////////////////////////////////////////////////////////
// PlayFab Authentication Helper - Game Center (iOS)
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

#if UNITY_IOS
using UnityEngine.SocialPlatforms.GameCenter;
#endif

using PlayFab;
using PlayFab.ClientModels;

namespace Quasar.PlayFab.Authentication
{
    public class GameCenterAuthentication : PlayFabAuthenticationBase
    {
        public override void Login(Action<object> LoginSuccessCallback, Action<LoginFailCode> LoginFailedCallback, GetPlayerCombinedInfoRequestParams infoRequestParams = null)
        {
#if UNITY_IOS
        string socialPlatformID = GetSocialPlatformID();

        if (string.IsNullOrEmpty(socialPlatformID))
        {
            Debug.LogWarning("Not logged into GameCenter. Logging in..");

            Social.Active = new GameCenterPlatform();

            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    SetSocialPlayformID(Social.localUser.id);

                    LoginWithGameCenterAccount(socialPlatformID, LoginSuccessCallback, LoginFailedCallback, infoRequestParams);
                }
                else
                {
                    LoginFailedCallback(LoginFailCode.SocialPlatformLoginFailed);
                }
            });
        }
        else
        {
            LoginWithGameCenterAccount(socialPlatformID, LoginSuccessCallback, LoginFailedCallback, infoRequestParams);
        }
#endif
        }

        void LoginWithGameCenterAccount(string id, Action<object> LoginSuccessCallback, Action<LoginFailCode> LoginFailedCallback, GetPlayerCombinedInfoRequestParams infoRequestParams)
        {
#if UNITY_IOS
        LinkGameCenterAccountRequest request = new LinkGameCenterAccountRequest();
        request.GameCenterId = gameCenterId;
        request.ForceLink = false;

        PlayFabClientAPI.LinkGameCenterAccount(request, (result) =>
        {
            SaveLoginPathway(LoginType.Google);

            SetSocialPlayformID(gameCenterId);

            ResultCallback(true);
        },
        (error) =>
        {
            ResultCallback(false);
        });
#endif
        }
    }
}