/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Inworld.Playground
{
    /// <summary>
    ///     Manages calling API and sending/receiving triggers for the Data Retrieval showcase.
    /// </summary>
    [RequireComponent(typeof(APIHandler))]
    public class DataRetrievalShowcase : MonoBehaviour
    {
        private const string sanFranciscoGeoEndpoint = "";
        private const string newYorkGeoEndpoint = "";
        
        [SerializeField] private InworldPlaygroundCharacter inworldPlaygroundCharacter;

        private APIHandler m_APIHandler;
        private UnityWebRequest.Result m_LastResult = 0;
        private string m_LastResponseBody = "";

        private void Awake()
        {
            m_APIHandler = GetComponent<APIHandler>();
        }

        private void OnEnable()
        {
            m_APIHandler.onResponseEvent.AddListener(OnResponseEvent);
            inworldPlaygroundCharacter.onServerTrigger.AddListener(OnDataRetrievalBotServerTrigger);
        }
        
        private void OnDisable()
        {
            m_APIHandler.onResponseEvent.RemoveListener(OnResponseEvent);
            if(inworldPlaygroundCharacter)
                inworldPlaygroundCharacter.onServerTrigger.RemoveListener(OnDataRetrievalBotServerTrigger);
        }

        private void OnResponseEvent(UnityWebRequest.Result result, string body)
        {
            m_LastResult = result;
            m_LastResponseBody = body;
        }
        
        private void OnDataRetrievalBotServerTrigger(string triggerName, Dictionary<string, string> parameters) 
        {
            switch (triggerName)
            {
                case "get_population":
                    if (!parameters.TryGetValue("city", out var city) || string.IsNullOrEmpty(city))
                    {
                        Debug.LogWarning("City is not supported.");
                        break;
                    }
                    StartCoroutine((GetPopulation(city)));
                    break;
            }
        }
        
        private IEnumerator GetPopulation(string city)
        {
            Debug.Log("Getting population data for: " + city);
            
            string endpoint;
            switch (city)
            {
                case "San Francisco":
                    endpoint = sanFranciscoGeoEndpoint;
                    break;
                case "New York City":
                    endpoint = newYorkGeoEndpoint;
                    break;
                default:
                    yield break;
            }
            
            Debug.Log("Sending Get Request: " + endpoint);
            
            // Get the population for the city.
            yield return m_APIHandler.SendGetRequest(endpoint);
            if (!HandleResponse(m_LastResult))
                yield break;
            
            try
            {
                var population = m_LastResponseBody;

            }
            catch (Exception e)
            {
                HandleError(e.ToString());
            }
        }

        private void GivePopulationInfo(string city, string population)
        {
            var parameters = new Dictionary<string, string>
            {
                { "city", city },
                { "population", population }
            };

            InworldController.Instance.SendTrigger("provide_population", inworldPlaygroundCharacter.ID, parameters);
        }


        private bool HandleResponse(UnityWebRequest.Result result)
        {
            if (result is not UnityWebRequest.Result.Success)
            {
                HandleError(m_LastResult.ToString());
                return false;
            }
            return true;
        }

        private void HandleError(string errorMessage)
        {
            Debug.LogError("Web request failed: " + m_LastResult);
            var parameters = new Dictionary<string, string> { { "error", errorMessage } };
            InworldController.Instance.SendTrigger("error_fetching_data", inworldPlaygroundCharacter.ID, parameters);
        }
    }
}
