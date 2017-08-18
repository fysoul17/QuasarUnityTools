////////////////////////////////////////////////////////////////////////////
// PlayFab Authentication Helper - PlayFab Custom
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

using PlayFab;
using PlayFab.ClientModels;

namespace Quasar.PlayFab.Authentication
{
    public class PlayFabAuthentication : PlayFabAuthenticationBase
    {
        string loginUserName;
        string loginPassword;

        Action<LoginFailCode> FailedCallback { get; set; }

        /// <summary>
        /// PlayFab account user needs to input id, pw manually.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public PlayFabAuthentication(string username, string password)
        {
            loginUserName = username;
            loginPassword = password;
        }

        public override void Login(Action<object> LoginSuccessCallback, Action<LoginFailCode> LoginFailedCallback, GetPlayerCombinedInfoRequestParams infoRequestParams = null)
        {
            FailedCallback = LoginFailedCallback;

            PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
            {
                Username = loginUserName,
                Password = loginPassword,
                InfoRequestParameters = infoRequestParams

            }, (result) =>
            {
                SaveLoginPathway(LoginType.PlayFab);

                LoginSuccessCallback(result);

            }, (error) =>
            {
                Debug.Log(string.Format("PlayFabAuthentication Error {0}: {1}, {2}", error.HttpCode, error.Error.ToString(), error.ErrorMessage));

            // Create new account if this is a new user.
            if (error.Error == PlayFabErrorCode.AccountNotFound)
                {
                    PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
                    {
                        Username = loginUserName,
                        Password = loginPassword,
                        Email = loginUserName + "@test.com"

                    }, (registerResult) =>
                    {
                        Debug.Log(string.Format("PlayFab registeration Successful! -- Player ID:{0}", registerResult.PlayFabId));

                        Login(LoginSuccessCallback, LoginFailedCallback, infoRequestParams);

                    }, (registerFail) =>
                    {
                        Debug.Log(string.Format("PlayFab registeration Failed! -- {0}", registerFail.ErrorMessage));

                        FailedCallback(LoginFailCode.UnknownError);
                    });
                }
            // Otherwise, send error to callback.
            else
                {
                    if (error.Error == PlayFabErrorCode.InvalidEmailOrPassword || error.Error == PlayFabErrorCode.InvalidEmailAddress)
                    {
                        FailedCallback(LoginFailCode.InvalidUsernameOrPassword);
                    }
                    else
                    {
                        FailedCallback(LoginFailCode.UnknownError);
                    }
                }
            });
        }
    }
}