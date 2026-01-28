# DotNetMaui.ConnectivityBanner

A lightweight animated connectivity status banner for .NET MAUI apps (.NET 8+).  
This control automatically detects internet connectivity changes and shows a smooth animated banner when the device goes offline or comes back online.

## Features

- Works with .NET MAUI (.NET 8+)
- Animated height-based banner (no translate glitches)
- Fully customizable via BindableProperties
- Safe lifecycle handling (no memory leaks)
- No permissions required
- Lightweight & production ready

## Installation

Install via .NET CLI:

```bash
dotnet add package DotNetMaui.ConnectivityBanner
```
Or via Package Manager Console:

```powershell
Install-Package DotNetMaui.ConnectivityBanner
```

## Usage

1. Add the namespace to your XAML page root:

```xml
xmlns:controls="clr-namespace:Maui.ConnectivityBanner.Controls;assembly=DotNetMaui.ConnectivityBanner"
```
2. Add the banner inside a Grid so it overlays your page content:
```xml
<Grid>
    <!-- Your Page Content -->
    

    <!-- Connectivity Banner -->
    <controls:ConnectivityView
        VerticalOptions="Start"
        ZIndex="100"
        BannerHeight="60"
        AnimationDuration="250"
        HideDelay="3000"
        OfflineText="No Internet Connection"
        OnlineText="Back Online"
        OfflineColor="Red"
        OnlineColor="Green"
        EnableAnimation="true" />
</Grid>

```
## Customization

All public BindableProperties can be customized in XAML or C#:

| Property            | Type   | Default                            | Description                                              |
| ------------------- | ------ | ---------------------------------- | -------------------------------------------------------- |
| `BannerHeight`      | double | 50                                 | Height of the banner                                     |
| `AnimationDuration` | int    | 250                                | Duration of show/hide animation in ms                    |
| `HideDelay`         | int    | 3000                               | Time banner stays visible after connection restored (ms) |
| `EnableAnimation`   | bool   | true                               | Enable/disable animation                                 |
| `OfflineColor`      | Color  | Red                                | Banner background when offline                           |
| `OnlineColor`       | Color  | Green                              | Banner background when online                            |
| `OfflineText`       | string | "Device Offline - Reconnecting..." | Text when offline                                        |
| `OnlineText`        | string | "Internet Restored"                | Text when online                                         |


## Notes

. Always place the banner inside a Grid with VerticalOptions="Start".
. Safe for multiple pages / repeated usage.
. Banner automatically subscribes and unsubscribes from connectivity events.
. Works on Android, iOS, MacCatalyst, and Windows.
. Banner height is adjustable via BannerHeight property.

## License
MIT License



