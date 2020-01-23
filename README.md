# Useful tools for Unity by Quasar Inc & GIX Inc.
_Copyright (c) 2015 Quasar Inc., GIX Inc., Terry Keetae Kwon All rights reserved._

This project is for Unity&reg; to provide a useful tools that allows game developers to save time making thoes on every project.
However, this project is not in any way endorsed or supervised by Unity Technologies.

_Unity&reg; is a trademark of Unity Technologies._

Working progress can be found at [Trello](https://trello.com/b/oTbv6v3Z/quasar-unity-tool)

## Overview

* PlayFab authentication with various platform with just one line of code. (__IMPORTANT__: API/Plugins provided by each organization are required)
  - [PlayFab](https://api.playfab.com/sdks/unity) - Tested with Version of 2.27.170828  
  - [Facebook](https://developers.facebook.com/docs/unity/) - Tested with Version of 7.9.4
  - [Google Play Service](https://github.com/playgameservices/play-games-plugin-for-unity) - Tested with Version of 0.9.40
  
* PlayFab Server time synchroniser. (__IMPORTANT__: Plugin is required)  
  - [JSON .NET for Unity](http://u3d.as/5q2) - JSON Parser. ($27.50 on Asset store)  
  - __Cloud script__ must be uploaded to PlayFab Server
  
* Many useful UI tools. (__IMPORTANT__: Some require plugins)
  - __Loading Indicator__ needs [iTween](http://u3d.as/1s9) - Tested with version of 2.0.7
  - Flexible tab window
  
* RPG tools that can be commonly used for RPG Games such as Stat system or inventory system.

* Utilities
  - Random selection from list of item with probabilities.
  - Big number formatter. (1,000 -> 1.00A, 1,000,000 -> 1.00B, ...)

* Input system
  - Advanced joystick (Single / Dual). (__IMPORTANT__: 'Standard Assets' is required)
    Import: Assets > (Right click) > Import Package > __CrossPlatformInput__
  - Swipe detector in 4 directions. 
  
* Classes that apply GoF design patterns. (Singleton, Command, etc..)

## PlayFab
```csharp
using Quasar.PlayFab
```
### Authentication

```csharp
using Quasar.PlayFab.Authentication;

public class Authentication : MonoBehaviour
{
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
}
```
### Manager
Allow multiple server API calls simultaneously, and throw callback when it is all finished.

1. Create and attach API calling script on appropriate GameObject.
```csharp
[SerializeField] PlayFabManager manager;

void Awake()
{
    // Register. Some data that has to be loaded before 'ScheduleLoading'
    manager.SchedulePreloading(OnReadyToPreLoad);
    
    // Register default data loading.
    manager.ScheduleLoading(OnReadyToLoad);
}

void OnReadyToPreLoad() 
{
    // API Calls..
    
    // Must call this when API call has finished, so that we can check whether all data is loaded.
    manager.FinishedPreloading();
}
```

2. Simply fire data fetch function on appropriate timing.
```csharp
[SerializeField] PlayFabManager playFabManager;

void OnLoggedIntoPlayFab() 
{
    yield return StartCoroutine(playFabManager.FetchDataFromServer(() =>
    {
        // One of loading is done.
    },
    () =>
    {
        // All loadings are done. Proceed next process.
    }));
}
```

### Server Time
Fetch server time and sync on client device, so that we can prevent time hacks on client side.
__Attach PlayFabManager, PlayFabTimer and ServerTimeSynchroniser scripts on GameObject__ then use as below:
```csharp
ServerTimeSynchroniser.ServerTime
```

## RPG
TBA

## UI
```csharp
using Quasar.UI
```
### Loading Indicator
Grab prefab named 'LoadingIndicator' and put it on Canvas which has __highest sorting order__ so that it can block inner UIs while showing indicator.

```csharp
public class UITest : MonoBehaviour
{
    [SerializeField] LoadingIndicator loadingIndicator;

    public void LoadSomething() 
    {
        // With Text: loadingIndicator.Display("Loading data from server...");
        loadingIndicator.Display();
        
        // Loading start.
        // Loading...
        // Loading done.
        
        loadingIndicator.Hide();
    }
}
```
### Tab Window
Grab prefab named 'Tab Window' and put it on Canvas. It has 3 tab buttons and 3 panels that interact to each button as a default.  
In order to make from scratch, just add 'Tab Window Controller' script and 'Tab Window Button' prefabs, then link buttons and panels using the controller script.


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
// 10,000,000,000 = 10.00 C (A, B, C ...)

int aIntValue = 10000000;
string formattedIntString = aIntValue.ToFormattedString("n2");
// 10,000,000 = 10.00 M (K, M, B, T)
```

## Input
```csharp
using Quasar.Input
```
### AdvancedJoystick  

Added few function based on Unity standard assets' 'Joystick' script as standard joystick only provides simple function whereas 'AdvancedJoystick' provides more such as 'Circle joystick', 'Single/dual joystick' and 'Dynamic joystick' function.  

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

## Patterns
```csharp
using Quasar.Patterns
```
### Singleton Pattern
By inheriting MonoSingleton class, you can easily implement singleton pattern.  
Source code is from [this site](http://wiki.unity3d.com/index.php/Singleton).
This script doesn't need to add 'using' as it does not use custom namespace.
```csharp
public class Singleton : MonoSingleton<Singleton>
{
    ...
}
```

### Observer Pattern
Simply add observers and notify on other scripts. Below is sample codes for usage.
```csharp
public class Player : MonoBehaviour
{
    void Start() 
    {
        EventHandler.Notify("OnPlayerAwaken", gameObject);
    }
}
```
```csharp
public class GameMaster : MonoBehaviour
{
    void Awake() 
    {
        EventHandler.AddObserver("OnPlayerAwaken", JoinBattle);
    }
    
    // IMPORTANT: Must remove when destoried as the event is static.
    void OnDestroy()
    {
        EventHandler.RemoveObserver("OnPlayerAwaken", JoinBattle);
    }
    
    void JoinBattle(params object[] args) 
    {
        Player player = (args[0] as GameObject).GetComponent<Player>();
        Debug.Log(player + " is joinning battle"); 
    }
}
```

### Command Pattern
Applies command pattern of GoF. Just a simple abstract class.
```csharp
public class SomeCommand : Command
{
    public override double TimeStamp
    {
        get;
        set;
    }
    
    public override void Execute()
    {
    }
    
    public override void Undo()
    {
    }
}
```

## Attribute
```csharp
using Quasar.Attribute
```
### Read Only Attribute
Prevents modifying value on Editor.
```csharp
[ReadOnlyProperty]
public int readOnlyValue = 10;
```

## Unity Assets
__In this folder, every assets are created by Unity Technologies and can be downloaded from thier webpage__  
### EditorWithSubEditors
This class acts as a base class for Editors that have Editors nested within them.

### Serialized Property Extensions
This script is created by Unity Technologies and added here to use at 'Interaction' module editor.  
