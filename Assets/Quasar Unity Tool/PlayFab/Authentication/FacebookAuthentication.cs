////////////////////////////////////////////////////////////////////////////
// PlayFab Authentication Helper - Facebook
//
// DEPENDENCY: PlayFab SDK (https://api.playfab.com/sdks/unity)
// DEPENDENCY: Facebook SDKs (https://developers.facebook.com/docs/unity/)
//
// Author: Terry Keetae Kwon
// 
// Copyright (C) 2015 Terry K. @ GIX Entertainment Inc. & Quasar Inc.
//
////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using UnityEngine;

using PlayFab;
using PlayFab.ClientModels;

using Facebook.Unity;

namespace Quasar.PlayFab.Authentication
{
    public class FacebookAuthentication : PlayFabAuthenticationBase
    {
        Action<object> SuccessCallback { get; set; }
        Action<LoginFailCode> FailedCallback { get; set; }
        GetPlayerCombinedInfoRequestParams infoParams;

        public override void Login(Action<object> LoginSuccessCallback, Action<LoginFailCode> LoginFailedCallback, GetPlayerCombinedInfoRequestParams infoRequestParams = null)
        {
            SuccessCallback = LoginSuccessCallback;
            FailedCallback = LoginFailedCallback;
            infoParams = infoRequestParams;

            if (!FB.IsInitialized)
            {
                // Initialize the Facebook SDK
                FB.Init(InitCallback, OnHideUnity);
            }
            else
            {
                // Already initialized, signal an app activation App Event
                Login();
            }
        }

        void Login()
        {
            // Signal an app activation App Event
            FB.ActivateApp();

            // Continue with Facebook SDK
            var perms = new List<string>() { "public_profile", "email", "user_friends" };
            FB.LogInWithReadPermissions(perms, AuthCallback);
        }

        private void InitCallback()
        {
            if (FB.IsInitialized)
            {
                Login();
            }
            else
            {
                FailedCallback(LoginFailCode.FBInitFailed);
            }
        }

        private void OnHideUnity(bool isGameShown)
        {
            if (!isGameShown)
            {
                // Pause the game - we will need to hide
                Time.timeScale = 0;
            }
            else
            {
                // Resume the game - we're getting focus again
                Time.timeScale = 1;
            }
        }

        private void AuthCallback(ILoginResult result)
        {
            if (FB.IsLoggedIn)
            {
                // AccessToken class will have session details
                var aToken = AccessToken.CurrentAccessToken;

                // Print current access token's User ID
                Debug.Log(aToken.UserId);

                // Print current access token's granted permissions
                foreach (string perm in aToken.Permissions)
                {
                    Debug.Log(perm);
                }

                PlayFabClientAPI.LoginWithFacebook(new LoginWithFacebookRequest()
                {
                    AccessToken = aToken.TokenString,
                    CreateAccount = true,
                    InfoRequestParameters = infoParams
                },
                (playFabResult) =>
                {
                    SaveLoginPathway(LoginType.MobileSpecific);

                    SuccessCallback(playFabResult);
                },
                ErrorCallback);
            }
            else
            {
                FailedCallback(LoginFailCode.FBLoginFailed);
            }
        }

        void ErrorCallback(PlayFabError error)
        {
            Debug.Log(string.Format("Facebook Auth Error {0}: {1}, {2}", error.HttpCode, error.Error.ToString(), error.ErrorMessage));

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