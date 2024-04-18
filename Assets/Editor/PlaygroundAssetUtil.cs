/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.IO;
using UnityEditor;
using UnityEngine;

namespace Inworld.Playground
{
    public static class PlaygroundAssetUtil
    {
        private const string pathToCSV = "/PlaygroundAssetNames.csv";
        
        [MenuItem("Util/RenamePlaygroundAssets")]
        static void RenamePlaygroundAssets()
        {
            string[] lines = File.ReadAllLines(Application.dataPath + pathToCSV);

            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] csvLineElements = line.Split(',');
                
                if(csvLineElements[1].Substring(csvLineElements[1].LastIndexOf('.')) == ".meta")
                    continue;

                string oldFileName = csvLineElements[1].Substring(0, csvLineElements[1].IndexOf('.'));
                string oldFileNameWithExtension = csvLineElements[1];
                string newFileName = csvLineElements[2].Substring(0, csvLineElements[2].IndexOf('.'));
                
                if(oldFileName == newFileName)
                    continue;
                
                // Debug.Log($"{oldFileNameWithExtension}, {newFileName}");

                string[] files = Directory.GetFiles(Application.dataPath, oldFileNameWithExtension, SearchOption.AllDirectories);

                if (files.Length == 0)
                {
                    Debug.Log($"Did not find any files with the name: {oldFileNameWithExtension}");
                    continue;
                }
                    
                for (int j = 0; j < files.Length; j++)
                {
                    string file = files[j].Remove(0, Application.dataPath.Length - "Assets".Length);
                    AssetDatabase.RenameAsset(file, newFileName);
                    Debug.Log($"Renamed asset at: `{file}` to {newFileName}");
                }
            }
            
            
        }
    }
}
