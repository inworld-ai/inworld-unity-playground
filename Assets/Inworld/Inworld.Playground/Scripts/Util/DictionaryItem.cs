/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
namespace Inworld.Playground
{
    /// <summary>
    ///     A serializable representation of a dictionary item.
    /// </summary>
    [Serializable]
    public class DictionaryItem
    {
        public string Key;
        public string Value;
    }
}
