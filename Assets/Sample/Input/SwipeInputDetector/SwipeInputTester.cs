using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Quasar.Input;

public class SwipeInputTester : MonoBehaviour
{
    [SerializeField] Text inputText;

    private void Awake()
    {
        SwipeInputDetector.InputDerectionCallback += HandleSwipeInput;
    }

    void HandleSwipeInput(InputDirection direction)
    {
        inputText.text = direction.ToString();    
    }
}
