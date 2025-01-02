# Inworld AI Unity Playground Demo
The Playground Demo is a great way to get started with the Inworld SDK for Unity. The project aims to serve as a frequently updated demo project to show off new Inworld features as they release.

For more in-depth documentation of the playground, visit the [Inworld AI docs](https://docs.inworld.ai/docs/tutorial-integrations/Unity/unity-playground/).

## Prerequisites
- [Unity](https://unity.com/download) 2022.2+
- [Playground Demo](https://github.com/inworld-ai/inworld-unity-playground/) Unity package file


## Quickstart

* For GitHub Repo:
Please follow the following steps:
1. clone this repo.
2. Navigate to the folder `inworld-unity-playground`.
3. Run `python importSDK.py`.
4. Open the Unity project `inworld-unity-playground`.

* For Unitypackage on Release page:
Create a new Unity project and import the [Playground Demo](https://github.com/inworld-ai/inworld-unity-playground/releases/latest) Unity package. Then select and run the **Setup** scene located at: `Assets/Inworld/Inworld.Playground/Scenes/Setup.unity` and follow the instructions.

## Install
***Warning:** Do not rename the Unity package file (`InworldAI.Playground.unitypackage`) or the auto-import process will fail.*

If you import the Playground Demo through the Unity package from the [Release](https://github.com/inworld-ai/inworld-unity-playground/releases/latest) all the required packages will be installed and settings will be updated for you.

In case something goes wrong during the install process, here are the required dependencies and settings:

## Update SDK
The playground references [Inworld Unity SDK](https://github.com/inworld-ai/inworld-unity-full/) as the submodule. By default, whenever it's fetched, the Submodule's version will never change.

If you'd like to upgrade SDK. Please follow the following steps:
1. Navigate to the folder `inworld-unity-playground/SDK`.
2. Run `git checkout main`.
3. Run `git pull`.

If you want to submit the change, please:
1. Navigate back to the folder `inworld-unity-playground`.
2. Run `git add SDK`.
Then you can commit and push.

### Required Components

#### Dependencies

- com.inworld.unity (use package included in project: `Assets/Inworld/com.inworld.unity.core-3.4.0.tgz`)
- [com.unity.cloud.gltfast](https://docs.unity3d.com/Packages/com.unity.cloud.gltfast@6.0/manual/index.html)
- [com.unity.probuilder](https://docs.unity3d.com/Packages/com.unity.probuilder@6.0/manual/index.html)
- [com.unity.ai.navigation](https://docs.unity3d.com/Packages/com.unity.ai.navigation@2.0/manual/)
- [com.unity.sentis](https://docs.unity3d.com/Packages/com.unity.sentis@1.4/manual/index.html) (if using Unity 2022.3 or newer)

#### Packages

- [TextMesh Pro](https://docs.unity3d.com/Manual/com.unity.textmeshpro.html) Essentials

#### Build Settings
Add all the scenes within `Assets/Inworld/Inworld.Playground/Scenes/` to the **Build Settings**. Scenes listed below:

- Setup
- Lobby
- Animations
- Avatars
- Goals
- Mutations
- Environment
- Scene
- Interactions

## Controls
- Movement: 'WASD' & 'Mouse'
- Select: 'Left Mouse Click'
- Open Chat Panel: '~'
- Settings: 'Esc'
- Push-to-Talk Key (if enabled): 'V'



