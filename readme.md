# Wheel of Fortune - Unity
This is a generic implementation of wheel of fortune in Unity. It provides you with prefabs that can be used in any project and can be customized to suit your needs.

## Table of Contents
1. [Limitations](#limitations)
2. [Requirements](#requirements)
3. [How to Install](#how-to-install)
4. [How to Use](#how-to-use)
5. [Data Classes Definitions](#data-classes-definitions)

# Limitations
Currently, due to design constraints, this wheel of fortune is limited to only 8 maximim items.

Apart from that, the code is pretty tightly integrated with the given assets. <b>It is best to use the prefabs as it is without changing the design.</b>

I do plan to change it later on so the design assets can be easily replaced to match however you want them to be.

# Requirements
Following Unity packages need to be installed BEFORE in order for this package to work properly:
- TextMeshPro
- DOTween

# How to Install
1. Install the packages mentioned in the Requirements
2. Download the latest release of the .unitypackage from Releases tab and open it

# How to Use
This package contains the generic "Wheel of Fortune" prefab and an example "coin rewards screen" that implements this wheel of fortune to give players rewards. Both these elements can be used in any project if proper API is followed for them.

> In order to use this package, you will need to use the namespace `WheelOfFortune`

## Wheel of Fortune
The simple "Wheel of Fortune" prefab can be instantiated or dragged and dropped into the project to use.
1. Instantiate the wheel of fortune prefab.
2. Call `Initialize(List<WheelRewardItem>)` method to load the reward data.
3. Call the `StartSpin()` method to spin the wheel.
4. Use the [output events](#output-events) to communicate with other scripts.
Details of each step is given below.

### Initialization
In order to use the Wheel of Fortune, you will need to call the `Initialize(List<WheelRewardItem>)` method. This method creates the child 'reward' items. It takes a `List<WheelRewardItem>` as an input, which is the data of the items you want to display in the wheel. See the [definition](#data-classes-definitions) for `WheelRewardItem` below.
> WARNING! Do not call `Initialize` more than once. It will result in unexpected behaviour.

### Spin the wheel
To start spinning the wheel, use the method `StartSpin()`.

### Output Events
Wheel of Fortune provides two events to communicate with other scripts.
- <b>OnSpinStarted:</b>
Triggered whenever the spin is started.
- <b>OnSpinCompleted:</b>
Triggered whenever the spin is completed. It returns an `int index` which is the index of the 'winning/selected' item from the array you provided as an input.<br></br>

## Example Screen: Coin Rewards Screen
Coin Rewards Screen prefab is designed so it can be used in any project as well.
1. Instantiate the coin rewards screen prefab.
2. Call `Initialize(RewardsData)` method to load the reward data.
3. Use the [output events](#output-events-1) to communicate with other scripts.

### Initialization
To use it, simply call the `Initialize(RewardsData)` method after instantiating the prefab. It takes in [`RewardsData`](#data-classes-definitions).
> WARNING! Do not call `Initialize` more than once. It will result in unexpected behaviour.

> In the demo scene and it's UIController, the data for the coin rewards screen is being loaded from a JSON file. For the ease of usage, loading strategies have been created that can be explored in the `Scripts/LoadingStrategies` folder.<br></br>To create your own loading strategy, simply implement the interface `ILoadRewardsStrategy`.

### Output Events
- <b>GrantReward:</b>
Triggered when the spin is completed and the data in the multiplier and reward panel is filled. It returns an `int index` which is the index of the 'winning/selected' item from the array you provided as an input.<br></br>

## Data Classes Definitions
### WheelRewardItem
``` 
public struct WheelRewardItem
{
  public int index;
  public Sprite sprite;
  public string labelText;
  public string color;
  public float probability;
}
```

### RewardsData
```
public class RewardsData
{
  public int coins;
  public List<RewardItemData> rewards;
}

public class RewardItemData
{
  public float multiplier;
  public float probability;
  public string color;
  public int type;
}

```
