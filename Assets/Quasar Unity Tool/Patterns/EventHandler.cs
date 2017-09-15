////////////////////////////////////////////////////////////////////////////
//
// Event handler - Applies 'Observer Pattern'.
//
// Copyright (C) 2015 Terry K. @ GIX Entertainment Inc. & Quasar Inc.
//
////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

namespace Quasar.Patterns
{
    // IMPORTANT: You MUST remove event in order to avoid calling event after the observer (event receiver) is destroyed. 
    //            (It will be invoked event after the scene changes because it is static method)
    //            Alternatively, if all previous events are not going to be used on new Scene, simply call ClearObservers() method.
    //            옵저버(이벤트 받는놈)가 Destroy 되었을 시에도 이벤트가 발생하는 것을 방지 하기 위하여 (static 함수들이기 때문에 Scene 이 바껴도 불림),
    //            반드시 OnDestroy, 혹은 필요시마다 RemoveEvent 로 해제해준다. (혹은 씬이 바뀔 때 모든 이벤트가 필요 없다면 Clear 함수를 이용한다.).

    public class EventHandler
    {
        static Dictionary<string, Action<object[]>> eventCallbacks = new Dictionary<string, Action<object[]>>();

        //public static void AddObserver(string eventName, Action<object[]> callback)
        public static void AddObserver(string eventName, Action<object[]> callback)
        {
            Action<object[]> storedCallback;
            eventCallbacks.TryGetValue(eventName, out storedCallback);

            storedCallback += callback;

            eventCallbacks[eventName] = storedCallback;
        }

        public static void RemoveObserver(string eventName, Action<object[]> callback)
        {
            Action<object[]> storedCallback;
            if (eventCallbacks.TryGetValue(eventName, out storedCallback))
            {
                storedCallback -= callback;

                eventCallbacks[eventName] = storedCallback;
            }
        }

        public static void ClearObservers()
        {
            eventCallbacks.Clear();
        }

        public static void Notify(string eventName, params object[] args)
        {
            Action<object[]> storedCallback;
            if (eventCallbacks.TryGetValue(eventName, out storedCallback))
            {
                if (storedCallback != null)
                {
                    storedCallback(args);
                }
            }
        }
    }
}