/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace Inworld.Playground
{
    
    /// <summary>
    /// Handles sending requests to web servers.
    /// </summary>
    public class APIHandler : MonoBehaviour
    {

        #region Event
        /// <summary>
        /// Action is invoked when a response is received from a web request.
        /// Contains the result and text body of the response message.
        /// </summary>
        public event Action<UnityWebRequest.Result, string> OnResponseEvent;
        #endregion

        /// <summary>
        /// Sends a GET request to the provided URI.
        /// </summary>
        /// <param name="uri">The id of the web server to send this message to.</param>
        /// <returns>A coroutine which will complete when either communication finishes or an error occurs.</returns>
        public Coroutine SendGetRequest(string uri)
        {
            return StartCoroutine(WebGetRequest(uri));
        }
        
        private IEnumerator WebGetRequest(string uri)
        {
            using var request = UnityWebRequest.Get(uri);
            yield return request.SendWebRequest();
            OnResponseEvent(request.result, request.downloadHandler.text);
        }
    }
}

