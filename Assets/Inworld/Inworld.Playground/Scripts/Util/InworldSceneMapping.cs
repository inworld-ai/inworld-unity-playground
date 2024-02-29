/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;

/// <summary>
///     A mapping of a Unity scene name to an Inworld scene name.
///     Used for switching Inworld scenes when the Unity scene changes.
/// </summary>
[Serializable]
public class InworldSceneMapping
{
    /// <summary>
    ///     The name of the Unity scene.
    /// </summary>
    public string UnitySceneName;
    /// <summary>
    ///     The name of the Inworld scene.
    /// </summary>
    public string InworldSceneName;
}
