////////////////////////////////////////////////////////////////////////////
// PlayFab Authentication Helper - Google Play (Android)
//
// DEPENDENCY: PlayFab SDK (https://api.playfab.com/sdks/unity)
// DEPENDENCY: Google Play Games SDK (https://github.com/playgameservices/play-games-plugin-for-unity)
//
// Author: Terry Keetae Kwon
// 
// Copyright (C) 2015 Terry K. @ GIX Entertainment Inc. & Quasar Inc.
//
////////////////////////////////////////////////////////////////////////////

using System;
using UnityEngine;

using PlayFab;
using PlayFab.ClientModels;

#if (UNITY_ANDROID || (UNITY_IPHONE && !NO_GPGS))
using GooglePlayGames;
#endif

namespace Quasar.PlayFab.Authentication
{
    public class GooglePlayAuthentication : PlayFabAuthenticationBase
    {
        public override void Login(Action<object> LoginSuccessCallback, Action<LoginFailCode> LoginFailedCallback, GetPlayerCombinedInfoRequestParams infoRequestParams = null)
        {
            string socialPlatformID = GetSocialPlatformID();

            if (string.IsNullOrEmpty(socialPlatformID))
            {
                Debug.LogWarning("Not logged into Google. Logging in..");

#if (UNITY_ANDROID || (UNITY_IPHONE && !NO_GPGS))
                PlayGamesPlatform.DebugLogEnabled = true;

                PlayGamesPlatform.Activate();

                Social.localUser.Authenticate((bool success) =>
                {
                    if (success)
                    {
                        SetSocialPlayformID(Social.localUser.id);

                        LoginWithGooglePlayAccount(socialPlatformID, LoginSuccessCallback, LoginFailedCallback, infoRequestParams);
                    }
                    else
                    {
                        LoginFailedCallback(LoginFailCode.SocialPlatformLoginFailed);
                    }
                });
#else
                Debug.LogWarning("Only Android platfor or Ios with !NO_GPGS is allowed");
#endif
            }
            else
            {
                LoginWithGooglePlayAccount(socialPlatformID, LoginSuccessCallback, LoginFailedCallback, infoRequestParams);
            }
        }

        void LoginWithGooglePlayAccount(string id, Action<object> LoginSuccessCallback, Action<LoginFailCode> LoginFailedCallback, GetPlayerCombinedInfoRequestParams infoRequestParams)
        {
            // NOTE: LoginWithGoogleAccount needs google credential (Auth2.0). PlayGamesPlatform login does not give that, so use google id as a custon id.
            PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest
            {
                CustomId = id,
                CreateAccount = false,
                InfoRequestParameters = infoRequestParams
            },
            (result) =>
            {
                SaveLoginPathway(LoginType.Google);

                LoginSuccessCallback(result);
            },
            (error) =>
            {
                Debug.Log(string.Format("GooglePlay Auth Error {0}: {1}, {2}", error.HttpCode, error.Error.ToString(), error.ErrorMessage));

                if (error.Error == PlayFabErrorCode.AccountNotFound)
                {
                    LoginFailedCallback(LoginFailCode.AccountNotFound);
                }
                else if (error.Error == PlayFabErrorCode.AccountBanned)
                {
                    LoginFailedCallback(LoginFailCode.AccountBanned);
                }
                else
                {
                    LoginFailedCallback(LoginFailCode.UnknownError);
                }
            });
        }

        public void LinkGoogle(string id, Action<bool> ResultCallback)
        {
            LinkCustomIDRequest request = new LinkCustomIDRequest();
            request.CustomId = id;
            request.ForceLink = false;

            PlayFabClientAPI.LinkCustomID(request, (result) =>
            {
                SaveLoginPathway(LoginType.Google);

                SetSocialPlayformID(id);

                ResultCallback(true);
            },
            (error) =>
            {
                ResultCallback(false);
            });
        }
    }
}