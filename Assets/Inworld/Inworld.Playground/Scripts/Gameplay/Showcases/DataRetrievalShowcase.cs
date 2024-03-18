/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections.Generic;
using Inworld.Playground.Data;
using UnityEngine;

namespace Inworld.Playground
{
    /// <summary>
    ///     Manages calling API and sending/receiving triggers for the Data Retrieval showcase.
    /// </summary>
    [RequireComponent(typeof(APIHandler))]
    public class DataRetrievalShowcase : MonoBehaviour
    {
        [SerializeField] InworldPlaygroundCharacter m_InworldPlaygroundCharacter;

        void OnEnable()
        {
            m_InworldPlaygroundCharacter.onServerTrigger.AddListener(OnDataRetrievalBotServerTrigger);
        }
        
        void OnDisable()
        {
            if(m_InworldPlaygroundCharacter)
                m_InworldPlaygroundCharacter.onServerTrigger.RemoveListener(OnDataRetrievalBotServerTrigger);
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
                    GetPopulation(city);
                    break;
            }
        }
        
        void GetPopulation(string cityName)
        {
            Debug.Log("Getting population data for: " + cityName);
            try
            {
                TextAsset populationDataTextAsset = Resources.Load<TextAsset>("PopulationData");
                var populationData = JsonUtility.FromJson<PopulationData>(populationDataTextAsset.text);
            
                foreach (City city in populationData.Cities)
                {
                    if (city.Name == cityName)
                    {
                        GivePopulationInfo(cityName, city.Population.ToString());
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Failed to get population data for: {cityName}, error: {e}");                
            }
        }

        void GivePopulationInfo(string cityName, string population)
        {
            var parameters = new Dictionary<string, string>
            {
                { "city", cityName },
                { "population", population }
            };

            InworldController.Instance.SendTrigger("provide_population", m_InworldPlaygroundCharacter.ID, parameters);
        }
    }
}
