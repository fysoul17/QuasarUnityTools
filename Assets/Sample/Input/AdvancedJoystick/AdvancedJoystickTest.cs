using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityStandardAssets.CrossPlatformInput;
using Quasar.Input;

// IMPORTANT: Only works on Mobile Platform.
public class AdvancedJoystickTest : MonoBehaviour
{
    [SerializeField] AdvancedJoystick singleJoystick;

    [SerializeField] GameObject dualJoystickParent;
    [SerializeField] AdvancedJoystick[] dualJoysticks;

    [SerializeField] float speed = 10f;
    [SerializeField] Rigidbody targetRig;

    Vector2 moveVector = Vector2.zero;
    Vector2 rotationVector = Vector2.zero;

    bool useDual = false;

    public void UseSingleJoystick()
    {
        dualJoystickParent.gameObject.SetActive(false);
        useDual = false;

        singleJoystick.gameObject.SetActive(true);
        singleJoystick.DisplayJoystick(true);
    }

    public void UseDualJoystick()
    {
        singleJoystick.gameObject.SetActive(false);

        dualJoystickParent.gameObject.SetActive(true);
        useDual = true;

        int numberOfJoysticks = 2;
        for (int index = 0; index < numberOfJoysticks; index++)
        {
            dualJoysticks[index].DisplayJoystick(true);
        }
    }

    void FixedUpdate()
    {
        moveVector.x = CrossPlatformInputManager.GetAxis("Horizontal");
        moveVector.y = CrossPlatformInputManager.GetAxis("Vertical");

        if (useDual)
        {
            rotationVector.y = -CrossPlatformInputManager.GetAxis("HorizontalRotation");
            rotationVector.x = CrossPlatformInputManager.GetAxis("VerticalRotation");

            targetRig.AddTorque(rotationVector * speed);
        }

        targetRig.AddForce(moveVector * speed);
    }
}
