# Useful tools for Unity by Terry @ Quasar Inc.
_Copyright (c) 2015 Quasar Inc. & Terry Keetae Kwon All rights reserved._

This project is for Unity&reg; to provide a useful tools that allows game developers to save time making thoes on every project.
However, this project is not in any way endorsed or supervised by Unity Technologies.

_Unity&reg; is a trademark of Unity Technologies._

## Overview
* PlayFab authentication with verious platform with just one line of code. (__IMPORTANT__: API/Plugins provided by each organization are required)
  - [PlayFab](https://api.playfab.com/sdks/unity) - Tested with Version of 2.26.170814 
  - [Facebook](https://developers.facebook.com/docs/unity/) - Tested with Version of 7.9.4
  - [Google Play Service](https://github.com/playgameservices/play-games-plugin-for-unity) - Tested with Version of 0.9.40

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
