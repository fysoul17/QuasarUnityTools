////////////////////////////////////////////////////////////////////////////
// Swipe Input Detector
//
// Author: Terry Keetae Kwon
// 
// Copyright (C) 2015 Terry K. @ GIX Entertainment Inc. & Quasar Inc.
//
////////////////////////////////////////////////////////////////////////////

using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Quasar.Input
{
    public class SwipeInputDetector : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public static Action<InputDirection> InputDerectionCallback;
        public float clickThreshold = 50f;

        Vector2 startPoint;

        public void OnPointerDown(PointerEventData eventData)
        {
            startPoint = eventData.pointerCurrentRaycast.screenPosition;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Vector2 deltaPoint = eventData.pointerCurrentRaycast.screenPosition - startPoint;

            // Check if it should be recognised as Click.
            float distance = deltaPoint.magnitude;
            if (distance < clickThreshold) return;

            // Otherwise calculate drag angle.
            deltaPoint.Normalize();

            float angle = (Mathf.Atan2(deltaPoint.x, deltaPoint.y) * Mathf.Rad2Deg + 360f) % 360f;

            if (InputDerectionCallback != null)
            {
                angle += 45;
                angle %= 360;
                InputDerectionCallback((InputDirection)(angle / 90));
            }
        }
    }

    public enum InputDirection
    {
        Top,
        Right,
        Bottom,
        Left
    }
}

