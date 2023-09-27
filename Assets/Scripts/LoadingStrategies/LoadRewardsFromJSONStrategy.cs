using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System;

namespace WheelOfFortune
{
    public class LoadRewardsFromJSONStrategy : ILoadRewardsStrategy
    {
        private string jsonFileData;

        // * To cache the results so we can use it multiple times if needed.
        private RewardsData rewardsData;

        public LoadRewardsFromJSONStrategy(string jsonFilePath)
        {
            if (File.Exists(jsonFilePath))
            {
                jsonFileData = File.ReadAllText(jsonFilePath);
                rewardsData = JsonConvert.DeserializeObject<RewardsData>(jsonFileData);
            }
            else
            {
                throw new Exception("Specified JSON file not found.");
            }
        }

        public RewardsData LoadRewards()
        {
            return rewardsData;
        }
    }
}