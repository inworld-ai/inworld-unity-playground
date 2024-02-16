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
        const string sanFranciscoGeoEndpoint = "";
        const string newYorkCityGeoEndpoint = "";

        [SerializeField] int m_SanFranciscoPopulation;
        [SerializeField] int m_NewYorkCityPopulation;
        [SerializeField] InworldPlaygroundCharacter m_InworldPlaygroundCharacter;

        APIHandler m_APIHandler;
        UnityWebRequest.Result m_LastResult = 0;
        string m_LastResponseBody = "";

        void Awake()
        {
            m_APIHandler = GetComponent<APIHandler>();
        }

        void OnEnable()
        {
            m_APIHandler.onResponseEvent.AddListener(OnResponseEvent);
            m_InworldPlaygroundCharacter.onServerTrigger.AddListener(OnDataRetrievalBotServerTrigger);
        }
        
        void OnDisable()
        {
            m_APIHandler.onResponseEvent.RemoveListener(OnResponseEvent);
            if(m_InworldPlaygroundCharacter)
                m_InworldPlaygroundCharacter.onServerTrigger.RemoveListener(OnDataRetrievalBotServerTrigger);
        }

        void OnResponseEvent(UnityWebRequest.Result result, string body)
        {
            m_LastResult = result;
            m_LastResponseBody = body;
        }
        
        void OnDataRetrievalBotServerTrigger(string triggerName, Dictionary<string, string> parameters) 
        {
            switch (triggerName)
            {
                case "get_population":
                    if (!parameters.TryGetValue("city", out var city) || string.IsNullOrEmpty(city))
                    {
                        InworldController.Instance.SendTrigger("unsupported_city", m_InworldPlaygroundCharacter.ID, new Dictionary<string, string>
                        {
                            { "city", city }
                        });
                        break;
                    }
                    StartCoroutine((GetPopulation(city)));
                    break;
            }
        }
        
        IEnumerator GetPopulation(string city)
        {
            Debug.Log("Getting population data for: " + city);
            
            // string endpoint;
            switch (city)
            {
                case "San Francisco":
                    // endpoint = sanFranciscoGeoEndpoint;
                    GivePopulationInfo("San Francisco", m_SanFranciscoPopulation.ToString());
                    break;
                case "New York City":
                    // endpoint = newYorkCityGeoEndpoint;
                    GivePopulationInfo("New York City", m_NewYorkCityPopulation.ToString());
                    break;
                default:
                    yield break;
            }
            
            // Debug.Log("Sending Get Request: " + endpoint);
            
            // Get the population for the city.
            // yield return m_APIHandler.SendGetRequest(endpoint);
            // if (!HandleResponse(m_LastResult))
            //     yield break;
            //
            // try
            // {
            //     var population = m_LastResponseBody;
            //
            // }
            // catch (Exception e)
            // {
            //     HandleError(e.ToString());
            // }
        }

        void GivePopulationInfo(string city, string population)
        {
            var parameters = new Dictionary<string, string>
            {
                { "city", city },
                { "population", population }
            };

            InworldController.Instance.SendTrigger("provide_population", m_InworldPlaygroundCharacter.ID, parameters);
        }


        bool HandleResponse(UnityWebRequest.Result result)
        {
            if (result is not UnityWebRequest.Result.Success)
            {
                HandleError(m_LastResult.ToString());
                return false;
            }
            return true;
        }

        void HandleError(string errorMessage)
        {
            Debug.LogError("Web request failed: " + m_LastResult);
            var parameters = new Dictionary<string, string> { { "error", errorMessage } };
            InworldController.Instance.SendTrigger("error_fetching_data", m_InworldPlaygroundCharacter.ID, parameters);
        }
    }
}
