# Inworld AI Unity Playground Demo
The Playground Demo is a great way to get started with the Inworld SDK for Unity. The project aims to serve as a frequently updated demo project to show off new Inworld features as they release.

For more in-depth documentation of the playground, visit the [Inworld AI docs](https://docs.inworld.ai/docs/tutorial-integrations/Unity/unity-playground/).

## Prerequisites
- [Unity](https://unity.com/download) 2022.2+
- [Playground Demo](https://github.com/inworld-ai/inworld-unity-playground/) Unity package file

## Quickstart
Create a new Unity project and import the [Playground Demo](https://github.com/inworld-ai/inworld-unity-playground/releases/latest) Unity package. Then select and run the **Setup** scene located at: `Assets/Inworld/Inworld.Playground/Scenes/Setup.unity` and follow the instructions.

## Install
***Warning:** Do not rename the Unity package file (`InworldAI.Playground.unitypackage`) or the auto-import process will fail.*

If you import the Playground Demo through the Unity package from the [Release](https://github.com/inworld-ai/inworld-unity-playground/releases/latest) all the required packages will be installed and settings will be updated for you.

In case something goes wrong during the install process, here are the required dependencies and settings:

### Required Components

#### Dependencies

- [com.inworld.unity](https://github.com/inworld-ai/inworld-unity.git#yj3.2)
- [com.unity.cloud.gltfast](https://docs.unity3d.com/Packages/com.unity.cloud.gltfast@6.0/manual/index.html)
- [com.unity.probuilder](https://docs.unity3d.com/Packages/com.unity.probuilder@6.0/manual/index.html)
- [com.unity.ai.navigation](https://docs.unity3d.com/Packages/com.unity.ai.navigation@2.0/manual/)

#### Packages

- [TextMesh Pro](https://docs.unity3d.com/Manual/com.unity.textmeshpro.html) Essentials

#### Build Settings
Add all the scenes within `Assets/Inworld/Inworld.Playground/Scenes/` to the **Build Settings**. Scenes listed below:

- Setup
- Playground
- Animations
- Avatars
- Goals
- Mutations

### Common Issues

Upon installing the Playground Demo package you may come across multiple errors similar to the one below:
```
Problem detected while importing the Prefab file: 'Assets/Inworld/Inworld.Playground/Prefabs/Characters/Animations/gesture_george_inworld-playground.prefab'.
The file might be corrupt or have a missing Variant parent or nested Prefabs. See details below.
Errors:
	Nested Prefab problem. Missing Nested Prefab Asset: 'Armature (Missing Prefab with guid: 026a1ca2deceb73438540c38371b4dcb)'
```
These can be safely ignored, as the issue will be fixed once the [Unity glTFast package](https://docs.unity3d.com/Packages/com.unity.cloud.gltfast@6.0/manual/index.html) is installed.

## Controls
- Movement: 'WASD' & 'Mouse'
- Select: 'Left Mouse Click'
- Open Chat Panel: '~'
- Settings: 'Esc'
- Push-to-Talk Key (if enabled): 'V'



