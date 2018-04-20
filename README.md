[logo]: https://raw.githubusercontent.com/Geeksltd/Zebble.AugmentedReality/master/Shared/NuGet/Icon.png "Zebble.AugmentedReality"


## Zebble.AugmentedReality

![logo]

A Zebble plugin that allows you to show a live feed from the camera, plus objects in certain Geo Locations.


[![NuGet](https://img.shields.io/nuget/v/Zebble.AugmentedReality.svg?label=NuGet)](https://www.nuget.org/packages/Zebble.AugmentedReality/)

> AugmentedReality is a fancy component which shows a live feed from the camera, plus objects in certain Geo Locations.
When you create a new instance, you then add a number of PointOfInterest items to it. Each PointOfInterest displays a custom view in a certain Longitude and Latitude.

<br>


### Setup
* Available on NuGet: [https://www.nuget.org/packages/Zebble.AugmentedReality/](https://www.nuget.org/packages/Zebble.AugmentedReality/)
* Install in your platform client projects.
* Available for iOS, Android.
<br>


### Api Usage

You can use below codes for using this plugin:
```csharp
var pointsOfInterest = new [] {
      new PointOfInterest { View = new Label("place 1"), Location = someGeoLocation },
      new PointOfInterest { View = new Label("place 2"), Location = someOtherGeoLocation }
 };

this.Add(new AugmentedReality(pointsOfInterest));
```
For more information please refer to https://www.mapbox.com
<br>

### Properties
| Property     | Type         | Android | iOS | Windows |
| :----------- | :----------- | :------ | :-- | :------ |
| PixelsPerDegree           | float          | x       | x   | x       |

<br>

### Methods
| Method       | Return Type  | Parameters                          | Android | iOS | Windows |
| :----------- | :----------- | :-----------                        | :------ | :-- | :------ |
| Add         | Task         | poi -> PointOfInterest | x       | x   | x        |
