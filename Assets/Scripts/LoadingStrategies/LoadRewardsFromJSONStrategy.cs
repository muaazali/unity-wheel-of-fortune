using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System;

namespace WheelOfFortune
{
    public class LoadRewardsFromJSONStrategy : ILoadRewardsStrategy
    {
        // * To cache the results so we can use it multiple times if needed.
        private RewardsData rewardsData;

        public LoadRewardsFromJSONStrategy(string jsonFileData)
        {
            Debug.Log(jsonFileData);
            rewardsData = JsonConvert.DeserializeObject<RewardsData>(jsonFileData);
        }

        public RewardsData LoadRewards()
        {
            return rewardsData;
        }
    }
}