////////////////////////////////////////////////////////////////////////////
// Advanced Joystick
//
// Author: Terry Keetae Kwon
// 
// Copyright (C) 2015 Terry K. @ GIX Entertainment Inc. & Quasar Inc.
//
// NOTE: Added few function based on Unity standard assets' 'Joystick' script.
//
// USAGE: Simply call CrossPlatformInputManager.GetAxis("Horizontal") or CrossPlatformInputManager.GetAxis("Vertical")
//        on the script you want to handle input from the joystick.
//
// IMPORTANT: Only works on Mobile Platform.
//
////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using UnityStandardAssets.CrossPlatformInput;

namespace Quasar.Input
{ 
    public class AdvancedJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        public enum AxisOption
        {
            // Options for which axes to use
            Both,                                                       // Use both
            OnlyHorizontal,                                             // Only horizontal
            OnlyVertical                                                // Only vertical
        }

        public AxisOption axesToUse = AxisOption.Both;                  // The options for the axes that the joystick will use
        public int MovementRangeRadius = 50;                            // Range of joystick.

        public enum JoystickType                                        // Availble joystick types.
        {
            Circle,
            Rectagle
        }

        public JoystickType joystickType = JoystickType.Circle;         // Joystick boundary type. Defaults 'Circle'.

        public Transform joystickSprite;                                // Set joystick sprite as you like
        [SerializeField] Transform[] spritesToCenter;                   // All sprites' transform that should be centered when joystick position is initialised.
        [SerializeField] Image touchAreaImage;

        public bool dynamicJoystick = true;                             // Is this joystick movable?

        Vector3 initialTouchPosition;                                   // Starting position when press the joystick pad
        bool shouldUseX;                                                // Toggle for using the x axis
        bool shouldUseY;                                                // Toggle for using the Y axis
        CrossPlatformInputManager.VirtualAxis horizontalVirtualAxis;    // Reference to the joystick in the cross platform input
        CrossPlatformInputManager.VirtualAxis verticalVirtualAxis;      // Reference to the joystick in the cross platform input

        [SerializeField] string horizontalAxisName = "Horizontal";      // The name given to the horizontal axis for the cross platform input
        [SerializeField] string verticalAxisName = "Vertical";          // The name given to the vertical axis for the cross platform input

        #region Unity Base Function
        void Awake()
        {
            initialTouchPosition = Vector3.zero;
        }

        void OnEnable()
        {
            CreateVirtualAxes();
        }

        void OnDisable()
        {
            if (shouldUseX) horizontalVirtualAxis.Remove(); 
            if (shouldUseY) verticalVirtualAxis.Remove();
        }
        #endregion Unity Base Function END

        #region User interaction event handlers
        public void OnPointerDown(PointerEventData eventData)
        {
            if (joystickSprite == null) return;

            if (dynamicJoystick)
            {
                initialTouchPosition = eventData.pointerCurrentRaycast.screenPosition;
                
                // Center sprites.
                int numberOfSprites = spritesToCenter.Length;
                for (int index = 0; index < numberOfSprites; index++)
                {
                    spritesToCenter[index].position = initialTouchPosition;
                }
            }
            else
            {
                initialTouchPosition = joystickSprite.position;
            }
        }

        public void OnDrag(PointerEventData data)
        {
            Vector3 newPos = Vector3.zero;

            if (shouldUseX)
            {
                int delta = (int)(data.position.x - initialTouchPosition.x);
                delta = Mathf.Clamp(delta, -MovementRangeRadius, MovementRangeRadius);
                newPos.x = delta;
            }

            if (shouldUseY)
            {
                int delta = (int)(data.position.y - initialTouchPosition.y);
                delta = Mathf.Clamp(delta, -MovementRangeRadius, MovementRangeRadius);
                newPos.y = delta;
            }

            if (joystickType == JoystickType.Rectagle)
            {
                joystickSprite.position = initialTouchPosition + newPos;
            }
            else
            {
                joystickSprite.position = Vector3.ClampMagnitude(newPos, MovementRangeRadius) + initialTouchPosition;
            }

            UpdateVirtualAxes(joystickSprite.position);
        }

        public void OnPointerUp(PointerEventData data)
        {
            joystickSprite.position = initialTouchPosition;

            UpdateVirtualAxes(initialTouchPosition);
        }
        #endregion User interaction event handlers END

        #region Virtual axes control
        void CreateVirtualAxes()
        {
            // set axes to use
            shouldUseX = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyHorizontal);
            shouldUseY = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyVertical);

            // create new axes based on axes to use
            if (shouldUseX)
            {
                if (CrossPlatformInputManager.AxisExists(horizontalAxisName))
                {
                    CrossPlatformInputManager.UnRegisterVirtualAxis(horizontalAxisName);
                }

                horizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
                CrossPlatformInputManager.RegisterVirtualAxis(horizontalVirtualAxis);
            }
            if (shouldUseY)
            {
                if (CrossPlatformInputManager.AxisExists(verticalAxisName))
                {
                    CrossPlatformInputManager.UnRegisterVirtualAxis(verticalAxisName);
                }

                verticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
                CrossPlatformInputManager.RegisterVirtualAxis(verticalVirtualAxis);
            }
        }

        void UpdateVirtualAxes(Vector3 value)
        {
            var delta = initialTouchPosition - value;
            delta.y = -delta.y;
            delta /= MovementRangeRadius;
            if (shouldUseX)
            {
                horizontalVirtualAxis.Update(-delta.x);
            }

            if (shouldUseY)
            {
                verticalVirtualAxis.Update(delta.y);
            }
        }
        #endregion Virtual axes control END

        public void DisplayJoystick(bool display)
        {
            touchAreaImage.raycastTarget = display;

            int numberOfSprites = spritesToCenter.Length;
            for (int index = 0; index < numberOfSprites; index++)
            {
                spritesToCenter[index].gameObject.SetActive(display);
            }

            CreateVirtualAxes();
        }
    }
}