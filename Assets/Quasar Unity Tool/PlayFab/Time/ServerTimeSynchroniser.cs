////////////////////////////////////////////////////////////////////////////
// ServerTimeSynchroniser
//
// Author: Terry Keetae Kwon
// 
// Copyright (C) 2015 Terry K. @ GIX Entertainment Inc. & Quasar Inc.
//
////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System;
using System.Collections;

public class ServerTimeSynchroniser : MonoBehaviour
{
    DateTime startTime;
    DateTime fetchedTime;
    float timeSinceStartUp;

    public static DateTime ServerTime
    {
        get;
        protected set;
    }

    /// <summary>
    /// In order to calculate approximate ping (network latency), call this just before requesting time to server.
    /// 대략적인 핑(서버지연)을 계산하기 위하여 서버시간을 요청하기 직전에 호출하여 준다.
    /// </summary>
    public void StartSyncServerTime()
    {
        StopCoroutine("UpdateElapsedTimeSinceServerTimeUpdated");

        startTime = DateTime.UtcNow;
    }

    /// <summary>
    /// Call this when received server time. After then, we can simply use ServerTime property which is almost the same as current server time.
    /// 서버 시간을 받아오면 이 함수를 호출하여준다. 그러면 현재 서버 시간과 거의 동일한 ServerTime 프로퍼티를 사용할 수 있다.
    /// </summary>
    /// <param name="fetchedServerTime"></param>
    public void ReceviedServerTime(DateTime fetchedServerTime)
    {
        double networkTime = (DateTime.UtcNow - startTime).TotalSeconds;

        fetchedTime = fetchedServerTime.AddSeconds(networkTime / 2);

        timeSinceStartUp = Time.realtimeSinceStartup;

        StartCoroutine("UpdateElapsedTimeSinceServerTimeUpdated");
    }

    IEnumerator UpdateElapsedTimeSinceServerTimeUpdated()
    {
        while (true)
        {
            ServerTime = fetchedTime.AddSeconds(Time.realtimeSinceStartup - timeSinceStartUp);

            yield return null;
        }
    }
}
