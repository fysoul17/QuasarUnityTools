////////////////////////////////////////////////////////////////////////////
// PlayFab Timer
//
// Author: Terry Keetae Kwon
// 
// Copyright (C) 2015 Terry K. @ GIX Entertainment Inc. & Quasar Inc.
//
////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System;

using PlayFab;
using PlayFab.ClientModels;

using Newtonsoft.Json;

public class PlayFabTimer : MonoBehaviour
{
    [SerializeField] PlayFabManager manager;
    [SerializeField] ServerTimeSynchroniser timeSynchroniser;

    void Awake()
    {
        manager.SchedulePreloading(OnReadyToLoad);
    }

    void OnReadyToLoad()
    {
        ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest()
        {
            FunctionName = "GetServerTime",
            GeneratePlayStreamEvent = true, // Optional - Shows this event in PlayStream
        };

        timeSynchroniser.StartSyncServerTime();

        PlayFabClientAPI.ExecuteCloudScript(request, DidReceive, manager.HandlePlayFabError);
    }

    void DidReceive(ExecuteCloudScriptResult result)
    {
#if UNITY_EDITOR
        foreach (LogStatement log in result.Logs)
        {
            Debug.Log("SERVERTIME LOG: " + log.Message);
        }

        Debug.Log("ServerTime: " + result.FunctionResult.ToString());
#endif

        DateTime serverTime = JsonConvert.DeserializeObject<DateTime>(result.FunctionResult.ToString());

        timeSynchroniser.ReceviedServerTime(serverTime);

        manager.FinishedPreloading();
    }
}