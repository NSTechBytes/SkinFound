
# SkinFound - Rainmeter Plugin

**SkinFound** is a Rainmeter plugin that checks for the presence of specified Rainmeter skins in the Rainmeter Skins folder. It can report the availability of each skin by name or provide full status output.

## Features

- Check for multiple Rainmeter skins by name.
- Customize the output to show either available skin names or detailed status.
- Integrates seamlessly with Rainmeter’s `#SKINSPATH#` variable for automatic folder path resolution.

## Installation

1. **Download and Build**: Clone the repository and compile the code to produce `SkinFound.dll`.
2. **Move DLL to Plugins Folder**: Place the compiled `SkinFound.dll` in your Rainmeter plugins folder, typically located at:
   ```
   C:\Program Files\Rainmeter\Plugins\
   ```
3. **Add to Your Skin**: Reference the plugin in your Rainmeter skin `.ini` file as shown below.

## Usage

### Basic Example

Below is a sample configuration file that demonstrates how to use the plugin to check for the presence of skins named `Clock`, `Illustro`, and `CustomSkin`.

```ini
[Rainmeter]
Update=1000
BackgroundMode=2
SolidColor=000000

[Metadata]
Name=SkinFound
Author=NS Tech Bytes
Version=1.0
Description=Checks if specific skins exist in the Rainmeter skins folder using the SkinFound plugin.

[mSkinFoundWhole]
Measure=Plugin
Plugin=SkinFound.dll
SkinName=Clock | Illustro | CustomSkin
Output=Whole

[mSkinFoundNameOnly]
Measure=Plugin
Plugin=SkinFound.dll
SkinName=Clock | Illustro | CustomSkin
Output=NameOnly

[mSkinFoundSumSkin]
Measure=Plugin
Plugin=SkinFound.dll
SkinName=#SkinList#
Output=SumSkin
OnUpdateAction=[!Log "[mSkinFoundSumSki]"]

[TextWhole]
Meter=String
MeasureName=mSkinFoundWhole
X=10
Y=10
FontColor=FFFFFF
FontSize=12
Text="Full Status: #CRLF#[mSkinFoundWhole]"

[TextNameOnly]
Meter=String
MeasureName=mSkinFoundNameOnly
X=10
Y=70
FontColor=FFFFFF
FontSize=12
Text="Available Skins: #CRLF#[mSkinFoundNameOnly]"
;Contain this only in Version 1.1 SkinFound.dll
[TextSumSkin]
Meter=String
MeasureName=mSkinFoundSumSkin
X=10
Y=150
FontColor=10,10,10
FontSize=12
Text="Available Skins: #CRLF#[mSkinFoundSumSkin]"
DynamicVariables=1
Antialias=1
```

### Configuration Options

#### Parameters for `[mSkinFound]` Measure

- **`SkinName`**: A list of skins to check, separated by `|`. Example: `Clock | Illustro | CustomSkin`.
- **`Output`**: 
  - **`Whole`**: Returns a detailed status of each skin, showing if it’s available or not.
  - **`NameOnly`**: Returns only the names of the available skins.

## Example Output

Assuming your Rainmeter Skins folder contains only `Clock` and `Illustro`, and `CustomSkin` is missing:

- **Output = Whole**:
  ```
  Clock: 1
  Illustro: 1
  CustomSkin: 0
  ```

- **Output = NameOnly**:
  ```
  Clock
  Illustro
  ```

## Building the Plugin

1. Open the project in Visual Studio.
2. Build the solution.
3. Copy `SkinFound.dll` to your Rainmeter plugins directory.

## License

This project is licensed under the MIT License. See `LICENSE` for details.

## Contributing

Contributions are welcome! If you have suggestions or improvements, please submit an issue or a pull request.

---

Enjoy using **SkinFound** to streamline your Rainmeter experience by checking for skins programmatically!
