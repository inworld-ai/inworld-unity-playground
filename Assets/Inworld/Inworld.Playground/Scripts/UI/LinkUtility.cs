/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using UnityEngine;

namespace Inworld.Playground
{
    /// <summary>
    ///     Handles opening Web URLs.
    /// </summary>
    public class LinkUtility : MonoBehaviour
    {
        /// <summary>
        ///     Opens the URL in a web browser.
        /// </summary>
        /// <param name="url">The string URL to be opened.</param>
        public void OpenLink(string url)
        {
            Application.OpenURL(url);
        }
    }
}
