using UnityEngine;
using System.Collections.Generic;
using WheelOfFortune;

public static class Utility
{
    public static List<T> ShuffleItems<T>(List<T> items)
    {
        List<T> shuffledItems = new List<T>(items);

        for (int i = 0; i < shuffledItems.Count; i++)
        {
            T temp = shuffledItems[i];
            int randomIndex = Random.Range(i, shuffledItems.Count);
            shuffledItems[i] = shuffledItems[randomIndex];
            shuffledItems[randomIndex] = temp;
        }

        return shuffledItems;
    }

    public static List<WheelRewardItem> ConvertRewardDataToWheelRewardItems(RewardsData rewards)
    {
        List<WheelRewardItem> rewardItems = new List<WheelRewardItem>();

        for (int i = 0; i < rewards.rewards.Count; i++)
        {
            WheelRewardItem rewardItem = new WheelRewardItem
            {
                index = i,
                labelText = $"x{rewards.rewards[i].multiplier}",
                color = rewards.rewards[i].color,
                probability = rewards.rewards[i].probability
            };
            rewardItems.Add(rewardItem);
        }

        return rewardItems;
    }
}