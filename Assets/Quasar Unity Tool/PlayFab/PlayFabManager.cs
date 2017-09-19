////////////////////////////////////////////////////////////////////////////
// PlayFab Manager
//
// Author: Terry Keetae Kwon
// 
// Copyright (C) 2015 Terry K. @ GIX Entertainment Inc. & Quasar Inc.
//
////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using UnityEngine;

using PlayFab;

public class PlayFabManager : MonoBehaviour
{
    /** Events for fetching data from PlayFab */
    event Action OnPreloadReady;
    event Action OnLoadReady;

    /** Preloading and Loading counts */
    int numberOfPreLoading = 0;
    int numberOfLoading = 0;

    int numberOfPreLoaded = 0;
    int numberOfLoaded = 0;

    /** Fires event on every step's done */
    Action EachStepNotification;

    private void OnDestroy()
    {
        OnPreloadReady = null;
        OnLoadReady = null;
    }

    public void SchedulePreloading(Action OnReadyCallback)
    {
        OnPreloadReady += OnReadyCallback;
        numberOfPreLoading++;
    }

    public void FinishedPreloading()
    {
        numberOfPreLoaded++;

        if (EachStepNotification != null)
        {
            EachStepNotification();
        }
    }

    public void ScheduleLoading(Action OnReadyCallback)
    {
        OnLoadReady += OnReadyCallback;
        numberOfLoading++;
    }

    public void FinishedLoading()
    {
        numberOfLoaded++;

        if (EachStepNotification != null)
        {
            EachStepNotification();
        }
    }

    public IEnumerator FetchDataFromServer(Action NotifyEachStep, Action LoadingDone)
    {
        EachStepNotification = NotifyEachStep;

#if UNITY_EDITOR
        Debug.Log(">>> Start loading from PlayFab...");
        Debug.Log(">> numberOfPreLoading: " + numberOfPreLoading);
        Debug.Log(">> numberOfLoading: " + numberOfLoading);
#endif

        if (OnPreloadReady != null)
        {
            OnPreloadReady();
        }

        while(numberOfPreLoading > numberOfPreLoaded)
        {
            yield return null;
        }

        if (OnLoadReady != null)
        {
            OnLoadReady();
        }

        while(numberOfLoading > numberOfLoaded)
        {
            yield return null;
        }

#if UNITY_EDITOR
        Debug.Log(">>> Loading Done.");
#endif

        numberOfPreLoading = numberOfPreLoaded = 0;
        numberOfLoading = numberOfLoaded = 0;

        LoadingDone();
    }

    public void HandlePlayFabError(PlayFabError error)
    {
        Debug.Log(error.ErrorMessage);
        Debug.Log(error.ErrorDetails);

        HandlePlayFabError(error, false);
    }

    public void HandlePlayFabError(PlayFabError error, bool useErrorMsg)
    {
    }
}
