/*************************************************************************************************
 * Copyright 2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;


namespace Inworld.Playground.Data
{
    [Serializable]
    public class PopulationData
    {
        public City[] cities;
        public string source;
    }

    [Serializable]
    public class City
    {
        public string name;
        public int population;
        public int year;
    }
}
