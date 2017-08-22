# Useful tools for Unity by Terry @ Quasar Inc & GIX Inc.
_Copyright (c) 2015 Quasar Inc., GIX Inc., Terry Keetae Kwon All rights reserved._

This project is for Unity&reg; to provide a useful tools that allows game developers to save time making thoes on every project.
However, this project is not in any way endorsed or supervised by Unity Technologies.

_Unity&reg; is a trademark of Unity Technologies._

## Overview
* PlayFab authentication with verious platform with just one line of code. (__IMPORTANT__: API/Plugins provided by each organization are required)
  - [PlayFab](https://api.playfab.com/sdks/unity) - Tested with Version of 2.26.170814 
  - [Facebook](https://developers.facebook.com/docs/unity/) - Tested with Version of 7.9.4
  - [Google Play Service](https://github.com/playgameservices/play-games-plugin-for-unity) - Tested with Version of 0.9.40

* Random selection from list of item with probabilities.
* Big number formatter. (1,000 -> 1.00A, 1,000,000 -> 1.00B, ...)
* Advanced joystick (Single / Dual). (__IMPORTANT__: 'Standard Assets' is required)
  - Assets > (Right click) > Import Package > __CrossPlatformInput__
  

## PlayFab
### Authentication

```csharp
using Quasar.PlayFab.Authentication;

public class Authentication : MonoBehaviour
{

    ...
    
    public void Authenticate() 
    {
        // PlayFab Custom
        (new PlayFabAuthentication("PlayFabTestAccount", "Test1234")).Login(OnLoginSuccess, OnLoginFailed, infoRequestParams);
        
        // Mobile Platform using device unique identifier
        (new MobilePlayFabAuthentication()).Login(OnLoginSuccess, OnLoginFailed, infoRequestParams);
        
        // Facebook
        new FacebookAuthentication().Login(OnLoginSuccess, OnLoginFailed, infoRequestParams);
        
        // GameCenter
        new GameCenterAuthentication().Login(OnLoginSuccess, OnLoginFailed, infoRequestParams);
        
        // Google Play
        new GooglePlayAuthentication().Login(OnLoginSuccess, OnLoginFailed, infoRequestParams);
    }
    
    ...
}
```

## Utilities
```csharp
using Quasar.Util
```
### QuasarRandom
```csharp
// Select the index randomly from the array of probabilities.
// ex) float[] p = { 0.1f, 0.3f, 0.5f, 0.1f } => each item has 10%, 30%, 50%, 10% chances to be selected.
int randomSelectedIndex = QuasarRandom.SelectRandomIndexWithProbability(emergeProbabilities);
```
### BigNumberFormatter
```csharp
// Returns formatted string of double number.
// Can be renamed by modifying 'enum BigNumberFormat'.
double aBigNumber = 10000000000d;
string formattedString = aBigNumber.ToFormattedString("n2");
// 10,000,000,000 = 10.00 C
```

## Input
```csharp
using Quasar.Input
```
### AdvancedJoystick  

Added few function based on Unity standard assets' 'Joystick' script as standard joystick only provides simple function whereas 'AdvancedJoystick' provides more such as 'Rectangular joystick', 'Single/dual joystick' and 'dynamic joystick' function.  

Check __Dual joystick Prefab__ in Sample folder if needed.    
__IMPORTANT__: As standard joystick does, it __only works on Mobile Platform.__  
```csharp
public enum JoystickType                // Availble joystick types.
{
    Circle,
    Rectagle
}
public bool dynamicJoystick             // Allows to move around touch area when user touches.
```

### Swipe Detector

Detects swipe input in 4 directions. Can be modified to 8 directions easily.
Simply attach the script to component that is raycast targets (eg. elements of canvas), and use it as below.

```csharp
private void Awake()
{
    SwipeInputDetector.InputDerectionCallback += HandleSwipeInput;
}

private void HandleSwipeInput(InputDirection direction)
{
    ...
}
```
